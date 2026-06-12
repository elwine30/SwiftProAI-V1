using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using ThinknInsurTech.MultiTenancy.Accounting.Dto;

namespace ThinknInsurTech.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
