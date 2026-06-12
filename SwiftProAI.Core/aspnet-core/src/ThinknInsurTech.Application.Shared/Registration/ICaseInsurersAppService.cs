using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    public interface ICaseInsurersAppService : IApplicationService
    {
        Task<PagedResultDto<GetCaseInsurerForViewDto>> GetAll(GetAllCaseInsurersInput input);


        Task<GetCaseInsurerForEditOutput> GetCaseInsurerForEdit(int Id);

        Task CreateOrEdit(CreateOrEditCaseInsurerDto input);

        Task<GetCaseInsurerForViewDto> GetCaseInsurerForView(EntityDto input);

        //Task Delete(EntityDto input);

    }
}