using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
