using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinknInsurTech.Integration
{
    public class OpenAIIntegrationLogManager : ThinknInsurTechDomainServiceBase, IOpenAIIntegrationLogManager
    {

        private readonly IRepository<OpenAIIntegrationLog, int> _OpenAIIntegrationLogRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public OpenAIIntegrationLogManager(IRepository<OpenAIIntegrationLog, int> OpenAIIntegrationLogRepository, IUnitOfWorkManager unitOfWorkManager, ITenantCache tenantCache)
        {
            _OpenAIIntegrationLogRepository = OpenAIIntegrationLogRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task CreateOpenAIIntegrationLog(OpenAIIntegrationLog openAIIntegrationLog)
        {
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                await _OpenAIIntegrationLogRepository.InsertAsync(openAIIntegrationLog);
            });
        }

    }
}
