using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Branches;
using ThinknInsurTech.Case;
using ThinknInsurTech.Common;
using ThinknInsurTech.Companies;
using ThinknInsurTech.Configuration;
using ThinknInsurTech.Registration.Dto;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Remarks;
using System.Text.RegularExpressions;
using Abp.Domain.Uow;
using ThinknInsurTech.Organizations;
using ThinknInsurTech.Runtime;
using ThinknInsurTech.Organizations.Dtos;
using ThinknInsurTech.LawFirms;
using ThinknInsurTech.Workshops;


namespace ThinknInsurTech.Registration
{
    [AbpAuthorize]
    public class MainRegistrationAppService : ThinknInsurTechAppServiceBase, IMainRegistrationAppService
    {
        private readonly IMainRegistrationManager _mainRegistrationManager;
        private readonly IRemarkManager _remarkManager;
        private readonly IRepository<MainRegistration, int> _mainRegistrationRepository;
        private readonly IRepository<Remark, int> _remarkRepository;
        private readonly IRepository<InsuranceCompany, int> _insuranceCompanyRepository;
        private readonly IRepository<CaseType, int> _caseTypeRepository;
        private readonly IRepository<Staff, int> _staffRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Branch, int> _branchRepository;
        private readonly UserManager _userManager;
        private readonly IAppConfigurationAccessor _configurationAccessor;
        private readonly IRepository<DocumentSetting> _documentSettingRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ViewThirdPartyCasesManager _mainRegistrationOrganizationUnitManager;
        private readonly IRepository<ViewThirdPartyCases, int> _viewThirdPartyCasesRepository;
        private readonly IRepository<LawFirm, int> _lawFirmRepository;
        private readonly IRepository<Workshop, int> _workshopRepository;

        public MainRegistrationAppService(
            IMainRegistrationManager mainRegistrationManager,
            IRemarkManager remarkManager,
            IRepository<MainRegistration, int> mainRegistrationRepository,
            IRepository<Remark, int> remarkRepository,
            IRepository<InsuranceCompany, int> insuranceCompanyRepository,
            IRepository<CaseType, int> caseTypeRepository,
            IRepository<Staff, int> staffRepository,
            IRepository<User, long> userRepository,
            IRepository<Branch, int> branchRepository,
            IAppConfigurationAccessor configurationAccessor,
            IRepository<DocumentSetting> documentSettingRepository,
            UserManager userManager,
            IUnitOfWorkManager unitOfWorkManager,
            ViewThirdPartyCasesManager mainRegistrationOrganizationUnitManager,
            IRepository<ViewThirdPartyCases, int> viewThirdPartyCasesRepository,
            IRepository<LawFirm, int> lawFirmRepository,
            IRepository<Workshop, int> workshopRepository
            )

        {
            _mainRegistrationManager = mainRegistrationManager;
            _remarkManager = remarkManager;
            _mainRegistrationRepository = mainRegistrationRepository;
            _remarkRepository = remarkRepository;
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _caseTypeRepository = caseTypeRepository;
            _staffRepository = staffRepository;
            _userRepository = userRepository;
            _branchRepository = branchRepository;
            _userManager = userManager;
            _configurationAccessor = configurationAccessor;
            _documentSettingRepository = documentSettingRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _mainRegistrationOrganizationUnitManager = mainRegistrationOrganizationUnitManager;
            _viewThirdPartyCasesRepository = viewThirdPartyCasesRepository;
            _lawFirmRepository = lawFirmRepository;
            _workshopRepository = workshopRepository;
        }

        public async Task<PagedResultDto<MainRegistrationDashboardDto>> GetMainRegistrationDetails(GetMainRegistrationDetailsInput input)
        {
            /*
                ***** Dashboard case table & Dashboard Summary
                Superadmin - see all cases in the system
                Admin (with no OU) - see all cases in system
                Admin (with OU) - see all cases in its OU
                Adjuster - see all cases created by the adjuster (filter by createdUserId) and also see cases assigned by admin
                3rd Party - see all cases assigned to the 3rd party company by adjusters
             */

            var currentOUId = AbpSession.GetCurrentOUId();
            var query = _mainRegistrationRepository.GetAll();
            query = query
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), t => t.VehicleNo.Contains(input.Filter) || t.Id.ToString().Contains(input.Filter))
                .WhereIf(input.AssignmentDateStart.HasValue, t => t.AssignTime >= input.AssignmentDateStart.Value)
                .WhereIf(input.AssignmentDateEnd.HasValue, t => t.AssignTime <= input.AssignmentDateEnd.Value)
                .WhereIf(input.StatusIdSpecified, t => t.StatusId == input.StatusId)
                .WhereIf(input.CompanyIdSpecified, t => t.CompanyId == input.CompanyId)
                .WhereIf(input.AdjusterIdSpecified, t => t.AdjusterMemberId == input.AdjusterMemberId)
                .WhereIf(input.EditorIdSpecified, t => t.EditorMemberId == input.EditorMemberId);
            IQueryable<MainRegistrationDashboardDto> caseQuery;

            if (currentOUId == null)
            {
                Console.WriteLine("Get All cases for admin with no OU");

                using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
                {
                    caseQuery = BuildDashboardDto(query);
                }
            }
            else
            {
                var user = UserManager.GetUser(AbpSession.ToUserIdentifier());
                await UserManager.IsInRoleAsync(GetCurrentUser(), "admin");
                var userRoles = await UserManager.GetRolesAsync(user);

                if (userRoles.Contains("Admin"))
                {
                    // cater adjuster's admin and 3rd party's admin
                    // filter by OU
                    //using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))

                    var company = await _documentSettingRepository.FirstOrDefaultAsync(x => x.OrganizationUnitId == currentOUId);

                    if(company.companyType == "Adjuster")
                    {
                        caseQuery = query.Join(_insuranceCompanyRepository.GetAll().AsNoTracking(), mr => mr.CompanyId, cr => cr.Id, (mr, cr) => new { mr, cr })
                        .Join(_userRepository.GetAll().AsNoTracking(), joined => joined.mr.AdjusterMemberId, user => user.Id, (joined, user) => new { joined.mr, joined.cr, user })
                        .Where(x => !x.cr.IsDeleted)
                        .Select(x => new MainRegistrationDashboardDto()
                        {
                            CaseTypeId = x.mr.CaseTypeId,
                            CaseTypeShortName = x.mr.CaseType.ShortName,
                            BranchId = x.mr.BranchId,
                            BranchShortName = x.mr.Branch.ShortName,
                            CompanyId = x.mr.CompanyId,
                            CompanyShortName = x.mr.Company.ShortName,
                            VehicleNo = x.mr.VehicleNo,
                            ModeOfAssignment = x.mr.ModeOfAssignment,
                            AdjusterUserName = x.user.Name,
                            EditorMemberId = x.mr.EditorMemberId,
                            StatusId = x.mr.StatusId,
                            StatusCode = x.mr.Status.Code ?? "",
                            AssignTime = x.mr.AssignTime,
                            Id = x.mr.Id,
                            Prefix = x.mr.Prefix,
                            CaseNo = x.mr.CaseNo,
                        });
                    }
                    else
                    {
                        // third party
                        using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
                        {
                            var thirdPartyCaseQuery = query
                            .Join(_viewThirdPartyCasesRepository.GetAll().AsNoTracking(), mr => mr.Id, vtpc => vtpc.RegisterId, (mr, vtpc) => new { mr, vtpc })
                            .Join(_userRepository.GetAll().AsNoTracking(), y => y.mr.AdjusterMemberId, user => user.Id, (joined, user) => new { joined.mr, joined.vtpc, user })
                            .Where(x => x.vtpc.AssignedOUId == currentOUId) // filter by AssignedOUId, AssignedOUId != null after request is approved
                            .Select(j => new MainRegistrationDashboardDto
                            {
                                CaseTypeId = j.mr.CaseTypeId,
                                CaseTypeShortName = j.mr.CaseType.ShortName,
                                BranchId = j.mr.BranchId,
                                BranchShortName = j.mr.Branch.ShortName,
                                CompanyId = j.mr.CompanyId,
                                CompanyShortName = j.mr.Company.ShortName,
                                VehicleNo = j.mr.VehicleNo,
                                ModeOfAssignment = j.mr.ModeOfAssignment,
                                AdjusterUserName = j.user.Name,
                                EditorMemberId = j.mr.EditorMemberId,
                                StatusId = j.mr.StatusId,
                                StatusCode = j.mr.Status.Code ?? "",
                                AssignTime = j.mr.AssignTime,
                                Id = j.mr.Id,
                                Prefix = j.mr.Prefix,
                                CaseNo = j.mr.CaseNo,
                                IsReadOnly = true,
                            });

                            input.Sorting = input.Sorting.Replace(".", "");

                            var thirdPartyRegCount = await thirdPartyCaseQuery.CountAsync();

                            var thirdPartyRegDetails = await thirdPartyCaseQuery.OrderBy(input.Sorting).PageBy(input.SkipCount, input.MaxResultCount).ToListAsync();

                            return new PagedResultDto<MainRegistrationDashboardDto>(thirdPartyRegCount, thirdPartyRegDetails);
                        }
                    }
                }
                else if (userRoles.Contains("Adjuster"))
                {
                    // filter by creatorId (adjuster view cases created) & adjusterId (cases assigned to adjuster by other adjusters or admin)
                    caseQuery = BuildDashboardDto(query.Where(x => x.AdjusterMemberId == AbpSession.UserId || x.CreatorUserId == AbpSession.UserId));
                }
                else
                {
                    using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
                    {

                        caseQuery = query
                        .Join(_viewThirdPartyCasesRepository.GetAll().AsNoTracking(), mr => mr.Id, vtpc => vtpc.RegisterId, (mr, vtpc) => new { mr, vtpc })
                        .Join(_userRepository.GetAll().AsNoTracking(), y => y.mr.AdjusterMemberId, user => user.Id, (joined, user) => new { joined.mr, joined.vtpc, user })
                        .Where(x => x.vtpc.AssignedOUId == currentOUId) // filter by AssignedOUId, AssignedOUId != null after request is approved
                        .Select(j => new MainRegistrationDashboardDto
                        {
                            CaseTypeId = j.mr.CaseTypeId,
                            CaseTypeShortName = j.mr.CaseType.ShortName,
                            BranchId = j.mr.BranchId,
                            BranchShortName = j.mr.Branch.ShortName,
                            CompanyId = j.mr.CompanyId,
                            CompanyShortName = j.mr.Company.ShortName,
                            VehicleNo = j.mr.VehicleNo,
                            ModeOfAssignment = j.mr.ModeOfAssignment,
                            AdjusterUserName = j.user.Name,
                            EditorMemberId = j.mr.EditorMemberId,
                            StatusId = j.mr.StatusId,
                            StatusCode = j.mr.Status.Code ?? "",
                            AssignTime = j.mr.AssignTime,
                            Id = j.mr.Id,
                            Prefix = j.mr.Prefix,
                            CaseNo = j.mr.CaseNo,
                            IsReadOnly = true,
                        });

                        caseQuery = caseQuery.Distinct(); // distinct to cater for multiple of same lawfirm in a case, adjuster can assign the same lawfirm multiple times maybe due to different lawyer

                        input.Sorting = input.Sorting.Replace(".", "");

                        var thirdPartyRegCount = await caseQuery.CountAsync();
                        var thirdPartyRegDetails = await caseQuery.OrderBy(input.Sorting).PageBy(input.SkipCount, input.MaxResultCount).ToListAsync();

                        return new PagedResultDto<MainRegistrationDashboardDto>(thirdPartyRegCount, thirdPartyRegDetails);
                    }
                }
            }

            input.Sorting = input.Sorting.Replace(".", "");

            var registrationCount = await caseQuery.CountAsync();
            var registrationDetails = await caseQuery.OrderBy(input.Sorting).PageBy(input.SkipCount, input.MaxResultCount).ToListAsync();

            return new PagedResultDto<MainRegistrationDashboardDto>(registrationCount, registrationDetails);
        }

        public async Task<Dictionary<int, int>> GetMainRegistrationDashboardSummary()
        {
            /*
                ***** Dashboard case table & Dashboard Summary
                Superadmin - see all cases in the system
                Admin (with no OU) - see all cases in system
                Admin (with OU) - see all cases in its OU
                Adjuster - see all cases created by the adjuster (filter by createdUserId) and also see cases assigned by admin
                3rd Party - see all cases assigned to the 3rd party company by adjusters
             */

            var currentOUId = AbpSession.GetCurrentOUId();
            var query = _mainRegistrationRepository.GetAll()
                .Join(_userRepository.GetAll().AsNoTracking(), mr => mr.AdjusterMemberId, ur => ur.Id, (mr, ur) => new { mr })
                .Join(_insuranceCompanyRepository.GetAll().AsNoTracking(), joined => joined.mr.CompanyId, cr => cr.Id, (joined, cr) => new { joined.mr, cr })
                .Where(x => !x.cr.IsDeleted);

            if (currentOUId == null)
            {
                using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
                {
                    return await query
                        .GroupBy(joined => joined.mr.StatusId)
                        .ToDictionaryAsync(group => group.Key.Value, group => group.Count());
                }
            }
            else
            {
                var user = UserManager.GetUser(AbpSession.ToUserIdentifier());
                await UserManager.IsInRoleAsync(GetCurrentUser(), "admin");
                var userRoles = await UserManager.GetRolesAsync(user);

                if (userRoles.Contains("Admin"))
                {
                    var company = await _documentSettingRepository.FirstOrDefaultAsync(x => x.OrganizationUnitId == currentOUId);

                    if(company.companyType == "Adjuster")
                    {
                        return await query
                        .GroupBy(joined => joined.mr.StatusId)
                        .ToDictionaryAsync(group => group.Key.Value, group => group.Count());
                    }
                    else
                    {
                        // third party admin
                        using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
                        {
                            return await query
                            .Join(_viewThirdPartyCasesRepository.GetAll(), joined => joined.mr.Id, vtpc => vtpc.RegisterId, (joined, vtpc) => new { joined.mr, vtpc })
                            .Where(x => x.vtpc.AssignedOUId == AbpSession.GetCurrentOUId())
                            .GroupBy(joined => joined.mr.StatusId)
                            .ToDictionaryAsync(group => group.Key.Value, group => group.Count());
                        }
                    }

                }
                else if (userRoles.Contains("Adjuster"))
                {
                    return await query
                        .Where(x => x.mr.AdjusterMemberId == AbpSession.UserId)
                        .GroupBy(joined => joined.mr.StatusId)
                        .ToDictionaryAsync(group => group.Key.Value, group => group.Count());
                }
                else
                {
                    using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
                    {
                        return await query
                        .Join(_viewThirdPartyCasesRepository.GetAll(), joined => joined.mr.Id, vtpc => vtpc.RegisterId, (joined, vtpc) => new { joined.mr, vtpc })
                        .Where(x => x.vtpc.AssignedOUId == AbpSession.GetCurrentOUId())
                        .GroupBy(joined => joined.mr.StatusId)
                        .ToDictionaryAsync(group => group.Key.Value, group => group.Count());
                    } 
                }
            }
        }


        public async Task<int> CreateMainRegistration(CreateMainRegistrationInput registration)
        {
            var tenantPrefix = _configurationAccessor.Configuration["TenantPrefixSetting:Prefix"];

            var lookupAdjuster = await _staffRepository.GetAll().Where(s => s.UserId == registration.AdjusterMemberId).Select(staff => staff.UserFk.UserName).FirstOrDefaultAsync();
            var lookupCaseType = await _caseTypeRepository.GetAll().Where(caseType => caseType.Id == registration.CaseTypeId).Select(caseType => caseType.ShortName).FirstOrDefaultAsync();
            var lookupBranch = await _branchRepository.GetAll().Where(branch => branch.Id == registration.BranchId).Select(branch => branch.ShortName).FirstOrDefaultAsync();
            var lookupCompany = await _insuranceCompanyRepository.GetAll().Where(company => company.Id == registration.CompanyId).Select(company => company.ShortName).FirstOrDefaultAsync();


            long? organizationUnitId = null;

            // Initialize default values
            var prefix = _configurationAccessor.Configuration["CaseNoSetting:Prefix"];
            var length = Convert.ToInt32(_configurationAccessor.Configuration["CaseNoSetting:CaseNoLength"]);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                organizationUnitId = AbpSession.GetCurrentOUId().Value;
                var documentSettings = await _documentSettingRepository.FirstOrDefaultAsync(p => p.OrganizationUnitId == organizationUnitId);
                if (documentSettings != null)
                {
                    // Update prefix and length if document settings exist
                    prefix = documentSettings.caseRefNoPrefix ?? prefix;
                    length = documentSettings.caseRefNoLength ?? length;
                }
            }

            IQueryable<int> query = _mainRegistrationRepository.GetAll()
                .Select(mr => Convert.ToInt32(mr.CaseNo));

            int currentMaxCaseNo = 0;

            if (!query.IsNullOrEmpty())
                currentMaxCaseNo = query.Max();

            string caseNo = (currentMaxCaseNo + 1).ToString().PadLeft(length, '0');

            var fileRefNo = tenantPrefix + "/" + lookupCaseType + "/" + lookupCompany + "/" + caseNo + "/" + registration.AssignTime.Month.ToString() + "/" + registration.AssignTime.Day.ToString() + "/" + lookupBranch + "(" + lookupAdjuster + ")";
            var currentUserIdentifier = AbpSession.ToUserIdentifier();
            var sourceRegistration = new MainRegistration
            {
                CaseTypeId = registration.CaseTypeId,
                MemberId = currentUserIdentifier.UserId,
                BranchId = registration.BranchId,
                CompanyId = registration.CompanyId,
                VehicleNo = registration.VehicleNo,
                AssignTime = registration.AssignTime,
                CompletionTime = registration.CompletionTime,
                ModeOfAssignment = registration.ModeOfAssignment,
                AdjusterMemberId = registration.AdjusterMemberId,
                EditorMemberId = registration.EditorMemberId,
                StatusId = registration.StatusId ?? (int)EnumRegistrationStatus.UnderInvestigation,
                Prefix = (prefix.IsNullOrEmpty() ? null : prefix),
                CaseNo = caseNo,
                FileRefNo = fileRefNo,
                OrganizationUnitId = organizationUnitId
            };

            var id = await _mainRegistrationManager.CreateMainRegistrationAsync(sourceRegistration);

            // add MainRegistrationOU record for insurance company

            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var company = await _insuranceCompanyRepository.FirstOrDefaultAsync(x => x.Id == registration.CompanyId);

                // if company assignOUId is not null, means the view3rdpartycaserequest is approved 
                // only after insurance company onboard and approve the binding between insurance company and adjuster's company master data, then will add record in ViewThirdPartyCases
                // MainRegistrationOU == ViewThirdPartyCase
                if(company.AssignOUId != null)
                {
                    var entity = new ViewThirdPartyCases()
                    {
                        RegisterId = id,
                        AssignedOUId = company.AssignOUId,
                        TenantId = (int?)AbpSession.TenantId,
                        CreatorUserId = (int?)AbpSession.UserId
                    };

                    await _mainRegistrationOrganizationUnitManager.CreateMainRegistrationOU(entity);
                }
            }

            if (!registration.RemarkDescription.IsNullOrEmpty())
            {
                var sourceRemark = new Remark
                {
                    RegisterId = id,
                    Description = registration.RemarkDescription,
                };
                await _remarkManager.CreateRemarkAsync(sourceRemark);
            }

            return id;
        }

        public async Task UpdateStatus(int registerId)
        {
            var sourceRegistration = await _mainRegistrationRepository.FirstOrDefaultAsync(registerId);
            if (sourceRegistration == null)
            {
                throw new Exception("Registration case not found");
            }

            var statusList = (EnumRegistrationStatus[])Enum.GetValues(typeof(EnumRegistrationStatus));
            var currentStatusId = Array.IndexOf(statusList, (EnumRegistrationStatus)sourceRegistration.StatusId);

            //Minus 2 becasue we dont want to include Cancelled into the update status.
            if (currentStatusId < statusList.Length - 1)
            {
                sourceRegistration.StatusId = (int)statusList[currentStatusId + 1];
            }
            await _mainRegistrationRepository.UpdateAsync(sourceRegistration);
        }

        public MainRegistrationDto GetMainRegistrationDetailsByRegisterId(int registerId)
        {
            var items = _mainRegistrationRepository.Get(registerId);
            var remarkItem = _remarkRepository.GetAll()
                .Where(x => x.RegisterId.Equals(registerId))
                .OrderByDescending(x => x.CreationTime).FirstOrDefault();
            if (remarkItem == null)
            {

            }

            var mainRegistration = new MainRegistrationDto
            {
                Id = items.Id,
                VehicleNo = items.VehicleNo,
                BranchId = items.BranchId,
                CaseTypeId = items.CaseTypeId,
                CompanyId = items.CompanyId,
                ModeOfAssignment = items.ModeOfAssignment,
                AdjusterMemberId = items.AdjusterMemberId,
                EditorMemberId = items.EditorMemberId,
                AssignTime = items.AssignTime,
                CompletionTime = items.CompletionTime,
                StatusId = items.StatusId,
                RemarkId = (remarkItem != null ? remarkItem.Id.ToString() : null),
                RemarkDescription = (remarkItem != null ? remarkItem.Description : null),
                FileRefNo = items.FileRefNo
            };

            return mainRegistration;
        }

        public async Task UpdateCaseCompany(ReassignCaseCompanyDto data)
        {
            var sourceRegistration = await _mainRegistrationRepository.FirstOrDefaultAsync(data.RegistrationId);
            var newCompanyShortName = await _insuranceCompanyRepository.GetAll().Where(company => company.Id == data.CompanyId).Select(company => company.ShortName).FirstOrDefaultAsync();
            var fileRefNo = sourceRegistration.FileRefNo;

            if (fileRefNo != null)
            {
                string pattern = @"(?<=/.*/)[^/]+(?=/[^/]*/[^/]*/[^/]*/[^/]*\(.+\))";
                sourceRegistration.FileRefNo = Regex.Replace(fileRefNo, pattern, newCompanyShortName);
            }
            sourceRegistration.CompanyId = data.CompanyId;
            var currentUserIdentifier = AbpSession.ToUserIdentifier();
            var remark = new Remark
            {
                RegisterId = data.RegistrationId,
                Description = data.Remark.Description,
                CreatorUserId = currentUserIdentifier.UserId,
                CreationTime = DateTime.Now,

            };
            await _remarkManager.CreateRemarkAsync(remark);
        }

        public async Task UpdateCaseAdjuster(ReassignCaseAdjusterDto data)
        {
            var sourceRegistration = await _mainRegistrationRepository.FirstOrDefaultAsync(data.RegistrationId);
            var newAdjusterUserName = await _staffRepository.GetAll().Where(s => s.UserId == data.AdjusterId).Select(staff => staff.UserFk.UserName).FirstOrDefaultAsync();
            var fileRefNo = sourceRegistration.FileRefNo;

            if (fileRefNo != null)
            {
                string pattern = @"(?<=\()[^)]+(?=\))";
                sourceRegistration.FileRefNo = Regex.Replace(fileRefNo, pattern, newAdjusterUserName);
            }
            sourceRegistration.AdjusterMemberId = data.AdjusterId;
            var currentUserIdentifier = AbpSession.ToUserIdentifier();
            var remark = new Remark
            {
                RegisterId = data.RegistrationId,
                Description = data.Remark.Description,
                CreatorUserId = currentUserIdentifier.UserId,
                CreationTime = DateTime.Now,

            };
            await _remarkManager.CreateRemarkAsync(remark);

        }


        public async Task<List<RegistrationCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown()
        {
            return await _caseTypeRepository.GetAll()
                .Select(caseType => new RegistrationCaseTypeLookupTableDto
                {
                    Id = caseType.Id,
                    DisplayName = caseType == null || caseType.ShortName == null ? "" : caseType.ShortName.ToString()
                }).ToListAsync();
        }



        public async Task<RegistrationCreationTimeMinMax> GetMainRegistrationMinMaxCreationTime()
        {
            var minCreationTime = await _mainRegistrationRepository.GetAll().MinAsync(mr => mr.CreationTime);
            var maxCreationTime = DateTime.Now;

            var result = new RegistrationCreationTimeMinMax
            {
                MinDateTime = minCreationTime,
                MaxDateTime = maxCreationTime
            };

            return result;
        }

        public async Task<string> GetMainRegistrationFileRefNo(int registerId)
        {
            var fileRefNo = await _mainRegistrationRepository.GetAll().Where(s => s.Id == registerId).Select(mr => mr.FileRefNo).FirstOrDefaultAsync();

            return fileRefNo;
        }

        private IQueryable<MainRegistrationDashboardDto> BuildDashboardDto(IQueryable<MainRegistration> query)
        {
            return query.Join(_insuranceCompanyRepository.GetAll().AsNoTracking(), mr => mr.CompanyId, cr => cr.Id, (mr, cr) => new { mr, cr })
                        .Join(_userRepository.GetAll().AsNoTracking(), joined => joined.mr.AdjusterMemberId, user => user.Id, (joined, user) => new { joined.mr, joined.cr, user })
                        .Where(x => !x.cr.IsDeleted)
                        .Select(x => new MainRegistrationDashboardDto()
                        {
                            CaseTypeId = x.mr.CaseTypeId,
                            CaseTypeShortName = x.mr.CaseType.ShortName,
                            BranchId = x.mr.BranchId,
                            BranchShortName = x.mr.Branch.ShortName,
                            CompanyId = x.mr.CompanyId,
                            CompanyShortName = x.mr.Company.ShortName,
                            VehicleNo = x.mr.VehicleNo,
                            ModeOfAssignment = x.mr.ModeOfAssignment,
                            AdjusterUserName = x.user.Name,
                            EditorMemberId = x.mr.EditorMemberId,
                            StatusId = x.mr.StatusId,
                            StatusCode = x.mr.Status.Code ?? "",
                            AssignTime = x.mr.AssignTime,
                            Id = x.mr.Id,
                            Prefix = x.mr.Prefix,
                            CaseNo = x.mr.CaseNo,
                        });
        }

    }
}
