using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization.Roles;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Authorization.Users.Dto;
using ThinknInsurTech.Common;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Notifications;
using ThinknInsurTech.Organizations.Dto;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Url;

namespace ThinknInsurTech.Organizations
{
    public class OUOnboardingAppService : ThinknInsurTechAppServiceBase, IOUOnboardingAppService
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly IRepository<DocumentSetting> _documentSettingRepository;
        private readonly UserManager _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly RoleManager _roleManager;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IUserEmailer _userEmailer;
        private readonly IRepository<DeclarationQuestion> _declarationQuestionRepository;

        public IAppUrlService AppUrlService { get; set; }

        public OUOnboardingAppService(
            RoleManager roleManager,
            OrganizationUnitManager organizationUnitManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
            IRepository<Role, int> roleRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<DocumentSetting> documentSettingRepository,
            UserManager userManager,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IAppNotifier appNotifier,
            IUserEmailer userEmailer,
            IRepository<DeclarationQuestion> declarationQuestionRepository,
            INotificationSubscriptionManager notificationSubscriptionManager
        )
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
            _roleRepository = roleRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _documentSettingRepository = documentSettingRepository;
            _userManager = userManager;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _roleManager = roleManager;
            _appNotifier = appNotifier;
            _userEmailer = userEmailer;
            AppUrlService = NullAppUrlService.Instance;
            _declarationQuestionRepository = declarationQuestionRepository;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }

        public async Task CreateOnboardingOu(CreateOUOnboardingInput input)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    ValidateOUFields(input);
                    OrganizationUnitDto dataCreated = await CreateOrganizationUnit(input.OrganizationDto);
                    if (dataCreated != null)
                    {
                        input.DocumentSettingDto.organizationUnitId = dataCreated.Id;
                        input.AdminDto.OrganizationUnits.Add(dataCreated.Id);

                        await CreateDocumentSetting(input.DocumentSettingDto);
                        await CreateOnboardingUser(input.AdminDto);
                        await CreateDeclarationForm(dataCreated.Id);
                    }

                    await uow.CompleteAsync();
                }
                catch (Exception ex)
                {

                    throw new UserFriendlyException(ex.Message);
                }
            }
        }


        protected async Task<OrganizationUnitDto> CreateOrganizationUnit(
            CreateOrganizationUnitInput input
        )
        {
            using (
                _unitOfWorkManager.Current.DisableFilter(
                    ThinknInsurTechDataFilters.HaveOrganizationUnit
                )
            )
            {
                var organizationUnit = new OrganizationUnit(
                    AbpSession.TenantId,
                    input.DisplayName,
                    input.ParentId
                );

                await _organizationUnitManager.CreateAsync(organizationUnit);
                await CurrentUnitOfWork.SaveChangesAsync();
                if (organizationUnit.Id == 0)
                {
                    throw new Exception("OrganizationUnit ID was not set. Check if CreateAsync is correctly saving the entity.");
                }
                Role roleAssigned = new Role();

                if (input.CompanyType.ToLower() == "insurance")
                {
                    roleAssigned = await _roleRepository.FirstOrDefaultAsync(f => f.Name == "Insurer");
                }
                else if (input.CompanyType.ToLower() == "law firm")
                {
                    roleAssigned = await _roleRepository.FirstOrDefaultAsync(f => f.Name == "Lawyer");

                }
                else if (input.CompanyType.ToLower() == "workshop")
                {
                    roleAssigned = await _roleRepository.FirstOrDefaultAsync(f => f.Name == "Workshop");

                }
                else if (input.CompanyType.ToLower() == "adjuster")
                {
                    roleAssigned = await _roleRepository.FirstOrDefaultAsync(f => f.Name == "Adjuster");

                }

                var orgRole = new OrganizationUnitRole(
                        AbpSession.TenantId,
                        roleAssigned.Id,
                        organizationUnit.Id
                    );
                await _organizationUnitRoleRepository.InsertAsync(orgRole);

                return ObjectMapper.Map<OrganizationUnitDto>(organizationUnit);
            }
        }

        protected void ValidateOUFields(CreateOUOnboardingInput input)
        {
            using (
                _unitOfWorkManager.Current.DisableFilter(
                    ThinknInsurTechDataFilters.HaveOrganizationUnit
                )
            )
            {
                //OU Checking
                if (
                    input.OrganizationDto.CompanyType == null
                    || input.OrganizationDto.CompanyType == ""
                )
                {
                    throw new UserFriendlyException("Company Type Is Required");
                }
                if (_organizationUnitRepository.FirstOrDefault(f => f.DisplayName == input.OrganizationDto.DisplayName) != null)
                {
                    throw new UserFriendlyException($"{input.OrganizationDto.DisplayName} is registered.");
                }
                //Document Setting Checking
                if (
                    (
                        _documentSettingRepository.FirstOrDefault(f =>
                            f.businessRegistrationNo.ToLower()
                            == input.DocumentSettingDto.businessRegistrationNo.ToLower()
                        )
                    ) != null
                )
                {
                    throw new UserFriendlyException("Company have registered");
                }
                if (
                    (
                        _documentSettingRepository.FirstOrDefault(f =>
                            f.caseRefNoPrefix.ToLower()
                            == input.DocumentSettingDto.caseRefNoPrefix.ToLower()
                        )
                    ) != null
                )
                {
                    throw new UserFriendlyException(
                        $"{input.DocumentSettingDto.caseRefNoPrefix} is taken."
                    );
                }
            }
        }

        protected async Task CreateDocumentSetting(CreateOrEditDocumentSettingDto input)
        {
            using (
                _unitOfWorkManager.Current.DisableFilter(
                    ThinknInsurTechDataFilters.HaveOrganizationUnit
                )
            )
            {
                var documentSetting = ObjectMapper.Map<DocumentSetting>(input);

                if (AbpSession.TenantId != null)
                {
                    documentSetting.TenantId = (int)AbpSession.TenantId;
                }

                if (input.organizationUnitId != 0)
                {
                    await _documentSettingRepository.InsertAsync(documentSetting);
                    return;
                }
            }
        }

        private async Task CreateDeclarationForm(long organizationId)
        {
            using (UnitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var tenantId = 0;
                if (AbpSession.TenantId != null)
                {
                    tenantId = (int)AbpSession.TenantId;
                }

                var questions = new List<DeclarationQuestion>
                {
                    new DeclarationQuestion { TenantId = tenantId, OrganizationUnitId = organizationId, Question = "Are you familiar with the route?", OptionType = "RADIO", OptionValues = "Yes,No", CreationTime = DateTime.Now },
                    new DeclarationQuestion { TenantId = tenantId, OrganizationUnitId = organizationId, Question = "How was the traffic?", OptionType = "SINGLE_LINE", CreationTime = DateTime.Now },
                    new DeclarationQuestion { TenantId = tenantId, OrganizationUnitId = organizationId, Question = "Was the place brightly lit?", OptionType = "RADIO", OptionValues = "Yes,No", CreationTime = DateTime.Now },
                    new DeclarationQuestion { TenantId = tenantId, OrganizationUnitId = organizationId, Question = "Who are/is your passenger/s?", OptionType = "SINGLE_LINE", CreationTime = DateTime.Now },
                    new DeclarationQuestion { TenantId = tenantId, OrganizationUnitId = organizationId, Question = "Were they on official business?", OptionType = "RADIO", OptionValues = "Yes,No", CreationTime = DateTime.Now },
                    new DeclarationQuestion { TenantId = tenantId, OrganizationUnitId = organizationId, Question = "Were they injured?", OptionType = "RADIO", OptionValues = "Yes,No", CreationTime = DateTime.Now },
                    new DeclarationQuestion { TenantId = tenantId, OrganizationUnitId = organizationId, Question = "What was your speed prior to the accident?", OptionType = "SINGLE_LINE", CreationTime = DateTime.Now },
                    new DeclarationQuestion { TenantId = tenantId, OrganizationUnitId = organizationId, Question = "When did you first notice the third party vehicle?", OptionType = "SINGLE_LINE", CreationTime = DateTime.Now },
                    new DeclarationQuestion { TenantId = tenantId, OrganizationUnitId = organizationId, Question = "Did you take evasive measures to avoid the collision?", OptionType = "RADIO", OptionValues = "Yes,No", CreationTime = DateTime.Now },
                    new DeclarationQuestion { TenantId = tenantId, OrganizationUnitId = organizationId, Question = "Was there any pillion / passenger to the said vehicle?", OptionType = "RADIO", OptionValues = "Yes,No", CreationTime = DateTime.Now }
                };

                foreach (var question in questions)
                {
                    await _declarationQuestionRepository.InsertAsync(question);
                }
            }
        }


        private async Task<long> CreateOnboardingUser(CreateOrUpdateUserInput input)
        {
            using (
                _unitOfWorkManager.Current.DisableFilter(
                    ThinknInsurTechDataFilters.HaveOrganizationUnit
                )
            )
            {
                var user = ObjectMapper.Map<User>(input.User); //Passwords is not mapped (see mapping configuration)
                user.TenantId = AbpSession.TenantId;
                user.IsActive = true;

                //Set password
                if (input.SetRandomPassword)
                {
                    var randomPassword = await _userManager.CreateRandomPassword();
                    user.Password = _passwordHasher.HashPassword(user, randomPassword);
                    input.User.Password = randomPassword;
                }
                else if (!input.User.Password.IsNullOrEmpty())
                {
                    await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                    foreach (var validator in _passwordValidators)
                    {
                        CheckErrors(
                            await validator.ValidateAsync(UserManager, user, input.User.Password)
                        );
                    }

                    user.Password = _passwordHasher.HashPassword(user, input.User.Password);
                }

                user.ShouldChangePasswordOnNextLogin = input.User.ShouldChangePasswordOnNextLogin;

                //Assign roles
                user.Roles = new Collection<UserRole>();
                foreach (var roleName in input.AssignedRoleNames)
                {
                    var role = await _roleManager.GetRoleByNameAsync(roleName);
                    user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
                }

                await UserManager.CreateAsync(user);
                await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

                //Notifications
                await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(
                    user.ToUserIdentifier()
                );
                await _appNotifier.WelcomeToTheApplicationAsync(user);

                //Organization Units
                //await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());
                //Set the user organizationId to the creators organizationId


                if (input.OrganizationUnits.Count > 0)
                {
                    await UserManager.AddToOrganizationUnitAsync(
                        user.Id,
                        input.OrganizationUnits.FirstOrDefault()
                    );
                }

                //Send activation email
                if (input.SendActivationEmail)
                {
                    user.SetNewEmailConfirmationCode();
                    await _userEmailer.SendEmailActivationLinkAsync(
                        user,
                        AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                        input.User.Password
                    );
                }

                return user.Id;
            }
        }

    }
}
