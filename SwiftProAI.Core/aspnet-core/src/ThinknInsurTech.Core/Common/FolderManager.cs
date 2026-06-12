using Abp.Dependency;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinknInsurTech.Common
{
    public class FolderManager : IFolderManager, ITransientDependency
    {
        private readonly IRepository<Folder> _folderRepository;

        public FolderManager(IRepository<Folder> folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public Task<Folder> GetOrNullAsync(int id)
        {
            return _folderRepository.FirstOrDefaultAsync(id);
        }

        public Task SaveAsync(Folder file)
        {
            return _folderRepository.InsertAsync(file);
        }

        public Task DeleteAsync(int id)
        {
            return _folderRepository.DeleteAsync(id);
        }

    }
}
