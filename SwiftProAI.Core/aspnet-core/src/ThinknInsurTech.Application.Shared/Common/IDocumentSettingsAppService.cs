using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Common.Dtos;

namespace ThinknInsurTech.Common
{
    public interface IDocumentSettingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDocumentSettingForViewDto>> GetAll(GetAllDocumentSettingsInput input);

        Task<GetDocumentSettingForViewDto> GetDocumentSettingForView(int id);

        Task<GetDocumentSettingForEditOutput> GetDocumentSettingForEdit();

        Task CreateOrEdit(CreateOrEditDocumentSettingDto input);

        Task Delete(EntityDto input);

    }
}