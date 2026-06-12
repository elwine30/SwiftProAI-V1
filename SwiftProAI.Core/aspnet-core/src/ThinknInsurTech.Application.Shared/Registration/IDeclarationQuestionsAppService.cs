using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Registration
{
    public interface IDeclarationQuestionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDeclarationQuestionForViewDto>> GetAll(GetAllDeclarationQuestionsInput input);

        Task<GetDeclarationQuestionForViewDto> GetDeclarationQuestionForView(int id);

        Task<GetDeclarationQuestionForEditOutput> GetDeclarationQuestionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditDeclarationQuestionDto input);

        Task Delete(EntityDto input);

    }
}