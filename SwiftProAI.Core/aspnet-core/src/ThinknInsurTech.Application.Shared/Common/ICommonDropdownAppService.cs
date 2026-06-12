using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Common.Dtos;

namespace ThinknInsurTech.Common
{
    public interface ICommonDropdownAppService: IApplicationService
    {
        Task<List<CommonDropdownDto>> GetAllBranchForTableDropdown();
        Task<List<CommonDropdownDto>> GetAllCompanyForTableDropdown();
        Task<List<CommonAdjusterDropdownDto>> GetAllAdjusterForTableDropdown();
        Task<List<CommonAdjusterDropdownDto>> GetAllAdjusterByBranchForTableDropdown(int branchId);
        Task<List<CommonDropdownDto>> GetAllLocationByCountryForTableDropdown(int parentLocationId);
        Task<List<CommonDropdownDto>> GetAllLocationByStateForTableDropdown(int parentLocationId);
        Task<List<CommonDropdownDto>> GetAllHospitalForTableDropdown();
        Task<CommonHospitalDropdownDto> GetHospitalByIdForTableDropdown(int hospitalId);
        Task<List<CommonDropdownDto>> GetAllMakerVehicle();
        Task<List<CommonDropdownDto>> GetAllModelByMakerVehicle(string maker);
        Task<List<CommonDropdownDto>> GetAllSpecsByModelAndMakerVehicle(string maker, string model);
    }
}
