using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinknInsurTech.Configuration;
using ThinknInsurTech.Integration.Dto;
using ThinknInsurTech.Integration.Dtos;
using ThinknInsurTech.Registration;
using Prompt = ThinknInsurTech.OCR.Prompt;

namespace ThinknInsurTech.Integration
{
    public class OCRPromptService : IOCRPromptService
    {
        private readonly IIntegrationService _integrationService;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IOpenAIIntegrationLogManager _openAILogManager;
        private readonly IAbpSession _abpSesssion;
        private readonly IRepository<Prompt> _promptRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public OCRPromptService(IIntegrationService integrationService, IAppConfigurationAccessor appConfigurationAccessor, IOpenAIIntegrationLogManager openAILogManager, IAbpSession abpSesssion, IRepository<Prompt> promptRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _integrationService = integrationService;
            _appConfiguration = appConfigurationAccessor.Configuration;
            _openAILogManager = openAILogManager;
            _abpSesssion = abpSesssion;
            _promptRepository = promptRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public string GetPromptByPromptName(string promptName)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var prompt = _promptRepository.GetAll().IgnoreQueryFilters().Where(x => x.PromptName == promptName).FirstOrDefault();
                return prompt.PromptRequest;
            }
        }

        public async Task<string> UploadBulkDocument(UploadDocTypeEnum docType, string[] base64Files, string filename, string caseNo)
        {
            var prompt = GetPromptByPromptName(docType.ToString());

            var messageContent = new List<object>
            {
                new
                {
                    type = "text",
                    text = prompt,
                }
            };
            // Multiple Image
            foreach (var files in base64Files)
            {
                messageContent.Add(new
                {
                    type = "image_url",
                    image_url = new
                    {
                        url = $"data:image/jpeg;base64,{files}",
                    }
                });
            }

            var input = new ChatGPTInputDto()
            {
                model = "gpt-4o",
                max_tokens = 4095,
                temperature = 1,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0,
                messages = new List<ChatGPTInputMessagesDto>()
                {
                    new ChatGPTInputMessagesDto
                    {
                        role = "user",
                        content = messageContent
                    }
                }
            };


            var result = await _integrationService.ChatGPTCompletions(input);

            OpenAIIntegrationLogRequest requestLog = new OpenAIIntegrationLogRequest
            {
                FileName = filename,
                Prompt = prompt,
            };

            var outputObj = JObject.Parse(result); // may throw parse error from invalid json returned from chatgpt
            var promptTokens = Convert.ToInt32(outputObj["usage"]["prompt_tokens"]);
            var completionTokens = Convert.ToInt32(outputObj["usage"]["completion_tokens"]);
            var totalTokens = Convert.ToInt32(outputObj["usage"]["total_tokens"]);
            var inputTokenCost = Convert.ToDecimal(_appConfiguration.GetSection("OCR")["InputTokenCost"]);
            var outputTokenCost = Convert.ToDecimal(_appConfiguration.GetSection("OCR")["OutputTokenCost"]);

            OpenAIIntegrationLog openAIIntegrationLog = new OpenAIIntegrationLog
            {
                ActionUrl = Enum.GetName(typeof(UploadDocTypeEnum), docType),
                Request = JsonConvert.SerializeObject(requestLog),
                Response = result,
                PromptToken = promptTokens,
                CompletionToken = completionTokens,
                TotalCost = (promptTokens * inputTokenCost) + (completionTokens * outputTokenCost),
                CreatorUserId = _abpSesssion.UserId ?? null,
                CaseNo = caseNo,
            };

            await _openAILogManager.CreateOpenAIIntegrationLog(openAIIntegrationLog);

            return result;
        }
        public async Task<string> UploadDocument(UploadDocTypeEnum docType, string imageBase64String, string filename, string caseNo)
        {

            var prompt = GetPromptByPromptName(docType.ToString());
            var input = new ChatGPTInputDto()
            {
                model = "gpt-4o",
                max_tokens = 4095,
                temperature = 1,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0,
                messages = new List<ChatGPTInputMessagesDto>()
                    {
                        new ChatGPTInputMessagesDto
                        {
                            role = "user",
                            content = new List<object>
                            {
                                new
                                {
                                    type = "text",
                                    text = prompt,
                                },

                                new
                                {
                                    type = "image_url",
                                    image_url = new
                                    {
                                        url = $"data:image/jpeg;base64,{imageBase64String}",
                                    }
                                }
                            }
                        }
                    }
            };

            var result = await _integrationService.ChatGPTCompletions(input);

            OpenAIIntegrationLogRequest requestLog = new OpenAIIntegrationLogRequest
            {
                FileName = filename,
                Prompt = prompt,
            };

            var outputObj = JObject.Parse(result); // may throw parse error from invalid json returned from chatgpt
            var promptTokens = Convert.ToInt32(outputObj["usage"]["prompt_tokens"]);
            var completionTokens = Convert.ToInt32(outputObj["usage"]["completion_tokens"]);
            var totalTokens = Convert.ToInt32(outputObj["usage"]["total_tokens"]);
            var inputTokenCost = Convert.ToDecimal(_appConfiguration.GetSection("OCR")["InputTokenCost"]);
            var outputTokenCost = Convert.ToDecimal(_appConfiguration.GetSection("OCR")["OutputTokenCost"]);

            OpenAIIntegrationLog openAIIntegrationLog = new OpenAIIntegrationLog
            {
                ActionUrl = Enum.GetName(typeof(UploadDocTypeEnum), docType),
                Request = JsonConvert.SerializeObject(requestLog),
                Response = result,
                PromptToken = promptTokens,
                CompletionToken = completionTokens,
                TotalCost = (promptTokens * inputTokenCost) + (completionTokens * outputTokenCost),
                CreatorUserId = _abpSesssion.UserId ?? null,
                CaseNo = caseNo,
            };

            await _openAILogManager.CreateOpenAIIntegrationLog(openAIIntegrationLog);

            return result;
        }

        public async Task<string> GenerateSummary(string inputData, string promptName, string caseNo)
        {
            var prompt = GetPromptByPromptName(promptName) + inputData;
            var input = new ChatGPTInputDto()
            {
                model = "gpt-4o",
                max_tokens = 4095,
                temperature = 1,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0,
                messages = new List<ChatGPTInputMessagesDto>()
                    {
                        new ChatGPTInputMessagesDto
                        {
                            role = "user",
                            content = new List<object>
                            {
                                new
                                {
                                    type = "text",
                                    text = prompt,
                                }
                            }
                        }
                    }
            };

            var result = await _integrationService.ChatGPTCompletions(input);
            OpenAIIntegrationLogRequest requestLog = new OpenAIIntegrationLogRequest
            {
                Prompt = "mock test",
            };

            var outputObj = JObject.Parse(result); // may throw parse error from invalid json returned from chatgpt
            var promptTokens = Convert.ToInt32(outputObj["usage"]["prompt_tokens"]);
            var completionTokens = Convert.ToInt32(outputObj["usage"]["completion_tokens"]);
            var totalTokens = Convert.ToInt32(outputObj["usage"]["total_tokens"]);
            var inputTokenCost = Convert.ToDecimal(_appConfiguration.GetSection("OCR")["InputTokenCost"]);
            var outputTokenCost = Convert.ToDecimal(_appConfiguration.GetSection("OCR")["OutputTokenCost"]);

            OpenAIIntegrationLog openAIIntegrationLog = new OpenAIIntegrationLog
            {
                ActionUrl = promptName,
                Request = JsonConvert.SerializeObject(requestLog),
                Response = result,
                PromptToken = promptTokens,
                CompletionToken = completionTokens,
                TotalCost = (promptTokens * inputTokenCost) + (completionTokens * outputTokenCost),
                CreatorUserId = _abpSesssion.UserId ?? null,
                CaseNo = caseNo,
            };

            await _openAILogManager.CreateOpenAIIntegrationLog(openAIIntegrationLog);
            return result;
        }
    }
}
