using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinknInsurTech.Integration
{
    public interface IOpenAIIntegrationLogManager : IDomainService
    {
        Task CreateOpenAIIntegrationLog(OpenAIIntegrationLog openAIIntegrationLog);
    }
}
