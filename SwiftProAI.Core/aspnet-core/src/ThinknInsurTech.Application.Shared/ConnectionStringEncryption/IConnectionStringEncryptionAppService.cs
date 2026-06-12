using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.ConnectionStringEncryption
{
    public interface IConnectionStringEncryptionAppService : IApplicationService
    {
        string EncryptConnectionString();
    }
}
