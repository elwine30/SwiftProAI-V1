using Abp.Dependency;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Common
{

    public class FileOrgManager : IFileOrgManager, ITransientDependency
    {
        private readonly IRepository<FileOrg> _fileOrgRepository;

        public FileOrgManager(IRepository<FileOrg> fileOrgRepository)
        {
            _fileOrgRepository = fileOrgRepository;
        }

        public Task<FileOrg> GetOrNullAsync(int id)
        {
            return _fileOrgRepository.FirstOrDefaultAsync(id);
        }

        public Task<FileOrg> GetOrNullAsyncByReferenceNo(Guid? id)
        {
            return _fileOrgRepository.FirstOrDefaultAsync(f => f.ReferenceNo == id);
        }

        public Task SaveAsync(FileOrg file)
        {
            return _fileOrgRepository.InsertAsync(file);
        }

        public Task DeleteAsync(int id)
        {
            return _fileOrgRepository.DeleteAsync(id);
        }

    }
}
