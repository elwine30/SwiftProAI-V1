using Abp.Runtime.Security;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Configuration;

namespace ThinknInsurTech.ConnectionStringEncryption
{
    public class ConnectionStringEncryptionAppService : ThinknInsurTechAppServiceBase , IConnectionStringEncryptionAppService
    {
        private readonly IConfigurationRoot _appConfiguration;

        public ConnectionStringEncryptionAppService(IAppConfigurationAccessor appConfigurationAccessor)
        {
            _appConfiguration = appConfigurationAccessor.Configuration;
        }

        public string EncryptConnectionString()
        {

            var connectionString = _appConfiguration["ConnectionStrings:Default"];

            return SimpleStringCipher.Instance.Encrypt(connectionString, AppConsts.DefaultPassPhrase);
        }



    }
}
