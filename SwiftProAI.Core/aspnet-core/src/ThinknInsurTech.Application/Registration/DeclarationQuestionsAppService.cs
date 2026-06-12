using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_Administration_DeclarationQuestions)]
    public class DeclarationQuestionsAppService : ThinknInsurTechAppServiceBase, IDeclarationQuestionsAppService
    {
        private readonly IRepository<DeclarationQuestion> _declarationQuestionRepository;

        public DeclarationQuestionsAppService(IRepository<DeclarationQuestion> declarationQuestionRepository)
        {
            _declarationQuestionRepository = declarationQuestionRepository;

        }

        public virtual async Task<PagedResultDto<GetDeclarationQuestionForViewDto>> GetAll(GetAllDeclarationQuestionsInput input)
        {

            var filteredDeclarationQuestions = _declarationQuestionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Question.Contains(input.Filter) || e.OptionType.Contains(input.Filter) || e.OptionValues.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.QuestionFilter), e => e.Question.Contains(input.QuestionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OptionTypeFilter), e => e.OptionType.Contains(input.OptionTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OptionValuesFilter), e => e.OptionValues.Contains(input.OptionValuesFilter))
                        .WhereIf(input.OrganizationUnitIdFilter.HasValue, e => e.OrganizationUnitId == input.OrganizationUnitIdFilter);


            var pagedAndFilteredDeclarationQuestions = filteredDeclarationQuestions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var declarationQuestions = from o in pagedAndFilteredDeclarationQuestions
                                       select new
                                       {

                                           o.Question,
                                           o.OptionType,
                                           o.OptionValues,
                                           Id = o.Id,
                                           o.OrganizationUnitId
                                       };

            var totalCount = await filteredDeclarationQuestions.CountAsync();

            var dbList = await declarationQuestions.ToListAsync();
            var results = new List<GetDeclarationQuestionForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDeclarationQuestionForViewDto()
                {
                    DeclarationQuestion = new DeclarationQuestionDto
                    {

                        Question = o.Question,
                        OptionType = o.OptionType,
                        OptionValues = o.OptionValues,
                        Id = o.Id,
                        OrganizationUnitId = o.OrganizationUnitId
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDeclarationQuestionForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetDeclarationQuestionForViewDto> GetDeclarationQuestionForView(int id)
        {
            var declarationQuestion = await _declarationQuestionRepository.GetAsync(id);

            var output = new GetDeclarationQuestionForViewDto { DeclarationQuestion = ObjectMapper.Map<DeclarationQuestionDto>(declarationQuestion) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DeclarationQuestions_Edit)]
        public virtual async Task<GetDeclarationQuestionForEditOutput> GetDeclarationQuestionForEdit(EntityDto input)
        {
            var declarationQuestion = await _declarationQuestionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDeclarationQuestionForEditOutput { DeclarationQuestion = ObjectMapper.Map<CreateOrEditDeclarationQuestionDto>(declarationQuestion) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditDeclarationQuestionDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_DeclarationQuestions_Create)]
        protected virtual async Task Create(CreateOrEditDeclarationQuestionDto input)
        {
            var declarationQuestion = ObjectMapper.Map<DeclarationQuestion>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                declarationQuestion.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }


            if (AbpSession.TenantId != null)
            {
                declarationQuestion.TenantId = (int)AbpSession.TenantId;
            }

            await _declarationQuestionRepository.InsertAsync(declarationQuestion);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DeclarationQuestions_Edit)]
        protected virtual async Task Update(CreateOrEditDeclarationQuestionDto input)
        {
            var declarationQuestion = await _declarationQuestionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, declarationQuestion);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_DeclarationQuestions_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _declarationQuestionRepository.DeleteAsync(input.Id);
        }

    }
}