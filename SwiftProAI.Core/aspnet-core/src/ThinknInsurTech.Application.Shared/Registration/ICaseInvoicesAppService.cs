using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using System.Collections.Generic;

namespace ThinknInsurTech.Registration
{
    public interface ICaseInvoicesAppService : IApplicationService
    {
        Task<GetCaseInvoiceForPreviewDto> GetCaseInvoiceForPreview(int id);

        Task<GetCaseInvoiceForViewDto> GetCaseInvoiceForView(int id);

        Task<GetCaseInvoiceForEditOutput> GetCaseInvoiceForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCaseInvoiceDto input);

        Task Delete(EntityDto input);

        Task<List<CaseInvoiceMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown();

        Task<List<CaseInvoiceUserLookupTableDto>> GetAllUserForTableDropdown();

        Task<List<CaseInvoiceCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown();

    }
}