
using System;
using System.Threading.Tasks;

namespace ThinknInsurTech.Common
{
    public interface IFileOrgManager
    {
        Task<FileOrg> GetOrNullAsync(int id);

        Task<FileOrg> GetOrNullAsyncByReferenceNo(Guid? id);

        Task SaveAsync(FileOrg file);

        Task DeleteAsync(int id);
    }
}
