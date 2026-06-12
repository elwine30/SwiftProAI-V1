using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.OCR.Dtos;
using ThinknInsurTech.Dto;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ThinknInsurTech.Storage;
using ThinknInsurTech.Common.Dto;
using DocumentFormat.OpenXml.Spreadsheet;
using Abp.Domain.Uow;

namespace ThinknInsurTech.OCR
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Prompts)]
    public class PromptsAppService : ThinknInsurTechAppServiceBase, IPromptsAppService
    {
        private readonly IRepository<Prompt> _promptRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PromptsAppService(IRepository<Prompt> promptRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _promptRepository = promptRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public virtual async Task<PagedResultDto<GetPromptForViewDto>> GetAll(GetAllPromptsInput input)
        {
            //Get data without the tenant filtering
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var results = new List<GetPromptForViewDto>();
                var totalCount = 0;
                var filteredPrompts = _promptRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PromptName.Contains(input.Filter) || e.PromptRequest.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromptNameFilter), e => e.PromptName.Contains(input.PromptNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromptRequestFilter), e => e.PromptRequest.Contains(input.PromptRequestFilter));

                var pagedAndFilteredPrompts = filteredPrompts
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var prompts = from o in pagedAndFilteredPrompts
                              select new
                              {
                                  o.PromptName,
                                  o.PromptRequest,
                                  Id = o.Id
                              };
                var dbList = await prompts.ToListAsync();
                totalCount = await filteredPrompts.CountAsync();

                foreach (var o in dbList)
                {
                    var res = new GetPromptForViewDto()
                    {
                        Prompt = new PromptDto
                        {
                            PromptName = o.PromptName,
                            PromptRequest = o.PromptRequest,
                            Id = o.Id,
                        }
                    };
                    results.Add(res);
                }

                return new PagedResultDto<GetPromptForViewDto>(
                    totalCount,
                    results
                );
            }  
        }

        public PromptDto GetPromptByPromptName(string promptName)
        {
            var prompt = _promptRepository.GetAll().Where(x => x.PromptName == promptName).FirstOrDefault();
            var returnPrompt = new PromptDto
            {
                PromptName = prompt.PromptName,
                PromptRequest = prompt.PromptRequest,
            };
            return returnPrompt;
        }

        public virtual async Task<GetPromptForViewDto> GetPromptForView(int id)
        {
            var prompt = await _promptRepository.GetAsync(id);

            var output = new GetPromptForViewDto { Prompt = ObjectMapper.Map<PromptDto>(prompt) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Prompts_Edit)]
        public virtual async Task<GetPromptForEditOutput> GetPromptForEdit(EntityDto input)
        {
            var prompt = await _promptRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPromptForEditOutput { Prompt = ObjectMapper.Map<CreateOrEditPromptDto>(prompt) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditPromptDto input)
        {
            if (input.Id == null)
            {
                //await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        //[AbpAuthorize(AppPermissions.Pages_Administration_Prompts_Create)]
        //protected virtual async Task Create(CreateOrEditPromptDto input)
        //{
        //    var prompt = ObjectMapper.Map<Prompt>(input);

        //    if (AbpSession.TenantId != null)
        //    {
        //        prompt.TenantId = (int?)AbpSession.TenantId;
        //    }

        //    await _promptRepository.InsertAsync(prompt);

        //}

        [AbpAuthorize(AppPermissions.Pages_Administration_Prompts_Edit)]
        protected virtual async Task Update(CreateOrEditPromptDto input)
        {
            var prompt = await _promptRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, prompt);

        }


    }
}