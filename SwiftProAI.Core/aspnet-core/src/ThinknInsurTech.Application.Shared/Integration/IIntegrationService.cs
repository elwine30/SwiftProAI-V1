using Abp.Application.Services;
using System.Threading.Tasks;
using ThinknInsurTech.Integration.Dto;

namespace ThinknInsurTech.Integration
{
    public interface IIntegrationService : IApplicationService
    {
        /// <summary>
        /// POST request to ChatGPT
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Response from ChatGPT based on input content</returns>
        Task<string> ChatGPTCompletions(ChatGPTInputDto input);
    }
}
