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
using ThinknInsurTech.Companies;
using ThinknInsurTech.Organizations;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseInsurers)]
    public class CaseInsurersAppService : ThinknInsurTechAppServiceBase, ICaseInsurersAppService
    {
        private readonly IRepository<CaseInsurer> _caseInsurerRepository;
        private readonly IRepository<InsuranceCompany, int> _lookup_insuranceCompanyRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ViewThirdPartyCases> _mainRegistrationOrganizationUnitRepository;


        public CaseInsurersAppService(IRepository<CaseInsurer> caseInsurerRepository,
            IRepository<InsuranceCompany, int> lookup_insuranceCompanyRepository,
            IRepository<MainRegistration, int> lookup_mainRegistrationRepository,
                                    IRepository<ViewThirdPartyCases> mainRegistrationOrganizationUnitRepository,

            IUnitOfWorkManager unitOfWorkManager)
        {
            _caseInsurerRepository = caseInsurerRepository;
            _lookup_insuranceCompanyRepository = lookup_insuranceCompanyRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _mainRegistrationOrganizationUnitRepository = mainRegistrationOrganizationUnitRepository;

        }

        public virtual async Task<PagedResultDto<GetCaseInsurerForViewDto>> GetAll(GetAllCaseInsurersInput input)
        {

            var filteredCaseInsurers = _caseInsurerRepository.GetAll()
                        .Include(e => e.CompanyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReferenceNo.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Contact.Contains(input.Filter) || e.Email.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceNoFilter), e => e.ReferenceNo.Contains(input.ReferenceNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFilter), e => e.Contact.Contains(input.ContactFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(input.MinRegisterIdFilter != null, e => e.RegisterId >= input.MinRegisterIdFilter)
                        .WhereIf(input.MaxRegisterIdFilter != null, e => e.RegisterId <= input.MaxRegisterIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyNameFilter), e => e.CompanyFk != null && e.CompanyFk.Name == input.CompanyNameFilter);

            var pagedAndFilteredCaseInsurers = filteredCaseInsurers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var caseInsurers = from o in pagedAndFilteredCaseInsurers
                               join o1 in _lookup_insuranceCompanyRepository.GetAll() on o.CompanyId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               select new
                               {

                                   o.ReferenceNo,
                                   o.Name,
                                   o.Contact,
                                   o.Email,
                                   o.RegisterId,
                                   Id = o.Id,
                                   CompanyName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                               };

            var totalCount = await filteredCaseInsurers.CountAsync();

            var dbList = await caseInsurers.ToListAsync();
            var results = new List<GetCaseInsurerForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCaseInsurerForViewDto()
                {
                    CaseInsurer = new CaseInsurerDto
                    {

                        ReferenceNo = o.ReferenceNo,
                        Name = o.Name,
                        Contact = o.Contact,
                        Email = o.Email,
                        RegisterId = o.RegisterId,
                        Id = o.Id,
                    },
                    CompanyName = o.CompanyName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCaseInsurerForViewDto>(
                totalCount,
                results
            );

        }

        //public virtual async Task<GetCaseInsurerForViewDto> GetCaseInsurerForView(int id)
        //{
        //    var caseInsurer = await _caseInsurerRepository.GetAsync(id);

        //    var output = new GetCaseInsurerForViewDto { CaseInsurer = ObjectMapper.Map<CaseInsurerDto>(caseInsurer) };

        //    if (output.CaseInsurer.CompanyId != null)
        //    {
        //        var _lookupCompany = await _lookup_companyRepository.FirstOrDefaultAsync((int)output.CaseInsurer.CompanyId);
        //        output.CompanyName = _lookupCompany?.Name?.ToString();
        //    }

        //    return output;
        //}

        [AbpAuthorize(AppPermissions.Pages_CaseInsurers_Edit)]
        public virtual async Task<GetCaseInsurerForEditOutput> GetCaseInsurerForEdit(int Id)
        {
            var caseInsurer = _caseInsurerRepository.GetAll().Where(w => w.RegisterId.Equals(Id)).FirstOrDefault();

            var output = new GetCaseInsurerForEditOutput { CaseInsurer = ObjectMapper.Map<CreateOrEditCaseInsurerDto>(caseInsurer) };

            var _lookupMainRegistration = _lookup_mainRegistrationRepository.Get(Id);

            if (output.CaseInsurer == null)
            {
                output.CaseInsurer = new CreateOrEditCaseInsurerDto();
                output.CaseInsurer.CompanyId = _lookupMainRegistration.CompanyId;
            }
            else
            {
                output.CaseInsurer.CompanyId = _lookupMainRegistration.CompanyId;
            }

            //var _lookupCompany = await _lookup_companyRepository.FirstOrDefaultAsync((int)_lookupMainRegistration.CompanyId);
            //output.CompanyName = _lookupCompany?.Name?.ToString();

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCaseInsurerDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CaseInsurers_Create)]
        protected virtual async Task Create(CreateOrEditCaseInsurerDto input)
        {
            var caseInsurer = ObjectMapper.Map<CaseInsurer>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                caseInsurer.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }
            if (AbpSession.TenantId != null)
            {
                caseInsurer.TenantId = AbpSession.TenantId.Value;
            }

            await _caseInsurerRepository.InsertAsync(caseInsurer);

        }

        [AbpAuthorize(AppPermissions.Pages_CaseInsurers_Edit)]
        protected virtual async Task Update(CreateOrEditCaseInsurerDto input)
        {
            var caseInsurer = await _caseInsurerRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, caseInsurer);

        }

        //[AbpAuthorize(AppPermissions.Pages_CaseInsurers_Delete)]
        //public virtual async Task Delete(EntityDto input)
        //{
        //    await _caseInsurerRepository.DeleteAsync(input.Id);
        //}


        public virtual async Task<GetCaseInsurerForViewDto> GetCaseInsurerForView(EntityDto input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var currentOUId = AbpSession.GetCurrentOUId().Value;
                int registerId = Convert.ToInt32(input.Id);

                var assignedRegisterId = _mainRegistrationOrganizationUnitRepository.GetAll().Where(f => f.AssignedOUId == currentOUId).Where(f => f.RegisterId == registerId).Select(f => f.RegisterId).FirstOrDefault();


                var caseInsurer = _caseInsurerRepository.GetAll().Where(w => w.RegisterId == assignedRegisterId).FirstOrDefault();

                var output = new GetCaseInsurerForViewDto { CaseInsurer = ObjectMapper.Map<CaseInsurerDto>(caseInsurer) };

                var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync(input.Id);

                if (output.CaseInsurer == null)
                {
                    output.CaseInsurer = new CaseInsurerDto();
                }

                output.CaseInsurer.CompanyId = _lookupMainRegistration.CompanyId;
                var _lookupCompany = await _lookup_insuranceCompanyRepository.FirstOrDefaultAsync((int)_lookupMainRegistration.CompanyId);
                output.CompanyName = _lookupCompany?.Name?.ToString();

                return output;

            }

        }

    }
}