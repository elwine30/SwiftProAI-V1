using Abp.Runtime.Session;

namespace ThinknInsurTech.Runtime
{
    public interface IOUAbpSession : IAbpSession
    {
        long? CurrentOUId { get; }
    }
}
