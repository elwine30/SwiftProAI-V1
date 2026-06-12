using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace ThinknInsurTech.Registration
{
    public interface IHospitalsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHospitalForViewDto>> GetAll(GetAllHospitalsInput input);

        Task<GetHospitalForViewDto> GetHospitalForView(int id);

        Task<GetHospitalForEditOutput> GetHospitalForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditHospitalDto input);

        Task Delete(EntityDto input);

        

    }
}