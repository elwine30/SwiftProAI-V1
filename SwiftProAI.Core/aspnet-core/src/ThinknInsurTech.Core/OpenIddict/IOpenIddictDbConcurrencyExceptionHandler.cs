using System.Threading.Tasks;
using Abp.Domain.Uow;

namespace ThinknInsurTech.OpenIddict
{
    public interface IOpenIddictDbConcurrencyExceptionHandler
    {
        Task HandleAsync(AbpDbConcurrencyException exception);
    }
}