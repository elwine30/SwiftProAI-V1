using Abp.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using GraphQL;
using IdentityModel.OidcClient;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThinknInsurTech.Integration;
using ThinknInsurTech.Integration.Dto;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Web.Controllers
{
    [Route("api/services/[controller]/[action]")]
    public class OCRController : ThinknInsurTechControllerBase
    {
        #region Declarations
        private readonly IIntegrationService _integrationService;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        #endregion

        #region Constructor
        public OCRController(
            IIntegrationService integrationService,
            ITempFileCacheManager tempFileCacheManager
        )
        {
            _integrationService = integrationService;
            _tempFileCacheManager = tempFileCacheManager;

        }
        #endregion

        #region Methods
        /// <summary>
        /// Upload police report in image form to third party
        /// </summary>
        /// <returns>Extracted data based on prompt</returns>
        [HttpPost]
        [AbpAuthorize]
        public async Task<JsonResult> UploadPoliceReport()
        {
            try
            {
                var logoFile = Request.Form.Files.First();

                //Check input
                if (logoFile == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (logoFile.Length > 2097152) //2mb = 2 * 1024 * 1024
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                var imageBase64String = "";

                using (var stream = logoFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                    imageBase64String = Convert.ToBase64String(fileBytes);
                }

                var imageFormat = GetRawImageFormat(fileBytes);

                if (imageFormat.Name != "JPEG" && imageFormat.Name != "JPG" && imageFormat.Name != "PNG")
                {
                    throw new UserFriendlyException("File_Invalid_Type_Error");
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
                            content = new List<object>
                            {
                                new
                                {
                                    type = "text",
                                    text = "You are an admin clerk. You would be provided with a scanned police report which is written in Malay. Extract the following info from the report into a JSON object using camel case format without any extra string before and after the object { }. The incident date time is usually located in the summary.\r\n\r\nPolice station\r\nOfficer name\r\nOfficer service no\r\nReport no\r\nReport datetime\r\nIncident datetime (Retrieve from the report summary)\r\nReporter name\r\nOrganization name\r\nIncident location\r\nSummary of incident reported\r\nNature of incident reported\r\nVehicle registration numbers\r\nVehicle models\r\nVehicle colours\r\nFor any data fields that are not available, set as \"NA\". Return vehicle registration numbers, vehicle models, vehicle colours in an array of strings. Return the summary of the incident reported from a third-party narrative perspective, with car plate details included for each vehicle involved, translated to English."
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


                // hardcoded data returned from chatgpt,
                // uncomment this for local dev testing that dont required api call to chatgpt as each request will be charged
                // Police Report Result
                //var result = "{\r\n  \"id\": \"chatcmpl-9g5y4xXc1vTYMOASa9Mdd3OyFPh8B\",\r\n  \"object\": \"chat.completion\",\r\n  \"created\": 1719819744,\r\n  \"model\": \"gpt-4o-2024-05-13\",\r\n  \"choices\": [\r\n    {\r\n      \"index\": 0,\r\n      \"message\": {\r\n        \"role\": \"assistant\",\r\n        \"content\": \"```json\\n{\\n  \\\"policeStation\\\": \\\"Trafik Johor Bahru (S)\\\",\\n  \\\"officerName\\\": \\\"Jenefter Tay\\\",\\n  \\\"officerServiceNo\\\": \\\"R190818\\\",\\n  \\\"reportNo\\\": \\\"Trafik Johor Bahru (S)031197/19\\\",\\n  \\\"reportDatetime\\\": \\\"01/12/2019 14:02 PM\\\",\\n  \\\"incidentDatetime\\\": \\\"01/12/2019 09:00 AM\\\",\\n  \\\"reporterName\\\": \\\"Siah Tang Hiang\\\",\\n  \\\"organizationName\\\": \\\"NA\\\",\\n  \\\"incidentLocation\\\": \\\"Tambak Johor\\\",\\n  \\\"summaryOfIncidentReported\\\": \\\"On 01/12/2019 around 09:00 AM, I was driving a car with registration number JSE6990 from Johor Bahru heading towards Singapore. Upon reaching the Tambak Johor, while the road was congested, I stopped the car, and suddenly the car was hit strongly from behind by a lorry with registration number MKAR. My car was damaged at the rear bumper, lights, sensors, bonnet, and other parts which are yet to be verified.\\\",\\n  \\\"natureOfIncidentReported\\\": \\\"NA\\\",\\n  \\\"vehicleRegistrationNumbers\\\": [\\\"JSE6990\\\", \\\"MKAR\\\"],\\n  \\\"vehicleModels\\\": [\\\"NA\\\"],\\n  \\\"vehicleColours\\\": [\\\"NA\\\"]\\n}\\n```\"\r\n      },\r\n      \"logprobs\": null,\r\n      \"finish_reason\": \"stop\"\r\n    }\r\n  ],\r\n  \"usage\": {\r\n    \"prompt_tokens\": 952,\r\n    \"completion_tokens\": 286,\r\n    \"total_tokens\": 1238\r\n  },\r\n  \"system_fingerprint\": \"fp_4008e3b719\"\r\n}\r\n";

                return Json(new AjaxResponse(new { result }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        /// <summary>
        /// Upload driving license in image form to third party
        /// </summary>
        /// <returns>Extracted data based on prompt</returns>
        [HttpPost]
        [AbpAuthorize]
        public async Task<JsonResult> UploadFrontDrivingLicense()
        {
            try
            {
                var logoFile = Request.Form.Files.First();

                //Check input
                if (logoFile == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (logoFile.Length > 2097152) //2mb = 2 * 1024 * 1024
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                var imageBase64String = "";

                using (var stream = logoFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                    imageBase64String = Convert.ToBase64String(fileBytes);
                }

                var imageFormat = GetRawImageFormat(fileBytes);

                if (imageFormat.Name != "JPEG" && imageFormat.Name != "JPG" && imageFormat.Name != "PNG")
                {
                    throw new UserFriendlyException("File_Invalid_Type_Error");
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
                            content = new List<object>
                            {
                                new
                                {
                                    type = "text",
                                    text = "You are an admin clerk. You would be provided with a scanned malaysian driving license at the front side of the card. Please extract the following info from the report into a JSON object using camel case format without any extra string before and after the object { }.\n" +
                                           "a. License classes\n" +
                                           "b. License date from\n" +
                                           "c. License date to\n" +
                                           "For any data fields that is not available, set as \"NA\". " +
                                           "Please return License classes in an array of string."
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

                //var result = "{\n  \"id\": \"chatcmpl-9guc8fSmTlW6v6SkCJuwe7boO0fDQ\",\n  \"object\": \"chat.completion\",\n  \"created\": 1720014428,\n  \"model\": \"gpt-4o-2024-05-13\",\n  \"choices\": [\n    {\n      \"index\": 0,\n      \"message\": {\n        \"role\": \"assistant\",\n        \"content\": \"{\\n  \\\"licenseClasses\\\": [\\\"D\\\"],\\n  \\\"licenseDateFrom\\\": \\\"01/11/2021\\\",\\n  \\\"licenseDateTo\\\": \\\"10/07/2027\\\"\\n}\"\n      },\n      \"logprobs\": null,\n      \"finish_reason\": \"stop\"\n    }\n  ],\n  \"usage\": {\n    \"prompt_tokens\": 867,\n    \"completion_tokens\": 38,\n    \"total_tokens\": 905\n  },\n  \"system_fingerprint\": \"fp_4008e3b719\"\n}\n";

                return Json(new AjaxResponse(new { result }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        /// <summary>
        /// Upload driving license in image form to third party
        /// </summary>
        /// <returns>Extracted data based on prompt</returns>
        [HttpPost]
        [AbpAuthorize]
        public async Task<JsonResult> UploadBackDrivingLicense()
        {
            try
            {
                var logoFile = Request.Form.Files.First();

                //Check input
                if (logoFile == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (logoFile.Length > 2097152) //2mb = 2 * 1024 * 1024
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                var imageBase64String = "";

                using (var stream = logoFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                    imageBase64String = Convert.ToBase64String(fileBytes);
                }

                var imageFormat = GetRawImageFormat(fileBytes);

                if (imageFormat.Name != "JPEG" && imageFormat.Name != "JPG" && imageFormat.Name != "PNG")
                {
                    throw new UserFriendlyException("File_Invalid_Type_Error");
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
                            content = new List<object>
                            {
                                new
                                {
                                    type = "text",
                                    text = "You are an admin clerk. You would be provided with a scanned malaysian driving license at the back side of the card. Please extract the License No from the top right of the card into a JSON object using camel case format without any extra string before and after the object { }. For any data fields that is not available, set as \"NA\"."
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

                //var result = await _integrationService.ChatGPTCompletions(input);

                var result = "{\n  \"id\": \"chatcmpl-9gw7rHQCGzFY79IbnS09ucJknTyzS\",\n  \"object\": \"chat.completion\",\n  \"created\": 1720020239,\n  \"model\": \"gpt-4o-2024-05-13\",\n  \"choices\": [\n    {\n      \"index\": 0,\n      \"message\": {\n        \"role\": \"assistant\",\n        \"content\": \"{\\n  \\\"licenseNo\\\": \\\"41771307\\\"\\n}\"\n      },\n      \"logprobs\": null,\n      \"finish_reason\": \"stop\"\n    }\n  ],\n  \"usage\": {\n    \"prompt_tokens\": 843,\n    \"completion_tokens\": 12,\n    \"total_tokens\": 855\n  },\n  \"system_fingerprint\": \"fp_4008e3b719\"\n}\n";

                return Json(new AjaxResponse(new { result }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        // in-progress: integrating with FE
        [HttpPost]
        [AbpAuthorize]
        public async Task<JsonResult> UploadDocument(string prompt)
        {
            try
            {
                var file = Request.Form.Files.First();

                //Check input
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 2097152) //2mb = 2 * 1024 * 1024
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                var imageBase64String = "";

                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                    imageBase64String = Convert.ToBase64String(fileBytes);
                }

                var imageFormat = GetRawImageFormat(fileBytes);

                if (imageFormat.Name != "JPEG" && imageFormat.Name != "JPG" && imageFormat.Name != "PNG")
                {
                    throw new UserFriendlyException("File_Invalid_Type_Error");
                }
                var fileToken = Guid.NewGuid().ToString("N");
                _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, imageFormat.Name, fileBytes));
                var result = string.Empty;
                //If there is prompt run openAI call. If not return the fileToken 
                if (prompt != null)
                {
                    // Police Report Result
                    //var result = "{\r\n  \"id\": \"chatcmpl-9g5y4xXc1vTYMOASa9Mdd3OyFPh8B\",\r\n  \"object\": \"chat.completion\",\r\n  \"created\": 1719819744,\r\n  \"model\": \"gpt-4o-2024-05-13\",\r\n  \"choices\": [\r\n    {\r\n      \"index\": 0,\r\n      \"message\": {\r\n        \"role\": \"assistant\",\r\n        \"content\": \"```json\\n{\\n  \\\"policeStation\\\": \\\"Trafik Johor Bahru (S)\\\",\\n  \\\"officerName\\\": \\\"Jenefter Tay\\\",\\n  \\\"officerServiceNo\\\": \\\"R190818\\\",\\n  \\\"reportNo\\\": \\\"Trafik Johor Bahru (S)031197/19\\\",\\n  \\\"reportDatetime\\\": \\\"01/12/2019 14:02 PM\\\",\\n  \\\"incidentDatetime\\\": \\\"01/12/2019 09:00 AM\\\",\\n  \\\"reporterName\\\": \\\"Siah Tang Hiang\\\",\\n  \\\"organizationName\\\": \\\"NA\\\",\\n  \\\"incidentLocation\\\": \\\"Tambak Johor\\\",\\n  \\\"summaryOfIncidentReported\\\": \\\"On 01/12/2019 around 09:00 AM, I was driving a car with registration number JSE6990 from Johor Bahru heading towards Singapore. Upon reaching the Tambak Johor, while the road was congested, I stopped the car, and suddenly the car was hit strongly from behind by a lorry with registration number MKAR. My car was damaged at the rear bumper, lights, sensors, bonnet, and other parts which are yet to be verified.\\\",\\n  \\\"natureOfIncidentReported\\\": \\\"NA\\\",\\n  \\\"vehicleRegistrationNumbers\\\": [\\\"JSE6990\\\", \\\"MKAR\\\"],\\n  \\\"vehicleModels\\\": [\\\"NA\\\"],\\n  \\\"vehicleColours\\\": [\\\"NA\\\"]\\n}\\n```\"\r\n      },\r\n      \"logprobs\": null,\r\n      \"finish_reason\": \"stop\"\r\n    }\r\n  ],\r\n  \"usage\": {\r\n    \"prompt_tokens\": 952,\r\n    \"completion_tokens\": 286,\r\n    \"total_tokens\": 1238\r\n  },\r\n  \"system_fingerprint\": \"fp_4008e3b719\"\r\n}\r\n";

                    // Identity Card Result
                    result = "{\r\n  \"id\": \"chatcmpl-9gNXlSaw69BKxkfJZBbL68VOYze6A\",\r\n  \"object\": \"chat.completion\",\r\n  \"created\": 1719887305,\r\n  \"model\": \"gpt-4o-2024-05-13\",\r\n  \"choices\": [\r\n    {\r\n      \"index\": 0,\r\n      \"message\": {\r\n        \"role\": \"assistant\",\r\n        \"content\": \"{\\n  \\\"identityNumber\\\": \\\"550106-12-5821\\\",\\n  \\\"identityType\\\": \\\"IC\\\",\\n  \\\"fullName\\\": \\\"Rowan Sebastian Atkinson\\\",\\n  \\\"addressLine1\\\": \\\"GDW Kampung Bayangan\\\",\\n  \\\"postcode\\\": \\\"80000\\\",\\n  \\\"city\\\": \\\"Keningau\\\",\\n  \\\"state\\\": \\\"Sabah\\\",\\n  \\\"country\\\": \\\"NA\\\",\\n  \\\"nationality\\\": \\\"Malaysian\\\",\\n  \\\"gender\\\": \\\"Male\\\"\\n}\"\r\n      },\r\n      \"logprobs\": null,\r\n      \"finish_reason\": \"stop\"\r\n    }\r\n  ],\r\n  \"usage\": {\r\n    \"prompt_tokens\": 378,\r\n    \"completion_tokens\": 99,\r\n    \"total_tokens\": 477\r\n  },\r\n  \"system_fingerprint\": \"fp_4008e3b719\"\r\n}\r\n";

                    // Driving License Front Result
                    //var result = "{\n  \"id\": \"chatcmpl-9guc8fSmTlW6v6SkCJuwe7boO0fDQ\",\n  \"object\": \"chat.completion\",\n  \"created\": 1720014428,\n  \"model\": \"gpt-4o-2024-05-13\",\n  \"choices\": [\n    {\n      \"index\": 0,\n      \"message\": {\n        \"role\": \"assistant\",\n        \"content\": \"{\\n  \\\"licenseClasses\\\": [\\\"D\\\"],\\n  \\\"licenseDateFrom\\\": \\\"01/11/2021\\\",\\n  \\\"licenseDateTo\\\": \\\"10/07/2027\\\"\\n}\"\n      },\n      \"logprobs\": null,\n      \"finish_reason\": \"stop\"\n    }\n  ],\n  \"usage\": {\n    \"prompt_tokens\": 867,\n    \"completion_tokens\": 38,\n    \"total_tokens\": 905\n  },\n  \"system_fingerprint\": \"fp_4008e3b719\"\n}\n";

                    //Driving License Back Result
                    //var result = "{\n  \"id\": \"chatcmpl-9gw7rHQCGzFY79IbnS09ucJknTyzS\",\n  \"object\": \"chat.completion\",\n  \"created\": 1720020239,\n  \"model\": \"gpt-4o-2024-05-13\",\n  \"choices\": [\n    {\n      \"index\": 0,\n      \"message\": {\n        \"role\": \"assistant\",\n        \"content\": \"{\\n  \\\"licenseNo\\\": \\\"41771307\\\"\\n}\"\n      },\n      \"logprobs\": null,\n      \"finish_reason\": \"stop\"\n    }\n  ],\n  \"usage\": {\n    \"prompt_tokens\": 843,\n    \"completion_tokens\": 12,\n    \"total_tokens\": 855\n  },\n  \"system_fingerprint\": \"fp_4008e3b719\"\n}\n";
                    //var result = await _integrationService.ChatGPTCompletions(input);
                }

                var response = new
                {
                    success = true,
                    result = result,
                    fileToken = fileToken,
                };
                return Json(new AjaxResponse(response));
            }
            catch (UserFriendlyException ex)
            {
                var errorResponse = new
                {
                    success = false,
                    error = new ErrorInfo(ex.Message)
                };
                return Json(new AjaxResponse(errorResponse));
            }
        }

        /// <summary>
        /// Return imageformat based on input
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <returns>Imageformat interface</returns>
        private static IImageFormat GetRawImageFormat(byte[] fileBytes)
        {
            using (var ms = new MemoryStream(fileBytes))
            {
                var fileFormat = Image.DetectFormat(ms);

                return fileFormat;
            }
        }

        #endregion
    }
}
