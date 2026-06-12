using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using ThinknInsurTech.Companies.Dtos;
using System.Collections.Generic;

namespace ThinknInsurTech.Companies
{
    public interface ICompanyAppService : IApplicationService
    {
        Task<CompanyDto> GetCompanyDetailsbyId(int id);

        Task<ListResultDto<CompanyDto>> GetAllCompanyDetails();

        Task<PagedResultDto<GetCompanyForViewDto>> GetAll(GetAllCompaniesInput input);

        Task<GetCompanyForViewDto> GetCompanyForView(int id);

        Task<GetCompanyForEditOutput> GetCompanyForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCompanyDto input);

        Task Delete(EntityDto input);

        Task<List<CompanyCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown();
    }
}
