using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThinknInsurTech.Remarks
{
    public interface IRemarkManager : IDomainService
    {
        Task<int> CreateRemarkAsync(Remark remark);

        Task<List<Remark>> GetAllRemarkAsync();

        Task<Remark> GetRemarkbyIdAsync(int remarkId);

        Task<List<Remark>> GetAllRemarksByRegistrationId(int registrationId);

    }
}
