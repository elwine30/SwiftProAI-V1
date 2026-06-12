using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Approval.Dtos;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Common;
using ThinknInsurTech.LawFirms;
using ThinknInsurTech.Organizations;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Workshops;
using ThinknInsurTech.Companies;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ThinknInsurTech.Approval
{
    //[AbpAuthorize(AppPermissions.Pages_ViewThirdPartyCaseRequests)]
    public class ViewThirdPartyCaseRequestsAppService : ThinknInsurTechAppServiceBase, IViewThirdPartyCaseRequestsAppService
    {
        private readonly IRepository<ViewThirdPartyCaseRequest> _viewThirdPartyCaseRequestRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<DocumentSetting> _documentSettingsRepository;
        private readonly IRepository<CaseLawyer> _caseLawyerRepository;
        private readonly IRepository<ViewThirdPartyCases> _viewThirdPartyCasesRepository;
        private readonly IRepository<CaseWorkshop> _caseWorkshopRepository;
        private readonly IRepository<CaseAdjuster> _caseAdjusterRepository;
        private readonly IRepository<CaseInsurer> _caseInsurerRepository;
        private readonly IRepository<InsuranceCompany, int> _lookupInsuranceCompanyRepository;
        private readonly IRepository<LawFirm, int> _lookupLawFirmRepository;
        private readonly IRepository<Workshop, int> _lookupWorkshopRepository;
        private readonly IRepository<DocumentSetting, int> _lookupDocumentSettingsRepository;
        private readonly IRepository<MainRegistration, int> _mainRegistrationRepository;

        public ViewThirdPartyCaseRequestsAppService(IRepository<ViewThirdPartyCaseRequest> viewThirdPartyCaseRequestRepository,
                IRepository<User, long> lookup_userRepository, IRepository<DocumentSetting> documentSettingsRepository,
                IRepository<CaseLawyer> caseLawyerRepository, IRepository<ViewThirdPartyCases> viewThirdPartyCasesRepository,
                IRepository<CaseWorkshop> caseWorkshopRepository, IRepository<CaseAdjuster> caseAdjusterRepository,
                IRepository<CaseInsurer> caseInsurerRepository, IRepository<InsuranceCompany, int> lookupInsuranceCompanyRepository,
                IRepository<LawFirm, int> lookupLawFirmRepository, IRepository<Workshop, int> lookupWorkshopRepository,
                IRepository<DocumentSetting, int> _lookupDocumentSettingsRepository, IRepository<MainRegistration, int> mainRegistrationRepository)
        {
            _viewThirdPartyCaseRequestRepository = viewThirdPartyCaseRequestRepository;
            _lookup_userRepository = lookup_userRepository;
            _documentSettingsRepository = documentSettingsRepository;
            _caseLawyerRepository = caseLawyerRepository;
            _viewThirdPartyCasesRepository = viewThirdPartyCasesRepository;
            _caseWorkshopRepository = caseWorkshopRepository;
            _caseAdjusterRepository = caseAdjusterRepository;
            _caseInsurerRepository = caseInsurerRepository;
            _lookupInsuranceCompanyRepository = lookupInsuranceCompanyRepository;
            _lookupLawFirmRepository = lookupLawFirmRepository;
            _lookupWorkshopRepository = lookupWorkshopRepository;
            _lookupInsuranceCompanyRepository = lookupInsuranceCompanyRepository;
            _mainRegistrationRepository = mainRegistrationRepository;
        }

        public virtual async Task<PagedResultDto<GetViewThirdPartyCaseRequestForViewDto>> GetAllNotOnboarded(GetAllViewThirdPartyCaseRequestsInput input)
        {
            var documentSettings = _documentSettingsRepository.GetAll()
                    .Select(ds => ds.businessRegistrationNo)
                    .Distinct().ToHashSet();

            var filteredViewThirdPartyCaseRequests = _viewThirdPartyCaseRequestRepository.GetAll()
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Status.Contains(input.Filter))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Status), e => e.Status.Contains(input.Status))
                    .WhereIf(input.CreationDate.HasValue, e => e.CreationTime.Date.Equals(input.CreationDate.Value))
                    .Where(e => e.AssignByOU.HasValue)
                    .Where(e => !documentSettings.Contains(e.BusinessRegistrationNo));

            var pagedAndFilteredViewThirdPartyCaseRequests = filteredViewThirdPartyCaseRequests
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var viewThirdPartyCaseRequests = from o in filteredViewThirdPartyCaseRequests

                                             join user in _lookup_userRepository.GetAll()
                                             on o.CreatorUserId equals user.Id into userGroup
                                             from user in userGroup.DefaultIfEmpty()

                                             join ds in _documentSettingsRepository.GetAll()
                                             on o.AssignByOU equals ds.OrganizationUnitId into settingsGroup
                                             from ds in settingsGroup.DefaultIfEmpty()

                                             select new
                                             {
                                                 o.Id,
                                                 o.BusinessRegistrationNo,
                                                 RequestedBy = user == null || user.Name == null ? "" : user.Name,
                                                 CreationDate = o.CreationTime,
                                                 CompanyType = ds == null || ds.companyType == null ? "" : ds.companyType,
                                                 o.CompanyName,
                                                 AdjusterCompanyName = ds == null || ds.companyLegalName == null ? "" : ds.companyLegalName,
                                             };

            var totalCount = await filteredViewThirdPartyCaseRequests.CountAsync();

            var dbList = await viewThirdPartyCaseRequests.ToListAsync();
            var results = new List<GetViewThirdPartyCaseRequestForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetViewThirdPartyCaseRequestForViewDto()
                {
                    ViewThirdPartyCaseRequest = new ViewThirdPartyCaseRequestDto
                    {
                        Id = o.Id,
                        BusinessRegistrationNo = o.BusinessRegistrationNo,
                        RequestedBy = o.RequestedBy,
                        CreationDate = o.CreationDate,
                        CompanyType = o.CompanyType,
                        Name = o.CompanyName,
                        AdjusterCompanyName = o.AdjusterCompanyName,
                    }
                };
                results.Add(res);
            }

            return new PagedResultDto<GetViewThirdPartyCaseRequestForViewDto>(
                totalCount,
                results
            );
        }

        public virtual async Task<PagedResultDto<GetViewThirdPartyCaseRequestForViewDto>> GetAllOnboarded(GetAllViewThirdPartyCaseRequestsInput input)
        {
            var documentSettings = _documentSettingsRepository.GetAll()
                    .Select(ds => ds.businessRegistrationNo)
                    .Distinct().ToHashSet();

            var filteredViewThirdPartyCaseRequests = _viewThirdPartyCaseRequestRepository.GetAll()
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Status.Contains(input.Filter))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Status), e => e.Status.Contains(input.Status))
                    .WhereIf(input.CreationDate.HasValue, e => e.CreationTime.Date.Equals(input.CreationDate.Value))
                    .Where(e => e.AssignByOU.HasValue)
                    .Where(e => documentSettings.Contains(e.BusinessRegistrationNo));

            var pagedAndFilteredViewThirdPartyCaseRequests = filteredViewThirdPartyCaseRequests
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var viewThirdPartyCaseRequests = from o in filteredViewThirdPartyCaseRequests

                                             join user in _lookup_userRepository.GetAll()
                                             on o.CreatorUserId equals user.Id into userGroup
                                             from user in userGroup.DefaultIfEmpty()

                                             join ds in _documentSettingsRepository.GetAll()
                                             on o.AssignByOU equals ds.OrganizationUnitId into settingsGroup
                                             from ds in settingsGroup.DefaultIfEmpty()

                                             select new
                                             {
                                                 o.Id,
                                                 o.BusinessRegistrationNo,
                                                 RequestedBy = user == null || user.Name == null ? "" : user.Name,
                                                 CreationDate = o.CreationTime,
                                                 CompanyType = ds == null || ds.companyType == null ? "" : ds.companyType,
                                                 o.CompanyName,
                                                 AdjusterCompanyName = ds == null || ds.companyLegalName == null ? "" : ds.companyLegalName,
                                                 ApprovedDate = o.ApprovedDate == null ? "" : o.ApprovedDate.ToString(),
                                                 CancelledDate = o.CancelledDate == null ? "" : o.CancelledDate.ToString()
                                             };

            var totalCount = await filteredViewThirdPartyCaseRequests.CountAsync();

            var dbList = await viewThirdPartyCaseRequests.ToListAsync();
            var results = new List<GetViewThirdPartyCaseRequestForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetViewThirdPartyCaseRequestForViewDto()
                {
                    ViewThirdPartyCaseRequest = new ViewThirdPartyCaseRequestDto
                    {
                        Id = o.Id,
                        BusinessRegistrationNo = o.BusinessRegistrationNo,
                        RequestedBy = o.RequestedBy,
                        CreationDate = o.CreationDate,
                        CompanyType = o.CompanyType,
                        Name = o.CompanyName,
                        AdjusterCompanyName = o.AdjusterCompanyName,
                        ApprovedDate = o.ApprovedDate,
                        CancelledDate = o.CancelledDate
                    }
                };
                results.Add(res);
            }

            return new PagedResultDto<GetViewThirdPartyCaseRequestForViewDto>(
                totalCount,
                results
            );
        }

        public virtual async Task<GetViewThirdPartyCaseRequestForViewDto> GetViewThirdPartyCaseRequestForView(int id)
        {
            var viewThirdPartyCaseRequest = await _viewThirdPartyCaseRequestRepository.GetAsync(id);

            var output = new GetViewThirdPartyCaseRequestForViewDto { ViewThirdPartyCaseRequest = ObjectMapper.Map<ViewThirdPartyCaseRequestDto>(viewThirdPartyCaseRequest) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ViewThirdPartyCaseRequests_Edit)]
        public virtual async Task<GetViewThirdPartyCaseRequestForEditOutput> GetViewThirdPartyCaseRequestForEdit(EntityDto input)
        {
            var viewThirdPartyCaseRequest = await _viewThirdPartyCaseRequestRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetViewThirdPartyCaseRequestForEditOutput { ViewThirdPartyCaseRequest = ObjectMapper.Map<CreateOrEditViewThirdPartyCaseRequestDto>(viewThirdPartyCaseRequest) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditViewThirdPartyCaseRequestDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ViewThirdPartyCaseRequests_Create)]
        protected virtual async Task Create(CreateOrEditViewThirdPartyCaseRequestDto input)
        {
            var viewThirdPartyCaseRequest = ObjectMapper.Map<ViewThirdPartyCaseRequest>(input);

            if (AbpSession.TenantId != null)
            {
                viewThirdPartyCaseRequest.TenantId = (int)AbpSession.TenantId;
            }

            await _viewThirdPartyCaseRequestRepository.InsertAsync(viewThirdPartyCaseRequest);

        }

        //[AbpAuthorize(AppPermissions.Pages_ViewThirdPartyCaseRequests_Edit)]
        protected virtual async Task Update(CreateOrEditViewThirdPartyCaseRequestDto input)
        {
            var viewThirdPartyCaseRequest = await _viewThirdPartyCaseRequestRepository.FirstOrDefaultAsync((int)input.Id);
            //ObjectMapper.Map(input, viewThirdPartyCaseRequest);

            var _documentSettings = await _documentSettingsRepository.GetAll().Where(x => x.businessRegistrationNo.Contains(input.BusinessRegistrationNo)).FirstOrDefaultAsync();

            viewThirdPartyCaseRequest.Status = input.Status;
            viewThirdPartyCaseRequest.ApprovedDate = input.ApprovedDate;
            viewThirdPartyCaseRequest.ApprovedBy = (int?)AbpSession.UserId;
            viewThirdPartyCaseRequest.AssignToOU = _documentSettings.OrganizationUnitId;

            //update ViewThirdPartyCaseRequest table
            await _viewThirdPartyCaseRequestRepository.UpdateAsync(viewThirdPartyCaseRequest);

            if (_documentSettings.companyType.ToLower() == "insurance")
            {
                //update master data table
                var masterInsurance = _lookupInsuranceCompanyRepository.GetAll().Where(x => x.ViewThirdPartyCaseRequestId.Equals(viewThirdPartyCaseRequest.Id)).FirstOrDefault();
                if(masterInsurance != null)
                {
                    masterInsurance.AssignOUId = _documentSettings.OrganizationUnitId;

                    ObjectMapper.Map<InsuranceCompany>(masterInsurance);

                    //insert into ViewThirdPartyCase if available assigned cases.
                    var insurers = await _mainRegistrationRepository.GetAll().AsNoTracking().Where(x => x.CompanyId.Equals(masterInsurance.Id)).ToListAsync();

                    if (insurers.Count > 0)
                    {
                        foreach (var insurance in insurers)
                        {
                            ViewThirdPartyCases item = new ViewThirdPartyCases();
                            item.TenantId = insurance.TenantId;
                            item.RegisterId = insurance.Id;
                            item.AssignedOUId = viewThirdPartyCaseRequest.AssignToOU;

                            await _viewThirdPartyCasesRepository.InsertAsync(item);
                        }
                    }
                }
                
            }
            else if (_documentSettings.companyType.ToLower() == "law firm")
            {
                //update master data table
                var masterLawFirm = _lookupLawFirmRepository.GetAll().Where(x => x.ViewThirdPartyCaseRequestId.Equals(viewThirdPartyCaseRequest.Id)).FirstOrDefault();
                if(masterLawFirm != null)
                {
                    masterLawFirm.AssignOUId = _documentSettings.OrganizationUnitId;

                    ObjectMapper.Map<LawFirm>(masterLawFirm);

                    //insert into ViewThirdPartyCase if available assigned cases.
                    var lawFirmsAssigned = await _caseLawyerRepository.GetAll().Where(x => x.LawFirmId.Equals(masterLawFirm.Id)).ToListAsync();
                    if (lawFirmsAssigned.Count > 0)
                    {
                        foreach (var lawFirm in lawFirmsAssigned)
                        {
                            ViewThirdPartyCases item = new ViewThirdPartyCases();
                            item.TenantId = lawFirm.TenantId;
                            item.RegisterId = lawFirm.RegisterId;
                            item.AssignedOUId = viewThirdPartyCaseRequest.AssignToOU;

                            await _viewThirdPartyCasesRepository.InsertAsync(item);
                        }
                    }
                }
            }
            else if (_documentSettings.companyType.ToLower() == "workshop")
            {
                //update master data table
                var masterWorkshop = _lookupWorkshopRepository.GetAll().Where(x => x.ViewThirdPartyCaseRequestId.Equals(viewThirdPartyCaseRequest.Id)).FirstOrDefault();
                if (masterWorkshop != null)
                {
                    masterWorkshop.AssignOUId = _documentSettings.OrganizationUnitId;

                    ObjectMapper.Map<Workshop>(masterWorkshop);

                    //insert into ViewThirdPartyCase if available assigned cases.
                    var workshopAssigned = await _caseWorkshopRepository.GetAll().Where(x => x.WorkshopId.Equals(masterWorkshop.Id)).ToListAsync();
                    if (workshopAssigned.Count > 0)
                    {
                        foreach (var workshop in workshopAssigned)
                        {
                            ViewThirdPartyCases item = new ViewThirdPartyCases();
                            item.TenantId = workshop.TenantId;
                            item.RegisterId = workshop.RegisterId;
                            item.AssignedOUId = viewThirdPartyCaseRequest.AssignToOU;

                            await _viewThirdPartyCasesRepository.InsertAsync(item);
                        }
                    }
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ViewThirdPartyCaseRequests_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _viewThirdPartyCaseRequestRepository.DeleteAsync(input.Id);
        }

    }
}