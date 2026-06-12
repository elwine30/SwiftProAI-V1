using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Organizations.Dtos;
using ThinknInsurTech.Dto;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ThinknInsurTech.Storage;
using ThinknInsurTech.Companies;
using ThinknInsurTech.LawFirms;
using ThinknInsurTech.Workshops;
using Z.EntityFramework.Plus;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Runtime;
using ThinknInsurTech.Common;
using Abp.Domain.Uow;
using ThinknInsurTech.Authorization.Users;

namespace ThinknInsurTech.Organizations
{
    public class ViewThirdPartyCasesAppService : ThinknInsurTechAppServiceBase, IViewThirdPartyCasesAppService
    {
        private readonly IRepository<ViewThirdPartyCases> _mainRegistrationOrganizationUnitRepository;

        private readonly IRepository<InsuranceCompany, int> _insuranceCompanyRepository;

        private readonly IRepository<LawFirm, int> _lawFirmRepository;

        private readonly IRepository<Workshop, int> _workshopRepository;

        private readonly IRepository<CaseInsurer, int> _caseInsurerRepository;

        private readonly IRepository<CaseLawyer, int> _caseLawyerRepository;

        private readonly IRepository<CaseWorkshop, int> _caseWorkshopRepository;

        private readonly IRepository<DocumentSetting, int> _documentSettingRepository;

        private readonly IRepository<MainRegistration, int> _mainRegistrationRepository;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ViewThirdPartyCasesAppService(
            IRepository<ViewThirdPartyCases> mainRegistrationOrganizationUnitRepository, 
            IRepository<InsuranceCompany, int> insuranceCompanyRepository, 
            IRepository<LawFirm, int> lawFirmRepository,
            IRepository<Workshop, int> workshopRepository,
            IRepository<CaseInsurer, int> caseInsurerRepository,
            IRepository<CaseLawyer, int> caseLawyerRepository,
            IRepository<CaseWorkshop, int> caseWorkshopRepository,
            IRepository<DocumentSetting, int> documentSettingRepository,
            IRepository<MainRegistration, int> mainRegistrationRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _mainRegistrationOrganizationUnitRepository = mainRegistrationOrganizationUnitRepository;
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _lawFirmRepository = lawFirmRepository;
            _workshopRepository = workshopRepository;
            _caseInsurerRepository = caseInsurerRepository;
            _caseLawyerRepository = caseLawyerRepository;
            _caseWorkshopRepository = caseWorkshopRepository;
            _documentSettingRepository = documentSettingRepository;
            _mainRegistrationRepository = mainRegistrationRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public virtual async Task<PagedResultDto<GetViewThirdPartyCasesForViewDto>> GetAll(GetAllViewThirdPartyCasesInput input)
        {

            var filteredMainRegistrationOrganizationUnits = _mainRegistrationOrganizationUnitRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false);

            var pagedAndFilteredMainRegistrationOrganizationUnits = filteredMainRegistrationOrganizationUnits
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var mainRegistrationOrganizationUnits = from o in pagedAndFilteredMainRegistrationOrganizationUnits
                                                    select new
                                                    {

                                                        Id = o.Id
                                                    };

            var totalCount = await filteredMainRegistrationOrganizationUnits.CountAsync();

            var dbList = await mainRegistrationOrganizationUnits.ToListAsync();
            var results = new List<GetViewThirdPartyCasesForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetViewThirdPartyCasesForViewDto()
                {
                    ViewThirdPartyCases = new ViewThirdPartyCasesDto
                    {

                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetViewThirdPartyCasesForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetViewThirdPartyCasesForEditOutput> GetViewThirdPartyCasesForEdit(EntityDto input)
        {
            var mainRegistrationOrganizationUnit = await _mainRegistrationOrganizationUnitRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetViewThirdPartyCasesForEditOutput { MainRegistrationOrganizationUnit = ObjectMapper.Map<CreateOrEditViewThirdPartyCaseDto>(mainRegistrationOrganizationUnit) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditViewThirdPartyCaseDto input)
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

        protected virtual async Task Create(CreateOrEditViewThirdPartyCaseDto input)
        {
            var mainRegistrationOrganizationUnit = ObjectMapper.Map<ViewThirdPartyCases>(input);

            if (AbpSession.TenantId != null)
            {
                mainRegistrationOrganizationUnit.TenantId = (int?)AbpSession.TenantId;
            }

            await _mainRegistrationOrganizationUnitRepository.InsertAsync(mainRegistrationOrganizationUnit);

        }

        protected virtual async Task Update(CreateOrEditViewThirdPartyCaseDto input)
        {
            var mainRegistrationOrganizationUnit = await _mainRegistrationOrganizationUnitRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, mainRegistrationOrganizationUnit);

        }

        public virtual async Task Delete(EntityDto input)
        {
            await _mainRegistrationOrganizationUnitRepository.DeleteAsync(input.Id);
        }

        public void SyncAssignOUIdMasterData(string businessRegistrationNo, long ouId, string caseType)
        {
            // pending integration and test
            var companyOUId = _documentSettingRepository.GetAll().AsNoTracking()
                .Where(x => x.businessRegistrationNo == businessRegistrationNo)
                .Select(x => x.OrganizationUnitId)
                .FirstOrDefault();

            // reference: case type implementation and values based on this commit: https://gitlab.abagofdreams.com/thinkn/swiftproai/SwiftProAI.Web/-/merge_requests/202/diffs?commit_id=14bc6f5af594ca7ce7c287cea3dd727ecd6655f9
            switch (caseType)
            {
                case "Insurance":
                    _insuranceCompanyRepository.GetAll()
                        .Where(x => !x.IsDeleted && x.BusinessRegistrationNo.Equals(businessRegistrationNo))
                        .Update(x => new InsuranceCompany { AssignOUId = companyOUId });
                    break;
                case "Law Firm":
                    _lawFirmRepository.GetAll()
                        .Where(x => !x.IsDeleted && x.BusinessRegistrationNo.Equals(businessRegistrationNo))
                        .Update(x => new LawFirm { AssignOUId = companyOUId });
                    break;
                case "Workshop":
                    _workshopRepository.GetAll()
                        .Where(x => !x.IsDeleted && x.BusinessRegistrationNo.Equals(businessRegistrationNo))
                        .Update(x => new Workshop { AssignOUId = companyOUId });
                    break;
            }
        }


        public async void SyncThirdPartyCases(string businessRegistrationNo)
        {
            // pending integration and test
            await _unitOfWorkManager.WithUnitOfWorkAsync(() =>
            {
                using (_unitOfWorkManager.Current.SetTenantId(AbpSession.TenantId))
                {
                    var mainRegistrationQuery = _mainRegistrationRepository.GetAll();
                    var mainRegistrationOUQuery = _mainRegistrationOrganizationUnitRepository.GetAll();

                    var updateQuery = from mrou in mainRegistrationOUQuery
                                      join mr in mainRegistrationQuery on mrou.RegisterId equals mr.Id
                                      join entity in (
                                        from c in _insuranceCompanyRepository.GetAll()
                                        where c.BusinessRegistrationNo == businessRegistrationNo
                                        select new { c.OrganizationUnitId, c.AssignOUId }
                                      ).Concat(
                                          from l in _lawFirmRepository.GetAll()
                                          where l.BusinessRegistrationNo == businessRegistrationNo
                                          select new { l.OrganizationUnitId, l.AssignOUId }
                                        ).Concat(
                                          from w in _workshopRepository.GetAll()
                                          where w.BusinessRegistrationNo == businessRegistrationNo
                                          select new { w.OrganizationUnitId, w.AssignOUId }
                                          )
                                        on mr.OrganizationUnitId equals entity.OrganizationUnitId
                                      select new { mrou, assignedOUId = entity.AssignOUId };

                    var updateList = updateQuery.ToList();

                    foreach (var item in updateList)
                    {
                        item.mrou.AssignedOUId = item.assignedOUId;
                        _mainRegistrationOrganizationUnitRepository.Update(item.mrou);
                    }

                    return Task.CompletedTask;
                }
            });
        }
    }
}