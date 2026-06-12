using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;
using System.Collections.Generic;

namespace ThinknInsurTech.Registration
{
    public interface IInvoiceItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetInvoiceItemForViewDto>> GetAll(GetAllInvoiceItemsInput input);

        Task<GetInvoiceItemForEditOutput> GetInvoiceItemForEdit(EntityDto input);

        Task<List<InvoiceItemDto>> GetInvoiceItemByRegisterId(EntityDto input);

        Task<List<InvoiceItemAmountDto>> GetInvoiceItemAmountsByRegisterId(EntityDto input);

        Task CreateOrEdit(CreateOrEditInvoiceItemDto input);

        Task Delete(EntityDto input);

        Task<List<InvoiceItemMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown();

    }
}