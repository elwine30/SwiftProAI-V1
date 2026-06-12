using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinknInsurTech.Common
{
    public interface IFolderManager
    {
        Task<Folder> GetOrNullAsync(int id);

        Task SaveAsync(Folder file);

        Task DeleteAsync(int id);
    }
}
