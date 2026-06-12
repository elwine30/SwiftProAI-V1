using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Notifications;
using Abp.Organizations;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization.Permissions;
using ThinknInsurTech.Authorization.Permissions.Dto;
using ThinknInsurTech.Authorization.Roles;
using ThinknInsurTech.Authorization.Users.Dto;
using ThinknInsurTech.Authorization.Users.Exporting;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Exporting;
using ThinknInsurTech.Net.Emailing;
using ThinknInsurTech.Notifications;
using ThinknInsurTech.Organizations.Dto;
using ThinknInsurTech.Runtime;
using ThinknInsurTech.Url;

namespace ThinknInsurTech.Authorization.Users
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UserAppService : ThinknInsurTechAppServiceBase, IUserAppService
    {
        public IAppUrlService AppUrlService { get; set; }

        private readonly RoleManager _roleManager;
        private readonly IUserEmailer _userEmailer;
        private readonly IUserListExcelExporter _userListExcelExporter;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRoleManagementConfig _roleManagementConfig;
        private readonly UserManager _userManager;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly IOptions<UserOptions> _userOptions;
        private readonly IEmailSettingsChecker _emailSettingsChecker;

        private readonly IUnitOfWorkManager _unitOfWorkManager;


        public UserAppService(
            RoleManager roleManager,
            IUserEmailer userEmailer,
            IUserListExcelExporter userListExcelExporter,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IRepository<RolePermissionSetting, long> rolePermissionRepository,
            IRepository<UserPermissionSetting, long> userPermissionRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRoleManagementConfig roleManagementConfig,
            UserManager userManager,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
            IOptions<UserOptions> userOptions, IEmailSettingsChecker emailSettingsChecker,
                        IUnitOfWorkManager unitOfWorkManager
)
        {
            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _userListExcelExporter = userListExcelExporter;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _rolePermissionRepository = rolePermissionRepository;
            _userPermissionRepository = userPermissionRepository;
            _userRoleRepository = userRoleRepository;
            _userPolicy = userPolicy;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _organizationUnitRepository = organizationUnitRepository;
            _roleManagementConfig = roleManagementConfig;
            _userManager = userManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
            _userOptions = userOptions;
            _emailSettingsChecker = emailSettingsChecker;
            _roleRepository = roleRepository;
            _unitOfWorkManager = unitOfWorkManager;
            AppUrlService = NullAppUrlService.Instance;
        }

        [HttpPost]
        public async Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input)
        {
            var query = GetUsersFilteredQuery(input);

            var userCount = await query.CountAsync();

            var currentUserOUId = AbpSession.GetCurrentOUId();

            if (currentUserOUId != null)
            {
                query = query.AsNoTracking()
                    .Where(u => _userOrganizationUnitRepository.GetAll().AsNoTracking().Any(uou => uou.UserId == u.Id && uou.OrganizationUnitId == currentUserOUId));
            }

            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
            await FillRoleNames(userListDtos);

            return new PagedResultDto<UserListDto>(
                userCount,
                userListDtos
            );
        }

        public async Task<FileDto> GetUsersToExcel(GetUsersToExcelInput input)
        {
            var query = GetUsersFilteredQuery(input);

            var users = await query
                .OrderBy(input.Sorting)
                .ToListAsync();

            var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
            await FillRoleNames(userListDtos);

            return _userListExcelExporter.ExportToFile(userListDtos, input.SelectedColumns);
        }

        public async Task<List<string>> GetUserExcelColumnsToExcel()
        {
            return await Task.FromResult(EntityExportHelper.GetEntityColumnNames<UserListDto>());
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Create, AppPermissions.Pages_Administration_Users_Edit)]
        public async Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input)
        {
            //Getting all available roles
            var userRoleDtos = await _roleManager.Roles
                .OrderBy(r => r.DisplayName)
                .Select(r => new UserRoleDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    RoleDisplayName = r.DisplayName
                })
                .ToArrayAsync();

            var allOrganizationUnits = await _organizationUnitRepository.GetAllListAsync();

            var output = new GetUserForEditOutput
            {
                Roles = userRoleDtos,
                AllOrganizationUnits = ObjectMapper.Map<List<OrganizationUnitDto>>(allOrganizationUnits),
                MemberedOrganizationUnits = new List<string>(),
                AllowedUserNameCharacters = _userOptions.Value.AllowedUserNameCharacters,
                IsSMTPSettingsProvided = await _emailSettingsChecker.EmailSettingsValidAsync()
            };

            if (!input.Id.HasValue)
            {
                // Creating a new user
                output.User = new UserEditDto
                {
                    IsActive = true,
                    ShouldChangePasswordOnNextLogin = true,
                    IsTwoFactorEnabled =
                        await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                            .TwoFactorLogin.IsEnabled),
                    IsLockoutEnabled =
                        await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut
                            .IsEnabled)
                };

                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    var defaultUserRole = userRoleDtos.FirstOrDefault(ur => ur.RoleName == defaultRole.Name);
                    if (defaultUserRole != null)
                    {
                        defaultUserRole.IsAssigned = true;
                    }
                }
            }
            else
            {
                //Editing an existing user
                var user = await UserManager.GetUserByIdAsync(input.Id.Value);

                output.User = ObjectMapper.Map<UserEditDto>(user);
                output.ProfilePictureId = user.ProfilePictureId;

                var organizationUnits = await UserManager.GetOrganizationUnitsAsync(user);
                output.MemberedOrganizationUnits = organizationUnits.Select(ou => ou.Code).ToList();

                var allRolesOfUsersOrganizationUnits = GetAllRoleNamesOfUsersOrganizationUnits(input.Id.Value);

                foreach (var userRoleDto in userRoleDtos)
                {
                    userRoleDto.IsAssigned = await UserManager.IsInRoleAsync(user, userRoleDto.RoleName);
                    userRoleDto.InheritedFromOrganizationUnit =
                        allRolesOfUsersOrganizationUnits.Contains(userRoleDto.RoleName);
                }
            }

            return output;
        }

        private List<string> GetAllRoleNamesOfUsersOrganizationUnits(long userId)
        {
            return (from userOu in _userOrganizationUnitRepository.GetAll()
                    join roleOu in _organizationUnitRoleRepository.GetAll() on userOu.OrganizationUnitId equals roleOu
                        .OrganizationUnitId
                    join userOuRoles in _roleRepository.GetAll() on roleOu.RoleId equals userOuRoles.Id
                    where userOu.UserId == userId
                    select userOuRoles.Name).ToList();
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var permissions = PermissionManager.GetAllPermissions();
            var grantedPermissions = await UserManager.GetGrantedPermissionsAsync(user);

            return new GetUserPermissionsForEditOutput
            {
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName)
                    .ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task ResetUserSpecificPermissions(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            await UserManager.ResetAllPermissionsAsync(user);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task UpdateUserPermissions(UpdateUserPermissionsInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var grantedPermissions =
                PermissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
            await UserManager.SetGrantedPermissionsAsync(user, grantedPermissions);
        }

        public async Task<long> CreateOrUpdateUser(CreateOrUpdateUserInput input)
        {
            if (input.User.Id.HasValue)
            {
                await UpdateUserAsync(input);
                return input.User.Id.Value;
            }
            else
            {
                var userId = await CreateUserAsync(input);
                return userId;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Delete)]
        public async Task DeleteUser(EntityDto<long> input)
        {
            if (input.Id == AbpSession.GetUserId())
            {
                throw new UserFriendlyException(L("YouCanNotDeleteOwnAccount"));
            }

            var user = await UserManager.GetUserByIdAsync(input.Id);
            CheckErrors(await UserManager.DeleteAsync(user));
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Unlock)]
        public async Task UnlockUser(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            user.Unlock();
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Edit)]
        protected virtual async Task UpdateUserAsync(CreateOrUpdateUserInput input)
        {
            Debug.Assert(input.User.Id != null, "input.User.Id should be set.");

            var user = await UserManager.FindByIdAsync(input.User.Id.Value.ToString());

            if (user is null)
            {
                throw new AbpException(L("UserNotFound"));
            }

            var isEmailChanged = user.EmailAddress != input.User.EmailAddress;

            if (isEmailChanged)
            {
                user.IsEmailConfirmed = false;
            }

            //Update user properties
            ObjectMapper.Map(input.User, user); //Passwords is not mapped (see mapping configuration)

            CheckErrors(await UserManager.UpdateAsync(user));

            if (input.SetRandomPassword)
            {
                var randomPassword = await _userManager.CreateRandomPassword();
                user.Password = _passwordHasher.HashPassword(user, randomPassword);
                input.User.Password = randomPassword;
            }
            else if (!input.User.Password.IsNullOrEmpty())
            {
                await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                CheckErrors(await UserManager.ChangePasswordAsync(user, input.User.Password));
            }

            //Update roles
            CheckErrors(await UserManager.SetRolesAsync(user, input.AssignedRoleNames));

            //update organization units
            //await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

            if (input.SendActivationEmail || isEmailChanged)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(
                    user,
                    AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                    input.User.Password
                );
            }
        }

        public async Task<long> CreateOnboardingUser(CreateOrUpdateUserInput input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {


                var user = ObjectMapper.Map<User>(input.User); //Passwords is not mapped (see mapping configuration)
                user.TenantId = AbpSession.TenantId;

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
                        CheckErrors(await validator.ValidateAsync(UserManager, user, input.User.Password));
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
                await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
                await _appNotifier.WelcomeToTheApplicationAsync(user);

                //Organization Units
                //await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());
                //Set the user organizationId to the creators organizationId


                if (input.OrganizationUnits.Count > 0)
                {
                    await UserManager.AddToOrganizationUnitAsync(user.Id, input.OrganizationUnits.FirstOrDefault());
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Create)]
        protected virtual async Task<long> CreateUserAsync(CreateOrUpdateUserInput input)
        {


            var currentUser = await UserManager.GetUserByIdAsync((long)AbpSession.UserId);
            var currentOrganizationUnit = await UserManager.GetOrganizationUnitsAsync(currentUser);

            if (AbpSession.TenantId.HasValue)
            {
                await _userPolicy.CheckMaxUserCountAsync(AbpSession.GetTenantId());
            }

            var user = ObjectMapper.Map<User>(input.User); //Passwords is not mapped (see mapping configuration)
            user.TenantId = AbpSession.TenantId;

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
                    CheckErrors(await validator.ValidateAsync(UserManager, user, input.User.Password));
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

            CheckErrors(await UserManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

            //Notifications
            await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
            await _appNotifier.WelcomeToTheApplicationAsync(user);

            //Organization Units
            //await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());
            //Set the user organizationId to the creators organizationId
            if (input.OrganizationUnits.Count > 0)
            {
                await UserManager.AddToOrganizationUnitAsync(user.Id, input.OrganizationUnits.FirstOrDefault());
            }
            else
            {
                // added null checking for super admin 
                if (currentOrganizationUnit.Count > 0)
                {
                    await UserManager.AddToOrganizationUnitAsync(user.Id, currentOrganizationUnit.FirstOrDefault().Id);
                }
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

        /// <summary>
        /// Retrieves both UserRoles and OrganizationUnitRoles entities for the specified user.
        /// This method ensures that both roles associated with the user (UserRoles entity) and 
        /// roles within organizational units (OrganizationUnitRoles entity) are fetched and processed together.
        /// </summary>
        private async Task FillRoleNames(List<UserListDto> userListDtos)
        {

            var userIds = userListDtos.Select(u => u.Id).ToList();


            var userRolesWithRoles = await (from userRole in _userRoleRepository.GetAll()
                                            join role in _roleRepository.GetAll()
                                            on userRole.RoleId equals role.Id
                                            where userIds.Contains(userRole.UserId)
                                            select new
                                            {
                                                UserId = userRole.UserId,
                                                RoleId = role.Id,
                                                RoleName = role.Name,
                                            }).ToListAsync();

            var userRoleswithOu = await (from userOu in _userOrganizationUnitRepository.GetAll()
                                         join userOuRole in _organizationUnitRoleRepository.GetAll()
                                         on userOu.OrganizationUnitId equals userOuRole.OrganizationUnitId
                                         join role in _roleRepository.GetAll() // Assuming _roleRepository is set up
                                         on userOuRole.RoleId equals role.Id
                                         where userIds.Contains(userOu.UserId)
                                         select new
                                         {
                                             UserId = userOu.UserId,
                                             RoleId = role.Id,
                                             RoleName = role.Name,
                                         }).ToListAsync();


            var combinedRoles = userRolesWithRoles.Union(userRoleswithOu).ToList();

            var rolesGroupedByUser = combinedRoles.GroupBy(cr => cr.UserId);


            foreach (var userListDto in userListDtos)
            {
                var userRoles = rolesGroupedByUser.FirstOrDefault(g => g.Key == userListDto.Id);
                if (userRoles != null)
                {
                    userListDto.Roles = userRoles.Select(r => new UserListRoleDto
                    {
                        RoleId = r.RoleId,
                        RoleName = r.RoleName
                    }).OrderBy(r => r.RoleName).ToList();
                }
            }
        }

        private IQueryable<User> GetUsersFilteredQuery(IGetUsersInput input)
        {
            var query = UserManager.Users
                .WhereIf(input.Role.HasValue, u => u.Roles.Any(r => r.RoleId == input.Role.Value))
                .WhereIf(input.OnlyLockedUsers,
                    u => u.LockoutEndDateUtc.HasValue && u.LockoutEndDateUtc.Value > DateTime.UtcNow)
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter)
                );

            if (input.Permissions != null && input.Permissions.Any(p => !p.IsNullOrWhiteSpace()))
            {
                var staticRoleNames = _roleManagementConfig.StaticRoles.Where(
                    r => r.GrantAllPermissionsByDefault &&
                         r.Side == AbpSession.MultiTenancySide
                ).Select(r => r.RoleName).ToList();

                input.Permissions = input.Permissions.Where(p => !string.IsNullOrEmpty(p)).ToList();

                var userIds = from user in query
                              join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                              from ur in urJoined.DefaultIfEmpty()
                              join urr in _roleRepository.GetAll() on ur.RoleId equals urr.Id into urrJoined
                              from urr in urrJoined.DefaultIfEmpty()
                              join up in _userPermissionRepository.GetAll()
                                  .Where(userPermission => input.Permissions.Contains(userPermission.Name)) on user.Id equals up.UserId into upJoined
                              from up in upJoined.DefaultIfEmpty()
                              join rp in _rolePermissionRepository.GetAll()
                                  .Where(rolePermission => input.Permissions.Contains(rolePermission.Name)) on
                                  new { RoleId = ur == null ? 0 : ur.RoleId } equals new { rp.RoleId } into rpJoined
                              from rp in rpJoined.DefaultIfEmpty()
                              where (up != null && up.IsGranted) ||
                                    (up == null && rp != null && rp.IsGranted) ||
                                    (up == null && rp == null && staticRoleNames.Contains(urr.Name))
                              group user by user.Id
                    into userGrouped
                              select userGrouped.Key;

                query = UserManager.Users.Where(e => userIds.Contains(e.Id));
            }

            return query;
        }
    }
}
