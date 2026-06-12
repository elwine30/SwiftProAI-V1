using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Registration.Dtos;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ThinknInsurTech.Runtime;



namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseSearchFees)]
    public class CaseSearchFeesAppService : ThinknInsurTechAppServiceBase, ICaseSearchFeesAppService
    {
        private readonly IRepository<CaseSearchFee> _caseSearchFeeRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;

        public CaseSearchFeesAppService(IRepository<CaseSearchFee> caseSearchFeeRepository, IRepository<MainRegistration, int> lookup_mainRegistrationRepository)
        {
            _caseSearchFeeRepository = caseSearchFeeRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;

        }

        public virtual async Task<PagedResultDto<GetCaseSearchFeeForViewDto>> GetAll(GetAllCaseSearchFeesInput input)
        {

            var filteredCaseSearchFees = _caseSearchFeeRepository.GetAll()
                       .Include(e => e.RegisterFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegisterIdFilter), e => e.RegisterFk != null && e.RegisterFk.Id.ToString() == input.RegisterIdFilter);

            var pagedAndFilteredCaseSearchFees = filteredCaseSearchFees
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var caseSearchFees = from o in pagedAndFilteredCaseSearchFees

                                 select new
                                 {
                                     o.RegisterId,
                                     o.Remark,
                                     o.Amount,
                                     o.Id
                                 };

            var totalCount = await filteredCaseSearchFees.CountAsync();

            var dbList = await caseSearchFees.ToListAsync();
            var results = new List<GetCaseSearchFeeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCaseSearchFeeForViewDto()
                {
                    CaseSearchFee = new CaseSearchFeeDto
                    {
                        RegisterId = o.RegisterId,
                        Remark = o.Remark,
                        Amount = o.Amount,
                        Id = o.Id,
                    },
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCaseSearchFeeForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_CaseSearchFees_Edit)]
        public virtual async Task<GetCaseSearchFeeForEditOutput> GetCaseSearchFeeForEdit(EntityDto input)
        {
            var caseSearchFee = _caseSearchFeeRepository.GetAll().Where(w => w.Id.Equals(input.Id)).FirstOrDefault();
            var output = new GetCaseSearchFeeForEditOutput { CaseSearchFee = ObjectMapper.Map<CreateOrEditCaseSearchFeeDto>(caseSearchFee) };

            var _lookupMainRegistration = await _lookup_mainRegistrationRepository.FirstOrDefaultAsync((int)output.CaseSearchFee.RegisterId);
            output.MainRegistrationVehicleNo = _lookupMainRegistration?.VehicleNo?.ToString();
            
            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCaseSearchFeeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CaseSearchFees_Create)]
        protected virtual async Task Create(CreateOrEditCaseSearchFeeDto input)
        {
            var caseSearchFee = ObjectMapper.Map<CaseSearchFee>(input);
            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                caseSearchFee.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                caseSearchFee.TenantId = (int)AbpSession.TenantId;
            }

            await _caseSearchFeeRepository.InsertAsync(caseSearchFee);

        }

        [AbpAuthorize(AppPermissions.Pages_CaseSearchFees_Edit)]
        protected virtual async Task Update(CreateOrEditCaseSearchFeeDto input)
        {
            var caseSearchFee = await _caseSearchFeeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, caseSearchFee);

        }

        [AbpAuthorize(AppPermissions.Pages_CaseSearchFees_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _caseSearchFeeRepository.DeleteAsync(input.Id);
        }

        public async Task<List<CaseSearchFeeDto>> GetCaseSearchFeeAmountByRegisterId(EntityDto input)
        {
            var CaseSearchFee = _caseSearchFeeRepository.GetAll()
                                .Where(x  => x.RegisterId.Equals(input.Id))
                                .Select(s => new CaseSearchFeeDto
                                {
                                    Remark =  s.Remark,
                                    Amount = s.Amount,
                                    RegisterId = s.RegisterId
                                });
            return await CaseSearchFee.ToListAsync();
        }

    }
}