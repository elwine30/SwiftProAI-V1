
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.LawFirms.Dtos;


namespace ThinknInsurTech.LawFirms
{
    public interface ILawFirmsAppService : IApplicationService
    {
        Task<PagedResultDto<GetLawFirmForViewDto>> GetAll(GetAllLawFirmsInput input);

        Task<GetLawFirmForViewDto> GetLawFirmForView(int id);

        Task<GetLawFirmForEditOutput> GetLawFirmForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditLawFirmDto input);

        Task Delete(EntityDto input);

    }
}