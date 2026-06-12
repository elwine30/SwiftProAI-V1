using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Workshops.Dtos;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Workshops
{
    public interface IWorkshopsAppService : IApplicationService
    {
        Task<PagedResultDto<GetWorkshopForViewDto>> GetAll(GetAllWorkshopsInput input);

        Task<GetWorkshopForViewDto> GetWorkshopForView(int id);

        Task<GetWorkshopForEditOutput> GetWorkshopForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditWorkshopDto input);

        Task Delete(EntityDto input);

    }
}