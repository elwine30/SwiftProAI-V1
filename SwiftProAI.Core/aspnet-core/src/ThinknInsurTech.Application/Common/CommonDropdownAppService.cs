using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization.Roles;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Branches;
using ThinknInsurTech.Case;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Companies;
using ThinknInsurTech.LawFirms;
using ThinknInsurTech.Organizations;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Runtime;
using ThinknInsurTech.Vehicles;

namespace ThinknInsurTech.Common
{
    [AbpAuthorize]
    public class CommonDropdownAppService : ThinknInsurTechAppServiceBase, ICommonDropdownAppService
    {
        private readonly IRepository<Branch, int> _lookup_branchRepository;
        private readonly IRepository<InsuranceCompany, int> _lookup_insuranceCompanyRepository;
        private readonly IRepository<Staff, int> _lookup_staffRepository;
        private readonly IRepository<Location, int> _lookup_locationRepository;
        private readonly IRepository<Hospital, int> _lookup_hospitalRepository;
        private readonly IRepository<Group, int> _lookup_groupRepository;
        private readonly IRepository<LawFirm, int> _lookup_lawFirmRepository;
        private readonly IRepository<CaseType, int> _lookup_caseTypeRepository;
        private readonly IRepository<CaseThirdPartyVehicle, int> _caseThirdPartyVehicle;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly IRepository<Lookup, int> _lookup_lookupRepository;
        private readonly IRepository<ViewThirdPartyCases, int> _viewThirdPartyCasesRepository;
        private readonly IRepository<MainRegistration, int> _mainRegistrationRepository;

        public CommonDropdownAppService(IRepository<Branch, int> lookup_branchRepository,
            IRepository<InsuranceCompany, int> lookup_insuranceCompanyRepository,
            IRepository<Staff, int> lookup_staffRepository,
            IRepository<Location, int> lookup_locationRepository,
            IRepository<Hospital, int> lookup_hospitalRepository,
            IRepository<Group, int> lookup_groupRepository,
            IRepository<LawFirm> lookup_lawFirmRepository,
            IRepository<CaseType> lookup_caseTypeRepository,
            IRepository<CaseThirdPartyVehicle, int> caseThirdPartyVehicle,
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Vehicle> vehicleRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
            IRepository<ViewThirdPartyCases, int> viewThirdPartyCasesRepository,
            IRepository<MainRegistration, int> mainRegistrationRepository,
            IRepository<Lookup, int> lookupRepository,
            IUnitOfWorkManager unitOfWorkManager = null
            )
        {
            _lookup_branchRepository = lookup_branchRepository;
            _lookup_insuranceCompanyRepository = lookup_insuranceCompanyRepository;
            _lookup_staffRepository = lookup_staffRepository;
            _lookup_locationRepository = lookup_locationRepository;
            _lookup_hospitalRepository = lookup_hospitalRepository;
            _lookup_groupRepository = lookup_groupRepository;
            _lookup_lawFirmRepository = lookup_lawFirmRepository;
            _lookup_caseTypeRepository = lookup_caseTypeRepository;
            _caseThirdPartyVehicle = caseThirdPartyVehicle;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _vehicleRepository = vehicleRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
            _lookup_lookupRepository = lookupRepository;
            _viewThirdPartyCasesRepository = viewThirdPartyCasesRepository;
            _mainRegistrationRepository = mainRegistrationRepository;
        }

        public async Task<List<CommonDropdownDto>> GetAllBranchForTableDropdown()
        {
            return await _lookup_branchRepository.GetAll()
                .OrderBy(x => x.Name)
                .Select(branch => new CommonDropdownDto
                {
                    Id = branch.Id,
                    DisplayName = branch == null || branch.Name == null ? "" : branch.Name.ToString()
                }).ToListAsync();
        }

        public async Task<List<CommonDropdownDto>> GetAllCompanyForTableDropdown()
        {
            return await _lookup_insuranceCompanyRepository.GetAll()
                .OrderBy(x => x.ShortName)
                .Select(company => new CommonDropdownDto
                {
                    Id = company.Id,
                    DisplayName = company == null || company.ShortName == null ? "" : company.ShortName.ToString()
                }).ToListAsync();
        }

        public async Task<List<CommonAdjusterDropdownDto>> GetAllAdjusterForTableDropdown()
        {
            /*
                superadmin and admin (no ou) to see all adjusters
                adjuster company only can see their own company and adjusters
                third party will only see the adjusters that assign to them
             */

            // AbpUserRoles table will only store admin and superadmin roles
            // Other roles will be found in AbpOrganizationUnitRoles

            var currentOUId = AbpSession.GetCurrentOUId();
            var query = _userRepository.GetAll().AsNoTracking()
                .Join(_userOrganizationUnitRepository.GetAll().AsNoTracking(), ur => ur.Id, uou => uou.UserId, (ur, uou) => new { ur, uou })
                .Join(_organizationUnitRoleRepository.GetAll().AsNoTracking(), joined => joined.uou.OrganizationUnitId, our => our.OrganizationUnitId, (joined, our) => new { joined.ur, joined.uou, our })
                .Join(_roleRepository.GetAll().AsNoTracking(), joined => joined.our.RoleId, r => r.Id, (joined, r) => new { joined.ur, joined.uou, joined.our, r })
                .Where(x => x.r.Name == StaticRoleNames.Tenants.Adjuster);

            if (currentOUId == null)
            {
                // superadmin and admin (no ou)
                using (CurrentUnitOfWork.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
                {
                    return await query
                        .Select(x => new CommonAdjusterDropdownDto
                        {
                            Id = x.ur.Id,
                            DisplayName = x.ur.Name,
                        })
                        .ToListAsync();
                }
            }
            else
            {
                var user = UserManager.GetUser(AbpSession.ToUserIdentifier());
                var userRoles = await UserManager.GetRolesAsync(user);

                if (userRoles.Contains(StaticRoleNames.Tenants.Adjuster))
                {
                    return await query
                        .Where(x => x.uou.OrganizationUnitId == currentOUId)
                        .Select(x => new CommonAdjusterDropdownDto
                        {
                            Id = x.ur.Id,
                            DisplayName = x.ur.Name,
                        })
                        .ToListAsync();
                }
                else
                {
                    //return await query
                    //    .Join(_viewThirdPartyCasesRepository.GetAll().AsNoTracking(), joined => joined.)
                    using (CurrentUnitOfWork.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
                    {
                        return await _mainRegistrationRepository.GetAll().AsNoTracking()
                        .Join(_viewThirdPartyCasesRepository.GetAll().AsNoTracking(), mr => mr.Id, vtpc => vtpc.RegisterId, (mr, vtpc) => new { mr, vtpc })
                        .Join(_userOrganizationUnitRepository.GetAll().AsNoTracking(), joined => joined.mr.AdjusterMemberId, uou => uou.UserId, (joined, uou) => new { joined.vtpc, uou })
                        .Join(_userRepository.GetAll().AsNoTracking(), joined => joined.uou.UserId, u => u.Id, (joined, u) => new { joined.uou, joined.vtpc, u })
                        .Join(_organizationUnitRoleRepository.GetAll().AsNoTracking(), joined => joined.uou.OrganizationUnitId, our => our.OrganizationUnitId, (joined, our) => new { joined.uou, joined.u, joined.vtpc, our })
                        .Join(_roleRepository.GetAll().AsNoTracking(), joined => joined.our.RoleId, r => r.Id, (joined, r) => new { joined.u, joined.vtpc, joined.our, r })
                        .Where(x => x.vtpc.AssignedOUId == currentOUId)
                        .Where(x => x.r.Name == StaticRoleNames.Tenants.Adjuster)
                        .Select(x => new CommonAdjusterDropdownDto
                        {
                            Id = x.u.Id,
                            DisplayName = x.u.Name
                        })
                        .ToListAsync();
                    }
                }

            }
        }

        public async Task<List<CommonAdjusterDropdownDto>> GetAllAdjusterByBranchForTableDropdown(int branchId)
        {
            IQueryable<CommonAdjusterDropdownDto> userRoleQuery = _userRepository.GetAll().AsNoTracking()
                .Join(_lookup_staffRepository.GetAll().AsNoTracking(), user => user.Id, staff => staff.UserId, (user, staff) => new { user, staff })
                .Join(_userRoleRepository.GetAll().AsNoTracking(), joined => joined.user.Id, userRole => userRole.UserId, (joined, userRole) => new { joined.user, joined.staff, userRole })
                .Join(_roleRepository.GetAll().AsNoTracking(), joined => joined.userRole.RoleId, role => role.Id, (joined, role) => new { joined.user, joined.staff, role })
                .Where(j => j.role.Name == StaticRoleNames.Tenants.Adjuster && j.staff.GroupFk.BranchId == branchId)
                .OrderBy(j => j.user.Name)
                .Select(j => new CommonAdjusterDropdownDto
                {
                    Id = j.user.Id,
                    DisplayName = j.user.Name,
                });

            IQueryable<CommonAdjusterDropdownDto> userOURoleQuery = _userRepository.GetAll().AsNoTracking()
                .Join(_lookup_staffRepository.GetAll().AsNoTracking(), user => user.Id, staff => staff.UserId, (user, staff) => new { user, staff })
                .Join(_userOrganizationUnitRepository.GetAll().AsNoTracking(), joined => joined.user.Id, userOU => userOU.UserId, (joined, userOU) => new { joined.user, joined.staff, userOU })
                .Join(_organizationUnitRoleRepository.GetAll().AsNoTracking(), joined => joined.userOU.OrganizationUnitId, our => our.OrganizationUnitId, (joined, our) => new { joined.user, joined.staff, our })
                .Join(_roleRepository.GetAll(), joined => joined.our.RoleId, role => role.Id, (joined, role) => new { joined.user, joined.staff, role })
                .Where(j => j.role.Name == StaticRoleNames.Tenants.Adjuster && j.staff.GroupFk.BranchId == branchId)
                .OrderBy(j => j.user.Name)
                .Select(j => new CommonAdjusterDropdownDto
                {
                    Id = j.user.Id,
                    DisplayName = j.user.Name,
                });

            var query = userRoleQuery.Union(userOURoleQuery).OrderBy(dto => dto.DisplayName);
            var adjusters = await query.ToListAsync();
            return adjusters;
        }

        public async Task<List<CommonDropdownDto>> GetAllLocationByCountryForTableDropdown(int parentLocationId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {

                var location = await _lookup_locationRepository.GetAll().Where(w => w.ParentLocationId == parentLocationId)
                .OrderBy(x => x.Name)
                .Select(location => new CommonDropdownDto
                {
                    Id = location.Id,
                    DisplayName = location == null || location.Name == null ? "" : location.Name.ToString()
                }).ToListAsync();

                return location;

            }
        }

        public async Task<List<CommonDropdownDto>> GetAllLocationByStateForTableDropdown(int parentLocationId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {

                var location = await _lookup_locationRepository.GetAll().Where(w => w.ParentLocationId == parentLocationId)
                 .OrderBy(x => x.Name)
                .Select(location => new CommonDropdownDto
                {
                    Id = location.Id,
                    DisplayName = location == null || location.Name == null ? "" : location.Name.ToString()
                }).ToListAsync();

                return location;

            }
        }

        public async Task<List<CommonDropdownDto>> GetAllHospitalForTableDropdown()
        {
            return await _lookup_hospitalRepository.GetAll()
                .Select(hospital => new CommonDropdownDto
                {
                    Id = hospital.Id,
                    DisplayName = hospital == null || hospital.Name == null ? "" : hospital.Name.ToString(),
                }).ToListAsync();
        }

        public async Task<CommonHospitalDropdownDto> GetHospitalByIdForTableDropdown(int hospitalId)
        {
            var hospital = await _lookup_hospitalRepository.FirstOrDefaultAsync(hospitalId);

            if (hospital == null)
            {
                return new CommonHospitalDropdownDto() { };
            }

            return new CommonHospitalDropdownDto()
            {
                Id = hospital.Id,
                DisplayName = hospital.Name,
                Address = hospital.Address,
            };
        }

        //TODO GROUP , LAWFIRM , CASE TYPE, 
        public async Task<List<CommonDropdownDto>> GetAllGroupForTableDropdown()
        {
            return await _lookup_groupRepository.GetAll()
                .OrderBy(x => x.Name)
                .Select(group => new CommonDropdownDto
                {
                    Id = group.Id,
                    DisplayName = group == null || group.Name == null ? "" : group.Name.ToString(),
                }).ToListAsync();
        }

        public async Task<List<CommonDropdownDto>> GetAllLawFirmForTableDropdown()
        {
            return await _lookup_lawFirmRepository.GetAll()
                .OrderBy(x => x.Name)
                .Select(lawFirm => new CommonDropdownDto
                {
                    Id = lawFirm.Id,
                    DisplayName = lawFirm == null || lawFirm.Name == null ? "" : lawFirm.Name.ToString()
                }).ToListAsync();
        }
        public async Task<List<CommonDropdownDto>> GetAllCaseTypeForTableDropdown()
        {
            return await _lookup_caseTypeRepository.GetAll()
                .OrderBy(x => x.ShortName)
                .Select(caseType => new CommonDropdownDto
                {
                    Id = caseType.Id,
                    DisplayName = caseType == null || caseType.ShortName == null ? "" : caseType.ShortName.ToString()
                }).ToListAsync();
        }

        public async Task<List<CommonDropdownDto>> GetThirdPartyVehicleDetails(int registerId)
        {
            var vehicles = await _caseThirdPartyVehicle.GetAll()
                .Where(v => v.RegisterId == registerId)
                .Select(vehicle => new CommonDropdownDto
                {
                    Id = vehicle.Id,
                    DisplayName = vehicle.VehicleNo
                }).ToListAsync();

            vehicles.Add(new CommonDropdownDto
            {
                Id = 999,
                DisplayName = "NA"
            });

            return vehicles;
        }

        public async Task<List<CommonDropdownDto>> GetAllMakerVehicle()
        {
            var maker = await _vehicleRepository.GetAll()
                        .Select(makerVehicle => new CommonDropdownDto
                        {
                            Id = makerVehicle.Id,
                            DisplayName = makerVehicle.Make
                        }).GroupBy(x => x.DisplayName)
                        .Select(x => x.First())
                        .ToListAsync();

            return maker;

        }

        public async Task<List<CommonDropdownDto>> GetAllModelByMakerVehicle(string maker)
        {
            var model = await _vehicleRepository.GetAll()
                        .Where(x => x.Make == maker)
                        .Select(modelVehicle => new CommonDropdownDto
                        {
                            Id = modelVehicle.Id,
                            DisplayName = modelVehicle.Model
                        }).GroupBy(x => x.DisplayName)
                        .Select(x => x.First())
                        .ToListAsync();

            return model;

        }

        public async Task<List<CommonDropdownDto>> GetAllSpecsByModelAndMakerVehicle(string maker, string model)
        {
            var specs = await _vehicleRepository.GetAll()
                        .Where(x => x.Make == maker && x.Model == model)
                        .Distinct()
                        .Select(modelVehicle => new CommonDropdownDto
                        {
                            Id = modelVehicle.Id,
                            DisplayName = modelVehicle.Specification
                        }).GroupBy(x => x.DisplayName)
                        .Select(x => x.First())
                        .ToListAsync();

            return specs;

        }

        public async Task<List<CommonDropdownDto>> GetAllOrganizationUnitForDropdown()
        {
            return await _organizationUnitRepository.GetAll()
                .OrderBy(x => x.DisplayName)
                .Select(ou => new CommonDropdownDto
                {
                    Id = (int)ou.Id,
                    DisplayName = ou == null || ou.DisplayName == null ? "" : ou.DisplayName.ToString()
                }).ToListAsync();

        }

        public async Task<List<CommonDropdownDto>> GetAllThirdPartyCaseViewRequestStatus()
        {
            return await _lookup_lookupRepository.GetAll()
                .Where(x => x.Group.Equals("RequestStatus"))
                .OrderBy(x => x.Sequence)
                .Select(o => new CommonDropdownDto
                {
                    Id = (o.Id),
                    DisplayName = o == null || o.Description == null ? "" : o.Description.ToString()
                }).ToListAsync();
        }

    }
}
