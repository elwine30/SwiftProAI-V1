using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Common.Dtos;

namespace ThinknInsurTech.Common
{
    public interface IFoldersAppService : IApplicationService
    {
        Task<PagedResultDto<GetFolderForViewDto>> GetAll(GetAllFoldersInput input);

        Task<GetFolderForViewDto> GetFolderForView(int id);

        Task<Dictionary<string, Dictionary<string, int>>> GetAllInDictionary(int registerId);



    }
}