using Abp.Application.Services.Dto;

using Abp.Authorization;
using Abp.Domain.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;

using System.Threading.Tasks;
using ThinknInsurTech.Case.Dto;
using ThinknInsurTech.Migrations;
using Microsoft.EntityFrameworkCore;

using ThinknInsurTech.Authorization;


namespace ThinknInsurTech.Case
{
    [AbpAuthorize]
    public class CaseTypeAppService : ThinknInsurTechAppServiceBase, ICaseTypeAppService
    {
        private readonly ICaseTypeManager _caseTypeManager;
        private readonly IRepository<CaseType, int> _caseTypeRepository;

        public CaseTypeAppService(
            ICaseTypeManager caseTypeManager,
            IRepository<CaseType, int> caseTypeRepository)

        {
            _caseTypeManager = caseTypeManager;
            _caseTypeRepository = caseTypeRepository;
        }

        public virtual async Task<PagedResultDto<GetCaseTypeForViewDto>> GetAll(GetAllCaseTypeInput input)
        {
            var filteredCaseType = _caseTypeRepository.GetAll()
                                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x => false || x.Description.Contains(input.Filter) || x.ShortName.Contains(input.Filter))
                                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), x => x.Description.Contains(input.DescriptionFilter))
                                .WhereIf(!string.IsNullOrWhiteSpace(input.ShortNameFilter), x => x.ShortName.Contains(input.ShortNameFilter))
                                .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, x => (input.IsActiveFilter == 1 && x.IsActive) || (input.IsActiveFilter == 0 && !x.IsActive));

            var pagedAndFilteredCaseType = filteredCaseType
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var caseType = from c in pagedAndFilteredCaseType
                           select new
                           {
                               Id = c.Id,
                               c.Description,
                               c.ShortName,
                               c.IsActive,
                           };

            var totalCount = await filteredCaseType.CountAsync();

            var dbList = await caseType.ToListAsync();
            var results = new List<GetCaseTypeForViewDto>();

            foreach (var caseTypeDto in dbList)
            {
                var res = new GetCaseTypeForViewDto()
                {
                    CaseType = new CaseTypeDto
                    {
                        Description = caseTypeDto.Description,
                        ShortName = caseTypeDto.ShortName,
                        IsActive = caseTypeDto.IsActive,
                        Id = caseTypeDto.Id,
                    }
                };
                results.Add(res);
            }

            return new PagedResultDto<GetCaseTypeForViewDto>(totalCount, results);

        }

        public virtual async Task<GetCaseTypeForViewDto> GetCaseTypeForView(int id)
        {
            var casetype = await _caseTypeRepository.GetAsync(id);

            var output = new GetCaseTypeForViewDto { CaseType = ObjectMapper.Map<CaseTypeDto>(casetype) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CaseType_Edit)]
        public virtual async Task<GetCaseTypeForEditOutput> GetCasetypeForEdit(EntityDto input)
        {
            var casetype = await _caseTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCaseTypeForEditOutput { CaseType = ObjectMapper.Map<CreateOrEditCaseTypeDto>(casetype) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditCaseTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_CaseType_Create)]
        protected virtual async Task Create(CreateOrEditCaseTypeDto input)
        {
            var casetype = ObjectMapper.Map<CaseType>(input);


            await _caseTypeRepository.InsertAsync(casetype);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CaseType_Edit)]
        protected virtual async Task Update(CreateOrEditCaseTypeDto input)
        {
            var casetype = await _caseTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, casetype);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_CaseType_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _caseTypeRepository.DeleteAsync(input.Id);
        }



        public async Task<CaseTypeDto> GetCaseTypeDetailsbyId(int id)
        {
            var caseTypeDetail = await _caseTypeManager.GetCaseTypebyIdAsync(id);
            var _caseTypeDto = new CaseTypeDto
            {
                Id = caseTypeDetail.Id,
                Description = caseTypeDetail.Description,
                ShortName = caseTypeDetail.ShortName,
                IsActive = caseTypeDetail.IsActive
            };

            return _caseTypeDto;
        }

        public async Task<ListResultDto<CaseTypeDto>> GetAllCaseTypeDetails()
        {
            var sourceCaseTypeDetails = await _caseTypeManager.GetAllCaseTypeAsync();

            var caseTypeList = new List<CaseTypeDto>();
            foreach (var item in sourceCaseTypeDetails)
            {
                caseTypeList.Add(new CaseTypeDto
                {
                    Id = item.Id,
                    Description = item.Description,
                    ShortName = item.ShortName,
                    IsActive = item.IsActive
                });
            }

            return new ListResultDto<CaseTypeDto>(caseTypeList);
        }

        public async Task<int> CreateCaseType(CaseTypeDto input)
        {
            var sourceCaseType = new CaseType
            {
                Description = input.Description,
                ShortName = input.ShortName,
                IsActive = input.IsActive
            };

            var id = await _caseTypeManager.CreateCaseTypeAsync(sourceCaseType);

            return id;
        }

    }
}