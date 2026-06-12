using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Common;
using ThinknInsurTech.Organizations;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Runtime;
using ThinknInsurTech.Workshops;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseWorkshops)]
    public class CaseWorkshopsAppService : ThinknInsurTechAppServiceBase, ICaseWorkshopsAppService
    {
        private readonly IRepository<CaseWorkshop> _caseWorkshopRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IRepository<Workshop, int> _lookup_workshopRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<DocumentSetting> _documentSettingRepository;
        private readonly ViewThirdPartyCasesManager _mainRegistrationOrganizationUnitManager;
        private readonly IRepository<ViewThirdPartyCases> _mainRegistrationOrganizationUnitRepository;


        public CaseWorkshopsAppService(IRepository<CaseWorkshop> caseWorkshopRepository, IRepository<ViewThirdPartyCases> mainRegistrationOrganizationUnitRepository, IRepository<MainRegistration, int> lookup_mainRegistrationRepository, IRepository<Workshop, int> lookup_workshopRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<DocumentSetting> documentSettingRepository)
        {
            _caseWorkshopRepository = caseWorkshopRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _lookup_workshopRepository = lookup_workshopRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _documentSettingRepository = documentSettingRepository;
            _mainRegistrationOrganizationUnitRepository = mainRegistrationOrganizationUnitRepository;
        }

        public virtual async Task<PagedResultDto<GetCaseWorkshopForViewDto>> GetAll(GetAllCaseWorkshopsInput input)
        {

            var filteredCaseWorkshops = _caseWorkshopRepository.GetAll()
                        .Include(e => e.RegisterFk)
                        .Include(e => e.WorkshopFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Email.Contains(input.Filter) || e.ContactNo.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNoFilter), e => e.ContactNo.Contains(input.ContactNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNameFilter), e => e.ContactName.Contains(input.ContactNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MainRegistrationVehicleNoFilter), e => e.RegisterFk != null && e.RegisterFk.VehicleNo == input.MainRegistrationVehicleNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WorkshopWorkshopNameFilter), e => e.WorkshopFk != null && e.WorkshopFk.WorkshopName == input.WorkshopWorkshopNameFilter);

            var pagedAndFilteredCaseWorkshops = filteredCaseWorkshops
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var caseWorkshops = from o in pagedAndFilteredCaseWorkshops
                                join o1 in _lookup_mainRegistrationRepository.GetAll() on o.RegisterId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_workshopRepository.GetAll() on o.WorkshopId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new
                                {
                                    o.RegisterId,
                                    o.WorkshopId,
                                    o.Email,
                                    o.ContactNo,
                                    o.ContactName,
                                    o.Id,
                                    MainRegistrationVehicleNo = s1 == null || s1.VehicleNo == null ? "" : s1.VehicleNo.ToString(),
                                    WorkshopWorkshopName = s2 == null || s2.WorkshopName == null ? "" : s2.WorkshopName.ToString()
                                };

            var totalCount = await filteredCaseWorkshops.CountAsync();

            var dbList = await caseWorkshops.ToListAsync();
            var results = new List<GetCaseWorkshopForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCaseWorkshopForViewDto()
                {
                    CaseWorkshop = new CaseWorkshopDto
                    {
                        RegisterId = o.RegisterId,
                        WorkshopId = o.WorkshopId,
                        Email = o.Email,
                        ContactNo = o.ContactNo,
                        ContactName = o.ContactName,
                        Id = o.Id,
                    },
                    MainRegistrationVehicleNo = o.MainRegistrationVehicleNo,
                    WorkshopWorkshopName = o.WorkshopWorkshopName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCaseWorkshopForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetCaseWorkshopForViewDto> GetCaseWorkshopForView(EntityDto input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var currentOUId = AbpSession.GetCurrentOUId().Value;

                var assignedRegisterId = _mainRegistrationOrganizationUnitRepository.GetAll().Where(f => f.AssignedOUId == currentOUId).Where(f => f.RegisterId == input.Id).Select(f => f.RegisterId).FirstOrDefault();

                var caseWorkshop = _caseWorkshopRepository.GetAll().Where(w => w.RegisterId == assignedRegisterId).FirstOrDefault();

                var output = new GetCaseWorkshopForViewDto { CaseWorkshop = ObjectMapper.Map<CaseWorkshopDto>(caseWorkshop) };

                var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync(input.Id);
                output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();
                // Workshop could be null (No workshop entity)
                Workshop _lookupWorkshop = null;
                if (caseWorkshop != null)
                {
                    _lookupWorkshop = await _lookup_workshopRepository.FirstOrDefaultAsync((int)output.CaseWorkshop.WorkshopId);
                    output.WorkshopWorkshopName = _lookupWorkshop?.WorkshopName?.ToString();
                }

                return output;

            }

        }

        [AbpAuthorize(AppPermissions.Pages_CaseWorkshops_Edit)]
        public virtual async Task<GetCaseWorkshopForEditOutput> GetCaseWorkshopForEdit(EntityDto input)
        {
            var caseWorkshop = _caseWorkshopRepository.GetAll().Where(w => w.RegisterId.Equals(input.Id)).FirstOrDefault();

            var output = new GetCaseWorkshopForEditOutput { CaseWorkshop = ObjectMapper.Map<CreateOrEditCaseWorkshopDto>(caseWorkshop) };


            var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync(input.Id);
            output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();


            if (caseWorkshop != null && caseWorkshop.WorkshopId != null)
            {
                Workshop _lookupWorkshop = null;
                if (caseWorkshop.WorkshopId != null)
                {
                    _lookupWorkshop = await _lookup_workshopRepository.FirstOrDefaultAsync((int)output.CaseWorkshop.WorkshopId);
                }
                output.WorkshopWorkshopName = _lookupWorkshop?.WorkshopName?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCaseWorkshopDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CaseWorkshops_Create)]
        protected virtual async Task Create(CreateOrEditCaseWorkshopDto input)
        {
            var caseWorkshop = ObjectMapper.Map<CaseWorkshop>(input);
            if (caseWorkshop.WorkshopId == 0)
            {
                caseWorkshop.WorkshopId = null;
            }

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                caseWorkshop.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                caseWorkshop.TenantId = (int?)AbpSession.TenantId;
            }

            // add MainRegistrationOU record for workshop company
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                // if workshop assignOUId is not null, means the view3rdpartycaserequest is approved 
                // only after workshop company onboard and approve the binding between workshop company and adjuster's company master data, then will add record in ViewThirdPartyCases
                // MainRegistrationOU == ViewThirdPartyCase
                var workshop = await _lookup_workshopRepository.FirstOrDefaultAsync(x => x.Id == input.WorkshopId);

                if (workshop != null && workshop.AssignOUId != null)
                {
                    var entity = new ViewThirdPartyCases()
                    {
                        RegisterId = input.RegisterId,
                        AssignedOUId = workshop.AssignOUId,
                        TenantId = (int?)AbpSession.TenantId,
                        CreatorUserId = (int?)AbpSession.UserId
                    };

                    await _mainRegistrationOrganizationUnitManager.CreateMainRegistrationOU(entity);
                }
            }
            // Wiped out record during merge, if the sequence of events if not right can amend accordingly
            await _caseWorkshopRepository.InsertAsync(caseWorkshop);
        }

        // consider previous null but new not 
        // consider previous and new null
        // consider previous not and new not
        [AbpAuthorize(AppPermissions.Pages_CaseWorkshops_Edit)]
        protected virtual async Task Update(CreateOrEditCaseWorkshopDto input)
        {
            var caseWorkshop = await _caseWorkshopRepository.FirstOrDefaultAsync((int)input.Id);

            // update MainRegistrationOU record for workshop company
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var workshop = await _lookup_workshopRepository.FirstOrDefaultAsync(x => x.Id == input.WorkshopId);

                if (workshop != null && workshop.AssignOUId != null)
                {
                    var previousWorkshop = await _lookup_workshopRepository.FirstOrDefaultAsync(x => x.Id == caseWorkshop.WorkshopId);

                    if (previousWorkshop != null && previousWorkshop.AssignOUId != null)
                    {
                        var mrou = await _mainRegistrationOrganizationUnitManager.GetMainRegistrationOUByAssignedOUIdAsync((long)previousWorkshop.AssignOUId, input.RegisterId);

                        await _mainRegistrationOrganizationUnitManager.UpdateMainRegistrationOU((long)previousWorkshop.AssignOUId, input.RegisterId, (long)workshop.AssignOUId);
                    }
                }
            }

            ObjectMapper.Map(input, caseWorkshop);

            await _caseWorkshopRepository.UpdateAsync(caseWorkshop);
        }

        [AbpAuthorize(AppPermissions.Pages_CaseWorkshops_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _caseWorkshopRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_CaseWorkshops)]
        public async Task<PagedResultDto<CaseWorkshopMainRegistrationLookupTableDto>> GetAllMainRegistrationForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mainRegistrationRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.VehicleNo != null && e.VehicleNo.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mainRegistrationList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CaseWorkshopMainRegistrationLookupTableDto>();
            foreach (var mainRegistration in mainRegistrationList)
            {
                lookupTableDtoList.Add(new CaseWorkshopMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration.VehicleNo?.ToString()
                });
            }

            return new PagedResultDto<CaseWorkshopMainRegistrationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_CaseWorkshops)]
        public async Task<List<CaseWorkshopWorkshopLookupTableDto>> GetAllWorkshopForTableDropdown()
        {
            return await _lookup_workshopRepository.GetAll()
                .Select(workshop => new CaseWorkshopWorkshopLookupTableDto
                {
                    Id = workshop.Id,
                    DisplayName = workshop == null || workshop.WorkshopName == null ? "" : workshop.WorkshopName.ToString()
                }).ToListAsync();
        }
    }
}