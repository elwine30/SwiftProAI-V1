using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Case;
using ThinknInsurTech.Companies.Dtos;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using ThinknInsurTech.Runtime;
using ThinknInsurTech.Approval;
using ThinknInsurTech.Approval.Dtos;
using Abp.Domain.Uow;
using System;
using ThinknInsurTech.Organizations;


namespace ThinknInsurTech.Companies
{
    [AbpAuthorize]
    public class CompanyAppService : ThinknInsurTechAppServiceBase, ICompanyAppService
    {
        private readonly ICompanyManager _companyManager;
        private readonly IRepository<InsuranceCompany, int> _insuranceCompanyRepository;
        private readonly IRepository<CaseType, int> _lookup_caseTypeRepository;
        private readonly IRepository<ViewThirdPartyCaseRequest, int> _viewThirdPartyCaseRequestRepository;
        private readonly IRepository<ViewThirdPartyCases> _viewThirdPartyCasesRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CompanyAppService(
            ICompanyManager companyManager,
            IRepository<InsuranceCompany, int> insuranceCompanyRepository,
            IRepository<CaseType, int> lookup_caseTypeRepository,
            IRepository<ViewThirdPartyCaseRequest> viewThirdPartyCaseRequestRepository,
            IRepository<ViewThirdPartyCases> viewThirdPartyCasesRepository,
            IUnitOfWorkManager unitOfWorkManager)

        {
            _companyManager = companyManager;
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _lookup_caseTypeRepository = lookup_caseTypeRepository;
            _viewThirdPartyCaseRequestRepository = viewThirdPartyCaseRequestRepository;
            _viewThirdPartyCasesRepository = viewThirdPartyCasesRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }


        public async Task<CompanyDto> GetCompanyDetailsbyId(int id)
        {
        var companyDetail = await _companyManager.GetCompanybyIdAsync(id);
            var _companyDto = new CompanyDto
            {
                Name = companyDetail.Name,
                ShortName = companyDetail.ShortName,
                CaseTypeId = companyDetail.CaseTypeId,
                ClaimRate = companyDetail.ClaimRate,
                Address = companyDetail.Address,
                GstRegNo = companyDetail.GstRegNo,
                IsActive = companyDetail.IsActive,
                PhotographCharge = companyDetail.PhotographCharge,
                Id = companyDetail.Id,
                BusinessRegistrationNo = companyDetail.BusinessRegistrationNo,
                AssignOUId = companyDetail.AssignOUId,
                ViewThirdPartyCaseRequest = new CreateOrEditViewThirdPartyCaseRequestDto()
                {
                    Status = companyDetail.ViewThirdPartyCaseRequestFk.Status,
                    CancelRemark = companyDetail.ViewThirdPartyCaseRequestFk.CancelRemark,
                },
                ViewThirdPartyCaseRequestId = companyDetail.ViewThirdPartyCaseRequestId,
            };

            return _companyDto;
        }

        public async Task<ListResultDto<CompanyDto>> GetAllCompanyDetails()
        {
            var sourceCompanyDetails = await _companyManager.GetAllCompanyAsync();

            var companyList = new List<CompanyDto>();
            foreach (var item in sourceCompanyDetails)
            {
                companyList.Add(new CompanyDto
                {
                    Name = item.Name,
                    ShortName = item.ShortName,
                    CaseTypeId = item.CaseTypeId,
                    ClaimRate = item.ClaimRate,
                    Address = item.Address,
                    GstRegNo = item.GstRegNo,
                    IsActive = item.IsActive,
                    PhotographCharge = item.PhotographCharge,
                    Id = item.Id,
                    BusinessRegistrationNo = item.BusinessRegistrationNo,
                    AssignOUId= item.AssignOUId,
                    ViewThirdPartyCaseRequest = new CreateOrEditViewThirdPartyCaseRequestDto()
                    {
                        Status = item.ViewThirdPartyCaseRequestFk.Status,
                        CancelRemark = item.ViewThirdPartyCaseRequestFk.CancelRemark,
                    },
                    ViewThirdPartyCaseRequestId= item.ViewThirdPartyCaseRequestId,
                });
            }

            return new ListResultDto<CompanyDto>(companyList);
        }

        public virtual async Task<PagedResultDto<GetCompanyForViewDto>> GetAll(GetAllCompaniesInput input)
        {
            var filteredCompanies = _insuranceCompanyRepository.GetAll()
                        .Include(e => e.CaseTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.ShortName.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.GstRegNo.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShortNameFilter), e => e.ShortName.Contains(input.ShortNameFilter))
                        .WhereIf(input.MinClaimRateFilter != null, e => e.ClaimRate >= input.MinClaimRateFilter)
                        .WhereIf(input.MaxClaimRateFilter != null, e => e.ClaimRate <= input.MaxClaimRateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GstRegNoFilter), e => e.GstRegNo.Contains(input.GstRegNoFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(input.MinPhotographChargeFilter != null, e => e.PhotographCharge >= input.MinPhotographChargeFilter)
                        .WhereIf(input.MaxPhotographChargeFilter != null, e => e.PhotographCharge <= input.MaxPhotographChargeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CaseTypeDescriptionFilter), e => e.CaseTypeFk != null && e.CaseTypeFk.Description == input.CaseTypeDescriptionFilter);

            var pagedAndFilteredCompanies = filteredCompanies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var companies = from o in pagedAndFilteredCompanies
                            join o1 in _lookup_caseTypeRepository.GetAll() on o.CaseTypeId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            select new
                            {
                                o.Name,
                                o.ShortName,
                                o.ClaimRate,
                                o.Address,
                                o.GstRegNo,
                                o.IsActive,
                                o.PhotographCharge,
                                Id = o.Id,
                                CaseTypeDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                                o.BusinessRegistrationNo,
                                o.AssignOUId,
                                o.AllowToViewAssignedCases,
                                o.ViewThirdPartyCaseRequestFk,
                                o.ViewThirdPartyCaseRequestId
                            };

            var totalCount = await filteredCompanies.CountAsync();

            var dbList = await companies.ToListAsync();
            var results = new List<GetCompanyForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCompanyForViewDto()
                {
                    Company = new CompanyDto
                    {

                        Name = o.Name,
                        ShortName = o.ShortName,
                        ClaimRate = o.ClaimRate,
                        Address = o.Address,
                        GstRegNo = o.GstRegNo,
                        IsActive = o.IsActive,
                        PhotographCharge = o.PhotographCharge,
                        Id = o.Id,
                        BusinessRegistrationNo = o.BusinessRegistrationNo,
                        AssignOUId = o.AssignOUId,
                        AllowToViewAssignedCases = o.AllowToViewAssignedCases,
                        ViewThirdPartyCaseRequestId = o.ViewThirdPartyCaseRequestId,
                        ViewThirdPartyCaseRequest = new CreateOrEditViewThirdPartyCaseRequestDto()
                        {
                            Status = o.ViewThirdPartyCaseRequestFk != null ? o.ViewThirdPartyCaseRequestFk.Status : null,
                            CancelRemark = o.ViewThirdPartyCaseRequestFk != null ? o.ViewThirdPartyCaseRequestFk.CancelRemark : null,
                        }
                    },
                    CaseTypeDescription = o.CaseTypeDescription
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCompanyForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetCompanyForViewDto> GetCompanyForView(int id)
        {
            var company = await _insuranceCompanyRepository.GetAsync(id);

            var output = new GetCompanyForViewDto { Company = ObjectMapper.Map<CompanyDto>(company) };

            if (output.Company.CaseTypeId != null)
            {
                var _lookupCaseType = await _lookup_caseTypeRepository.FirstOrDefaultAsync((int)output.Company.CaseTypeId);
                output.CaseTypeDescription = _lookupCaseType?.Description?.ToString();
            }

            if (output.Company.ViewThirdPartyCaseRequestId != null)
            {
                var _viewThirdPartyCaseRequest = await _viewThirdPartyCaseRequestRepository.FirstOrDefaultAsync((int)output.Company.ViewThirdPartyCaseRequestId);
                output.Company.ViewThirdPartyCaseRequest = ObjectMapper.Map<CreateOrEditViewThirdPartyCaseRequestDto>(_viewThirdPartyCaseRequest);
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Companies_Edit)]
        public virtual async Task<GetCompanyForEditOutput> GetCompanyForEdit(EntityDto input)
        {
            var company = await _insuranceCompanyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCompanyForEditOutput { Company = ObjectMapper.Map<CreateOrEditCompanyDto>(company) };

            if (output.Company.CaseTypeId != null)
            {
                var _lookupCaseType = await _lookup_caseTypeRepository.FirstOrDefaultAsync((int)output.Company.CaseTypeId);
                output.CaseTypeDescription = _lookupCaseType?.Description?.ToString();
            }

            if (output.Company.ViewThirdPartyCaseRequestId != null)
            {
                var _viewThirdPartyCaseRequest = await _viewThirdPartyCaseRequestRepository.FirstOrDefaultAsync((int)output.Company.ViewThirdPartyCaseRequestId);
                output.Company.ViewThirdPartyCaseRequest = ObjectMapper.Map<CreateOrEditViewThirdPartyCaseRequestDto>(_viewThirdPartyCaseRequest);
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCompanyDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Companies_Create)]
        protected virtual async Task Create(CreateOrEditCompanyDto input)
        {
            var company = ObjectMapper.Map<InsuranceCompany>(input);

            if(AbpSession.GetCurrentOUId() != null)
            {
                company.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;
            }

            if (input.AllowToViewAssignedCases)
            {
                var viewThirdPartyCaseRequest = new CreateOrEditViewThirdPartyCaseRequestDto()
                {
                    AssignByOU = AbpSession.GetCurrentOUId() ?? null,
                    BusinessRegistrationNo = input.BusinessRegistrationNo,
                    CompanyName = input.Name,
                };

                company.ViewThirdPartyCaseRequestFk = ObjectMapper.Map<ViewThirdPartyCaseRequest>(viewThirdPartyCaseRequest);

                company.ViewThirdPartyCaseRequestFk.TenantId = (int)AbpSession.TenantId;
                company.ViewThirdPartyCaseRequestFk.Status = "Pending Approval";
            }

            if (AbpSession.TenantId != null)
            {
                company.TenantId = (int)AbpSession.TenantId;
            }

            await _insuranceCompanyRepository.InsertAsync(company);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Companies_Edit)]
        protected virtual async Task Update(CreateOrEditCompanyDto input)
        {
            var company = await _insuranceCompanyRepository.FirstOrDefaultAsync((int)input.Id);
            

            if (input.ViewThirdPartyCaseRequestId != null)
            {
                await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
                {
                    var entity = await _viewThirdPartyCaseRequestRepository.GetAll()
                    .Where(x => x.Id == (int)input.ViewThirdPartyCaseRequestId)
                    .FirstOrDefaultAsync();

                    if (entity != null)
                    {
                        if (!input.AllowToViewAssignedCases)
                        {
                            if (entity.Status == "Approved")
                            {
                                var allowedViewCase = _viewThirdPartyCasesRepository.GetAll().Where(w => w.MainRegistrationFk.OrganizationUnitId == AbpSession.GetCurrentOUId() && w.AssignedOUId == company.AssignOUId);

                                foreach (var item in allowedViewCase)
                                {
                                    _viewThirdPartyCasesRepository.Delete(item);
                                }

                                input.AssignOUId = null;
                            }
                            entity.CancelRemark = input.ViewThirdPartyCaseRequest.CancelRemark;
                            entity.CancelledBy = (int)AbpSession.UserId.Value;
                            entity.CancelledDate = DateTime.Now;
                            entity.Status = "Cancelled";
                            input.ViewThirdPartyCaseRequestId = null;
                        }

                        // allow adjuster to update if request is pendingApproval
                        if (entity.Status == "Pending Approval")
                        {
                            entity.BusinessRegistrationNo = input.BusinessRegistrationNo;
                            entity.CompanyName = input.Name;
                        }

                        await _viewThirdPartyCaseRequestRepository.UpdateAsync(entity);
                    }
                });
            }
            else
            {
                if (input.AllowToViewAssignedCases)
                {
                    var viewThirdPartyCaseRequest = new CreateOrEditViewThirdPartyCaseRequestDto()
                    {
                        AssignByOU = AbpSession.GetCurrentOUId() ?? null,
                        BusinessRegistrationNo = input.BusinessRegistrationNo,
                        CompanyName = input.Name
                    };

                    company.ViewThirdPartyCaseRequestFk = ObjectMapper.Map<ViewThirdPartyCaseRequest>(viewThirdPartyCaseRequest);

                    company.ViewThirdPartyCaseRequestFk.TenantId = (int)AbpSession.TenantId;
                    company.ViewThirdPartyCaseRequestFk.Status = "Pending Approval";
                }
            }

            ObjectMapper.Map(input, company);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Companies_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _insuranceCompanyRepository.DeleteAsync(input.Id);
        }


        [AbpAuthorize(AppPermissions.Pages_Administration_Companies)]
        public async Task<List<CompanyCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown()
        {
            return await _lookup_caseTypeRepository.GetAll()
                .Select(caseType => new CompanyCaseTypeLookupTableDto
                {
                    Id = caseType.Id,
                    DisplayName = caseType == null || caseType.Description == null ? "" : caseType.Description.ToString()
                }).ToListAsync();
        }


    }

}