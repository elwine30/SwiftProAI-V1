using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Common;
using ThinknInsurTech.LawFirms;
using ThinknInsurTech.Organizations;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseLawyers)]
    public class CaseLawyersAppService : ThinknInsurTechAppServiceBase, ICaseLawyersAppService
    {
        private readonly IRepository<CaseLawyer> _caseLawyerRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IRepository<LawFirm, int> _lookup_lawFirmRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<DocumentSetting> _documentSettingRepository;
        private readonly ViewThirdPartyCasesManager _mainRegistrationOrganizationUnitManager;
        private readonly IRepository<ViewThirdPartyCases> _mainRegistrationOrganizationUnitRepository;

        public CaseLawyersAppService(IRepository<CaseLawyer> caseLawyerRepository, IRepository<MainRegistration, int> lookup_mainRegistrationRepository, IRepository<LawFirm, int> lookup_lawFirmRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<DocumentSetting> documentSettingRepository, ViewThirdPartyCasesManager mainRegistrationOrganizationUnitManager, IRepository<ViewThirdPartyCases> mainRegistrationOrganizationUnitRepository)
        {
            _caseLawyerRepository = caseLawyerRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _lookup_lawFirmRepository = lookup_lawFirmRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _documentSettingRepository = documentSettingRepository;
            _mainRegistrationOrganizationUnitManager = mainRegistrationOrganizationUnitManager;
            _mainRegistrationOrganizationUnitRepository = mainRegistrationOrganizationUnitRepository;
        }

        public virtual async Task<PagedResultDto<GetCaseLawyerForViewDto>> GetAll(GetAllCaseLawyersInput input)
        {
            var filteredCaseLawyers = _caseLawyerRepository.GetAll()
                .Include(e => e.RegisterFk)
                .Include(e => e.LawFirmFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReferenceNo.Contains(input.Filter) || e.ContactNo.Contains(input.Filter) || e.ContactName.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Type.Contains(input.Filter))
                .WhereIf(input.MinHearingDateFilter != null, e => e.HearingDate >= input.MinHearingDateFilter)
                .WhereIf(input.MaxHearingDateFilter != null, e => e.HearingDate <= input.MaxHearingDateFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceNoFilter), e => e.ReferenceNo.Contains(input.ReferenceNoFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNoFilter), e => e.ContactNo.Contains(input.ContactNoFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNameFilter), e => e.ContactName.Contains(input.ContactNameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.RegisterIdFilter), e => e.RegisterFk != null && e.RegisterFk.Id.ToString() == input.RegisterIdFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LawFirmNameFilter), e => e.LawFirmFk != null && e.LawFirmFk.Name == input.LawFirmNameFilter);

            var pagedAndFilteredCaseLawyers = filteredCaseLawyers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var caseLawyers = from o in pagedAndFilteredCaseLawyers
                              join o1 in _lookup_mainRegistrationRepository.GetAll() on o.RegisterId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _lookup_lawFirmRepository.GetAll() on o.LawFirmId equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              select new
                              {
                                  o.LawFirmId,
                                  o.RegisterId,
                                  o.HearingDate,
                                  o.ReferenceNo,
                                  o.ContactNo,
                                  o.ContactName,
                                  o.Email,
                                  o.Type,
                                  o.Id,
                                  MainRegistrationVehicleNo = s1 == null || s1.VehicleNo == null ? "" : s1.VehicleNo.ToString(),
                                  LawFirmName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                              };

            var totalCount = await filteredCaseLawyers.CountAsync();

            var dbList = await caseLawyers.ToListAsync();
            var results = new List<GetCaseLawyerForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCaseLawyerForViewDto()
                {
                    CaseLawyer = new CaseLawyerDto
                    {
                        RegisterId = o.RegisterId,
                        LawFirmId = o.LawFirmId,
                        HearingDate = o.HearingDate,
                        ReferenceNo = o.ReferenceNo,
                        ContactNo = o.ContactNo,
                        ContactName = o.ContactName,
                        Email = o.Email,
                        Type = o.Type,
                        Id = o.Id,
                    },
                    MainRegistrationVehicleNo = o.MainRegistrationVehicleNo,
                    LawFirmName = o.LawFirmName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCaseLawyerForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<PagedResultDto<GetCaseLawyerForViewDto>> GetAllForView(GetAllCaseLawyersInput input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var currentOUId = AbpSession.GetCurrentOUId().Value;
                int registerId = Convert.ToInt32(input.RegisterIdFilter);

                var assignedRegisterId = _mainRegistrationOrganizationUnitRepository.GetAll().Where(f => f.AssignedOUId == currentOUId).Where(f => f.RegisterId == registerId).Select(f => f.RegisterId).FirstOrDefault();


                var filteredCaseLawyers = _caseLawyerRepository.GetAll()
                .Include(e => e.RegisterFk)
                .Include(e => e.LawFirmFk)
                .Where(f => f.RegisterId == assignedRegisterId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReferenceNo.Contains(input.Filter) || e.ContactNo.Contains(input.Filter) || e.ContactName.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Type.Contains(input.Filter))
                .WhereIf(input.MinHearingDateFilter != null, e => e.HearingDate >= input.MinHearingDateFilter)
                .WhereIf(input.MaxHearingDateFilter != null, e => e.HearingDate <= input.MaxHearingDateFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceNoFilter), e => e.ReferenceNo.Contains(input.ReferenceNoFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNoFilter), e => e.ContactNo.Contains(input.ContactNoFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNameFilter), e => e.ContactName.Contains(input.ContactNameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LawFirmNameFilter), e => e.LawFirmFk != null && e.LawFirmFk.Name == input.LawFirmNameFilter);

                var pagedAndFilteredCaseLawyers = filteredCaseLawyers
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var caseLawyers = from o in pagedAndFilteredCaseLawyers
                                  join o1 in _lookup_mainRegistrationRepository.GetAll() on o.RegisterId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_lawFirmRepository.GetAll() on o.LawFirmId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  select new
                                  {
                                      o.LawFirmId,
                                      o.RegisterId,
                                      o.HearingDate,
                                      o.ReferenceNo,
                                      o.ContactNo,
                                      o.ContactName,
                                      o.Email,
                                      o.Type,
                                      o.Id,
                                      MainRegistrationVehicleNo = s1 == null || s1.VehicleNo == null ? "" : s1.VehicleNo.ToString(),
                                      LawFirmName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                  };

                var totalCount = await filteredCaseLawyers.CountAsync();

                var dbList = await caseLawyers.ToListAsync();
                var results = new List<GetCaseLawyerForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetCaseLawyerForViewDto()
                    {
                        CaseLawyer = new CaseLawyerDto
                        {
                            RegisterId = o.RegisterId,
                            LawFirmId = o.LawFirmId,
                            HearingDate = o.HearingDate,
                            ReferenceNo = o.ReferenceNo,
                            ContactNo = o.ContactNo,
                            ContactName = o.ContactName,
                            Email = o.Email,
                            Type = o.Type,
                            Id = o.Id,
                        },
                        MainRegistrationVehicleNo = o.MainRegistrationVehicleNo,
                        LawFirmName = o.LawFirmName
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetCaseLawyerForViewDto>(
                    totalCount,
                    results
                );

            }
        }

        //public virtual async Task<GetCaseLawyerForViewDto> GetCaseLawyerForView(int id)
        //{
        //    var caseLawyer = await _caseLawyerRepository.GetAsync(id);

        //    var output = new GetCaseLawyerForViewDto { CaseLawyer = ObjectMapper.Map<CaseLawyerDto>(caseLawyer) };


        //    var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.CaseLawyer.RegisterId);
        //    output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();

        //    var _lookupLawFirm = await _lookup_lawFirmRepository.FirstOrDefaultAsync((int)output.CaseLawyer.LawFirmId);
        //    output.LawFirmName = _lookupLawFirm?.Name?.ToString();


        //    return output;
        //}

        [AbpAuthorize(AppPermissions.Pages_CaseLawyers_Edit)]
        public virtual async Task<GetCaseLawyerForEditOutput> GetCaseLawyerForEdit(EntityDto input)
        {
            var caseLawyer = _caseLawyerRepository.GetAll().Where(w => w.Id.Equals(input.Id)).FirstOrDefault();
            var output = new GetCaseLawyerForEditOutput { CaseLawyer = ObjectMapper.Map<CreateOrEditCaseLawyerDto>(caseLawyer) };

            if (caseLawyer != null)
            {
                var _lookupLawFirm = await _lookup_lawFirmRepository.FirstOrDefaultAsync((int)output.CaseLawyer.LawFirmId);
                output.LawFirmName = _lookupLawFirm?.Name?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCaseLawyerDto input)
        {

            if (input.HearingDate == null || !input.HearingDate.HasValue || input.HearingDate.Value.Year < 999)
            {
                input.HearingDate = System.DateTime.Now;
            }
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CaseLawyers_Create)]
        protected virtual async Task Create(CreateOrEditCaseLawyerDto input)
        {
            var caseLawyer = ObjectMapper.Map<CaseLawyer>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                caseLawyer.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                caseLawyer.TenantId = (int?)AbpSession.TenantId;
            }

            // add MainRegistrationOU record for lawfirm company
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                // if lawfirm assignOUId is not null, means the view3rdpartycaserequest is approved 
                // only after lawfirm company onboard and approve the binding between lawfirm company and adjuster's company master data, then will add record in ViewThirdPartyCases
                // MainRegistrationOU == ViewThirdPartyCase
                var lawFirm = await _lookup_lawFirmRepository.FirstOrDefaultAsync(x => x.Id == input.LawFirmId);

                if(lawFirm.AssignOUId != null)
                {
                    var entity = new ViewThirdPartyCases()
                    {
                        RegisterId = input.RegisterId,
                        AssignedOUId = lawFirm.AssignOUId,
                        TenantId = (int?)AbpSession.TenantId,
                        CreatorUserId = (int?)AbpSession.UserId
                    };

                    await _mainRegistrationOrganizationUnitManager.CreateMainRegistrationOU(entity);
                }
            }

            await _caseLawyerRepository.InsertAsync(caseLawyer);
        }

        [AbpAuthorize(AppPermissions.Pages_CaseLawyers_Edit)]
        protected virtual async Task Update(CreateOrEditCaseLawyerDto input)
        {
            var caseLawyer = await _caseLawyerRepository.FirstOrDefaultAsync((int)input.Id);

            // update MainRegistrationOU record for lawyer company
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var lawFirm = await _lookup_lawFirmRepository.FirstOrDefaultAsync(input.LawFirmId);

                if (lawFirm.AssignOUId != null)
                {
                    var previousLawFirm = await _lookup_lawFirmRepository.FirstOrDefaultAsync(caseLawyer.LawFirmId);

                    if (previousLawFirm.AssignOUId != null)
                    {
                        var mrou = await _mainRegistrationOrganizationUnitManager.GetMainRegistrationOUByAssignedOUIdAsync((long)previousLawFirm.AssignOUId, input.RegisterId);

                        await _mainRegistrationOrganizationUnitManager.UpdateMainRegistrationOU((long)previousLawFirm.AssignOUId, input.RegisterId, (long)lawFirm.AssignOUId);
                    }
                }
            }

            ObjectMapper.Map(input, caseLawyer);

            await _caseLawyerRepository.UpdateAsync(caseLawyer);
        }

        [AbpAuthorize(AppPermissions.Pages_CaseLawyers_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var caseLawFirm = await _caseLawyerRepository.FirstOrDefaultAsync(input.Id);

                var lawFirm = await _lookup_lawFirmRepository.FirstOrDefaultAsync(x => x.Id == caseLawFirm.LawFirmId);

                if(lawFirm.AssignOUId != null)
                {
                    await _mainRegistrationOrganizationUnitManager.DeleteMainRegistrationOU(lawFirm.AssignOUId, caseLawFirm.RegisterId);
                }
            }

            await _caseLawyerRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_CaseLawyers)]
        public async Task<PagedResultDto<CaseLawyerMainRegistrationLookupTableDto>> GetAllMainRegistrationForLookupTable(Registration.Dtos.GetAllForLookupTableInput input)
        {
            var query = _lookup_mainRegistrationRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.VehicleNo != null && e.VehicleNo.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mainRegistrationList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CaseLawyerMainRegistrationLookupTableDto>();
            foreach (var mainRegistration in mainRegistrationList)
            {
                lookupTableDtoList.Add(new CaseLawyerMainRegistrationLookupTableDto
                {
                    Id = mainRegistration.Id,
                    DisplayName = mainRegistration.VehicleNo?.ToString()
                });
            }

            return new PagedResultDto<CaseLawyerMainRegistrationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_CaseLawyers)]
        public async Task<List<CaseLawyerLawFirmLookupTableDto>> GetAllLawFirmForTableDropdown()
        {
            return await _lookup_lawFirmRepository.GetAll()
                .Select(lawFirm => new CaseLawyerLawFirmLookupTableDto
                {
                    Id = lawFirm.Id,
                    DisplayName = lawFirm == null || lawFirm.Name == null ? "" : lawFirm.Name.ToString()
                }).ToListAsync();
        }
    }
}