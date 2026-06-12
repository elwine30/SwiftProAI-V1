using System.Threading.Tasks;
using Abp.Dependency;

namespace ThinknInsurTech.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}