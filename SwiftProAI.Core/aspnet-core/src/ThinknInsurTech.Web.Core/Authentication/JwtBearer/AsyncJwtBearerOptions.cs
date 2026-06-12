using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ThinknInsurTech.Web.Authentication.JwtBearer
{
    public class AsyncJwtBearerOptions : JwtBearerOptions
    {
        public readonly List<IAsyncSecurityTokenValidator> AsyncSecurityTokenValidators;
        
        private readonly ThinknInsurTechAsyncJwtSecurityTokenHandler _defaultAsyncHandler = new ThinknInsurTechAsyncJwtSecurityTokenHandler();

        public AsyncJwtBearerOptions()
        {
            AsyncSecurityTokenValidators = new List<IAsyncSecurityTokenValidator>() {_defaultAsyncHandler};
        }
    }

}
