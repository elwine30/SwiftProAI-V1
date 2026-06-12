using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Registration
{
    public interface IInsuredPersonsAppService : IApplicationService
    {
        Task<GetInsuredPersonForEditOutput> GetInsuredPersonForEdit(EntityDto input, bool isOwner);

        Task<bool?> CreateOrEdit(CreateOrEditInsuredPersonDto input);

        Task Delete(EntityDto input);

        Task<List<InsuredPersonMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown();

        Task<List<CommonHospitalDropdownDto>> GetAllHospitalForTableDropdown();

        Task<List<CommonDropdownDto>> GetAllLocationForTableDropdown(int parentLocationId);

        Task RemoveDriverICFrontFile(EntityDto input);

        Task RemoveDriverICBackFile(EntityDto input);

        Task RemoveDriverLicenseFrontFile(EntityDto input);

        Task RemoveDriverLicenseBackFile(EntityDto input);

        Task RemoveDriverEmploymentDetailFile(EntityDto input);

        Task RemoveDriverHospitalDetailFile(EntityDto input);

        Task<GetInsuredPersonForViewDto> GetInsuredPersonForView(EntityDto input, bool isOwner);

    }
}