using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.OCR.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.OCR
{
    public interface IPromptsAppService : IApplicationService
    {
        Task<PagedResultDto<GetPromptForViewDto>> GetAll(GetAllPromptsInput input);

        Task<GetPromptForViewDto> GetPromptForView(int id);

        Task<GetPromptForEditOutput> GetPromptForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditPromptDto input);
    }
}