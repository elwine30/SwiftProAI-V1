using Abp.Runtime.Session;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ThinknInsurTech.Configuration;
using ThinknInsurTech.Registration;

namespace ThinknInsurTech.Integration
{
    public class MockOCRPromptService : IOCRPromptService
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IOpenAIIntegrationLogManager _openAILogManager;
        private readonly IAbpSession _abpSesssion;

        public MockOCRPromptService(IAppConfigurationAccessor appConfigurationAccessor, IOpenAIIntegrationLogManager openAILogManager, IAbpSession abpSession)
        {
            _appConfiguration = appConfigurationAccessor.Configuration;
            _openAILogManager = openAILogManager;
            _abpSesssion = abpSession;
        }
        public async Task<string> UploadBulkDocument(UploadDocTypeEnum docType, string[] base64Files, string filename, string caseNo)
        {
            var result = "";
            switch (docType)
            {
                case UploadDocTypeEnum.ICFront:
                    result = "{\r\n  \"id\": \"chatcmpl-9gNXlSaw69BKxkfJZBbL68VOYze6A\",\r\n  \"object\": \"chat.completion\",\r\n  \"created\": 1719887305,\r\n  \"model\": \"gpt-4o-2024-05-13\",\r\n  \"choices\": [\r\n    {\r\n      \"index\": 0,\r\n      \"message\": {\r\n        \"role\": \"assistant\",\r\n        \"content\": \"{\\n  \\\"identityNumber\\\": \\\"550106-12-5821\\\",\\n  \\\"identityType\\\": \\\"IC\\\",\\n  \\\"fullName\\\": \\\"Rowan Sebastian Atkinson\\\",\\n  \\\"addressLine1\\\": \\\"GDW Kampung Bayangan\\\",\\n  \\\"postcode\\\": \\\"80000\\\",\\n  \\\"city\\\": \\\"Keningau\\\",\\n  \\\"state\\\": \\\"Sabah\\\",\\n  \\\"country\\\": \\\"NA\\\",\\n  \\\"nationality\\\": \\\"Malaysian\\\",\\n  \\\"gender\\\": \\\"Male\\\"\\n}\"\r\n      },\r\n      \"logprobs\": null,\r\n      \"finish_reason\": \"stop\"\r\n    }\r\n  ],\r\n  \"usage\": {\r\n    \"prompt_tokens\": 378,\r\n    \"completion_tokens\": 99,\r\n    \"total_tokens\": 477\r\n  },\r\n  \"system_fingerprint\": \"fp_4008e3b719\"\r\n}\r\n";
                    break;
                case UploadDocTypeEnum.LicenseFront:
                    result = "{\n  \"id\": \"chatcmpl-9guc8fSmTlW6v6SkCJuwe7boO0fDQ\",\n  \"object\": \"chat.completion\",\n  \"created\": 1720014428,\n  \"model\": \"gpt-4o-2024-05-13\",\n  \"choices\": [\n    {\n      \"index\": 0,\n      \"message\": {\n        \"role\": \"assistant\",\n        \"content\": \"{\\n  \\\"licenseClasses\\\": [\\\"D\\\"],\\n  \\\"licenseDateFrom\\\": \\\"01/11/2021\\\",\\n  \\\"licenseDateTo\\\": \\\"10/07/2027\\\"\\n, \\\"identityNo\\\": \\\"550106-12-5821\\\"\\n}\"\n      },\n      \"logprobs\": null,\n      \"finish_reason\": \"stop\"\n    }\n  ],\n  \"usage\": {\n    \"prompt_tokens\": 867,\n    \"completion_tokens\": 38,\n    \"total_tokens\": 905\n  },\n  \"system_fingerprint\": \"fp_4008e3b719\"\n}\n";
                    break;
                case UploadDocTypeEnum.LicenseBack:
                    result = "{\n  \"id\": \"chatcmpl-9gw7rHQCGzFY79IbnS09ucJknTyzS\",\n  \"object\": \"chat.completion\",\n  \"created\": 1720020239,\n  \"model\": \"gpt-4o-2024-05-13\",\n  \"choices\": [\n    {\n      \"index\": 0,\n      \"message\": {\n        \"role\": \"assistant\",\n        \"content\": \"{\\n  \\\"licenseNo\\\": \\\"41771307\\\"\\n}\"\n      },\n      \"logprobs\": null,\n      \"finish_reason\": \"stop\"\n    }\n  ],\n  \"usage\": {\n    \"prompt_tokens\": 843,\n    \"completion_tokens\": 12,\n    \"total_tokens\": 855\n  },\n  \"system_fingerprint\": \"fp_4008e3b719\"\n}\n";
                    break;
                case UploadDocTypeEnum.HospitalDetail:
                    result = "";
                    break;
                case UploadDocTypeEnum.EmploymentDetail:
                    result = "{\r\n  \"employerName\": \"University of Malaya\",\r\n  \"employerContact\": \"NA\",\r\n  \"employerAddress\": \"Department of Social and Preventive Medicine, Faculty of Medicine, University of Malaya, 50603 Kuala Lumpur, Malaysia\",\r\n  \"monthlyIncome\": \"RM2000\"\r\n}";
                    break;
                case UploadDocTypeEnum.CarGrant:
                    result = "{\n  \"id\": \"chatcmpl-9zCbtW6Q75oscViYNqyq8fSQp2kno\",\n  \"object\": \"chat.completion\",\n  \"created\": 1724373509,\n  \"model\": \"gpt-4o-2024-05-13\",\n  \"choices\": [\n    {\n      \"index\": 0,\n      \"message\": {\n        \"role\": \"assistant\",\n        \"content\": \"{\\n  \\\"make\\\": \\\"VESPA\\\",\\n  \\\"model\\\": \\\"SCOOTER\\\",\\n  \\\"specs\\\": \\\"150\\\",\\n  \\\"year\\\": \\\"1965\\\",\\n  \\\"jpjRegisterNo\\\": \\\"D4347\\\",\\n  \\\"jpjRegisterDate\\\": \\\"11/05/1965\\\"\\n}\",\n        \"refusal\": null\n      },\n      \"logprobs\": null,\n      \"finish_reason\": \"stop\"\n    }\n  ],\n  \"usage\": {\n    \"prompt_tokens\": 1205,\n    \"completion_tokens\": 63,\n    \"total_tokens\": 1268\n  },\n  \"system_fingerprint\": \"fp_c9aa9c0491\"\n}\n";
                    break;
                case UploadDocTypeEnum.PoliceReport:
                    result = "{\n  \u0022id\u0022: \u0022chatcmpl-A189UeWFO7EKwuf7FjL1eGhfjRLnX\u0022,\n  \u0022object\u0022: \u0022chat.completion\u0022,\n  \u0022created\u0022: 1724833028,\n  \u0022model\u0022: \u0022gpt-4o-2024-05-13\u0022,\n  \u0022choices\u0022: [\n    {\n      \u0022index\u0022: 0,\n      \u0022message\u0022: {\n        \u0022role\u0022: \u0022assistant\u0022,\n        \u0022content\u0022: \u0022{\\n  \\\u0022policeStation\\\u0022 : \\\u0022TRAFIK HULU SELANGOR\\\u0022,\\n  \\\u0022officerName\\\u0022 : \\\u0022MOHAMAD AFIQ FIKRI BIN MOHD NIZAM\\\u0022,\\n  \\\u0022officerServiceNo\\\u0022 : \\\u0022R216864\\\u0022,\\n  \\\u0022reportNo\\\u0022 : \\\u0022TRAFIK HULU SELANGOR/005040/23\\\u0022,\\n  \\\u0022reportDatetime\\\u0022 : \\\u002230/07/2023 19:05\\\u0022,\\n  \\\u0022incidentDatetime\\\u0022 : \\\u002230/07/2023 18:15\\\u0022,\\n  \\\u0022reporterName\\\u0022 : \\\u0022KHAIRUMAN BIN HAMIM\\\u0022,\\n  \\\u0022organizationName\\\u0022 : \\\u0022NA\\\u0022,\\n  \\\u0022incidentLocation\\\u0022 : \\\u0022JALAN MASJID FELDA SOEHARTO, KUALA KUBU BHARU\\\u0022,\\n  \\\u0022summaryOfIncidentReported\\\u0022 : \\\u0022On 30/07/2023 at around 18:15 hrs, I was driving my vehicle with registration number SYG 6703, a metallic gray Honda City, from Kuala Kubu Bharu towards Tanjung Malim via Kuala Lumpur - Ipoh road. Upon approaching the intersection at the said road, while making a right turn, my vehicle was hit from behind by another vehicle bearing registration number DMA 3550, a white Yamaha 135LC. As a result of the collision, my car sustained damage on the rear bumper and right rear lamp, while the other vehicle\u0027s right rear fender, bumper, and lamps were also damaged. The objective of this report is to provide a reference for insurance purposes.\\\u0022,\\n  \\\u0022natureOfIncidentReported\\\u0022 : \\\u0022Accident\\\u0022,\\n  \\\u0022vehicleRegistrationNumbers\\\u0022 : [\\\u0022SYG 6703\\\u0022, \\\u0022DMA 3550\\\u0022],\\n  \\\u0022vehicleModels\\\u0022 : [\\\u0022Honda City\\\u0022, \\\u0022Yamaha 135LC\\\u0022],\\n  \\\u0022vehicleColours\\\u0022 : [\\\u0022Metallic Gray\\\u0022, \\\u0022White\\\u0022],\\n  \\\u0022identityNo\\\u0022 : \\\u0022550106125821\\\u0022,\\n  \\\u0022armedForceId\\\u0022 : \\\u0022A0645678\\\u0022,\\n  \\\u0022passportNo\\\u0022 : \\\u0022NA\\\u0022\\n}\u0022,\n        \u0022refusal\u0022: null\n      },\n      \u0022logprobs\u0022: null,\n      \u0022finish_reason\u0022: \u0022stop\u0022\n    }\n  ],\n  \u0022usage\u0022: {\n    \u0022prompt_tokens\u0022: 988,\n    \u0022completion_tokens\u0022: 423,\n    \u0022total_tokens\u0022: 1411\n  },\n  \u0022system_fingerprint\u0022: \u0022fp_fde2829a40\u0022\n}\n";
                    break;
                default:
                    break;
            }
            return result;
        }

        public async Task<string> UploadDocument(UploadDocTypeEnum docType, string imageBase64String, string filename, string caseNo)
        {
            var result = "";
            switch (docType)
            {
                case UploadDocTypeEnum.ICFront:
                    result = "{\r\n  \"id\": \"chatcmpl-9gNXlSaw69BKxkfJZBbL68VOYze6A\",\r\n  \"object\": \"chat.completion\",\r\n  \"created\": 1719887305,\r\n  \"model\": \"gpt-4o-2024-05-13\",\r\n  \"choices\": [\r\n    {\r\n      \"index\": 0,\r\n      \"message\": {\r\n        \"role\": \"assistant\",\r\n        \"content\": \"{\\n  \\\"identityNumber\\\": \\\"550106-12-5821\\\",\\n  \\\"identityType\\\": \\\"IC\\\",\\n  \\\"fullName\\\": \\\"Rowan Sebastian Atkinson\\\",\\n  \\\"addressLine1\\\": \\\"GDW Kampung Bayangan\\\",\\n  \\\"postcode\\\": \\\"80000\\\",\\n  \\\"city\\\": \\\"Keningau\\\",\\n  \\\"state\\\": \\\"Sabah\\\",\\n  \\\"country\\\": \\\"NA\\\",\\n  \\\"nationality\\\": \\\"Malaysian\\\",\\n  \\\"gender\\\": \\\"Male\\\"\\n}\"\r\n      },\r\n      \"logprobs\": null,\r\n      \"finish_reason\": \"stop\"\r\n    }\r\n  ],\r\n  \"usage\": {\r\n    \"prompt_tokens\": 378,\r\n    \"completion_tokens\": 99,\r\n    \"total_tokens\": 477\r\n  },\r\n  \"system_fingerprint\": \"fp_4008e3b719\"\r\n}\r\n";
                    break;
                case UploadDocTypeEnum.LicenseFront:
                    result = "{\n  \"id\": \"chatcmpl-9guc8fSmTlW6v6SkCJuwe7boO0fDQ\",\n  \"object\": \"chat.completion\",\n  \"created\": 1720014428,\n  \"model\": \"gpt-4o-2024-05-13\",\n  \"choices\": [\n    {\n      \"index\": 0,\n      \"message\": {\n        \"role\": \"assistant\",\n        \"content\": \"{\\n  \\\"licenseClasses\\\": [\\\"D\\\"],\\n  \\\"licenseDateFrom\\\": \\\"01/11/2021\\\",\\n  \\\"licenseDateTo\\\": \\\"10/07/2027\\\"\\n, \\\"identityNo\\\": \\\"550106-12-5821\\\"\\n}\"\n      },\n      \"logprobs\": null,\n      \"finish_reason\": \"stop\"\n    }\n  ],\n  \"usage\": {\n    \"prompt_tokens\": 867,\n    \"completion_tokens\": 38,\n    \"total_tokens\": 905\n  },\n  \"system_fingerprint\": \"fp_4008e3b719\"\n}\n";
                    break;
                case UploadDocTypeEnum.LicenseBack:
                    result = "{\n  \"id\": \"chatcmpl-9gw7rHQCGzFY79IbnS09ucJknTyzS\",\n  \"object\": \"chat.completion\",\n  \"created\": 1720020239,\n  \"model\": \"gpt-4o-2024-05-13\",\n  \"choices\": [\n    {\n      \"index\": 0,\n      \"message\": {\n        \"role\": \"assistant\",\n        \"content\": \"{\\n  \\\"licenseNo\\\": \\\"41771307\\\"\\n}\"\n      },\n      \"logprobs\": null,\n      \"finish_reason\": \"stop\"\n    }\n  ],\n  \"usage\": {\n    \"prompt_tokens\": 843,\n    \"completion_tokens\": 12,\n    \"total_tokens\": 855\n  },\n  \"system_fingerprint\": \"fp_4008e3b719\"\n}\n";
                    break;
                case UploadDocTypeEnum.HospitalDetail:
                    result = "";
                    break;
                case UploadDocTypeEnum.EmploymentDetail:
                    result = "{\r\n  \"employerName\": \"University of Malaya\",\r\n  \"employerContact\": \"NA\",\r\n  \"employerAddress\": \"Department of Social and Preventive Medicine, Faculty of Medicine, University of Malaya, 50603 Kuala Lumpur, Malaysia\",\r\n  \"monthlyIncome\": \"RM2000\"\r\n}";
                    break;
                case UploadDocTypeEnum.CarGrant:
                    result = "{\n  \"id\": \"chatcmpl-9zCbtW6Q75oscViYNqyq8fSQp2kno\",\n  \"object\": \"chat.completion\",\n  \"created\": 1724373509,\n  \"model\": \"gpt-4o-2024-05-13\",\n  \"choices\": [\n    {\n      \"index\": 0,\n      \"message\": {\n        \"role\": \"assistant\",\n        \"content\": \"{\\n  \\\"make\\\": \\\"VESPA\\\",\\n  \\\"model\\\": \\\"SCOOTER\\\",\\n  \\\"specs\\\": \\\"150\\\",\\n  \\\"year\\\": \\\"1965\\\",\\n  \\\"jpjRegisterNo\\\": \\\"D4347\\\",\\n  \\\"jpjRegisterDate\\\": \\\"11/05/1965\\\"\\n}\",\n        \"refusal\": null\n      },\n      \"logprobs\": null,\n      \"finish_reason\": \"stop\"\n    }\n  ],\n  \"usage\": {\n    \"prompt_tokens\": 1205,\n    \"completion_tokens\": 63,\n    \"total_tokens\": 1268\n  },\n  \"system_fingerprint\": \"fp_c9aa9c0491\"\n}\n";
                    break;
                case UploadDocTypeEnum.PoliceReport:
                    result = "{\n  \u0022id\u0022: \u0022chatcmpl-A189UeWFO7EKwuf7FjL1eGhfjRLnX\u0022,\n  \u0022object\u0022: \u0022chat.completion\u0022,\n  \u0022created\u0022: 1724833028,\n  \u0022model\u0022: \u0022gpt-4o-2024-05-13\u0022,\n  \u0022choices\u0022: [\n    {\n      \u0022index\u0022: 0,\n      \u0022message\u0022: {\n        \u0022role\u0022: \u0022assistant\u0022,\n        \u0022content\u0022: \u0022{\\n  \\\u0022policeStation\\\u0022 : \\\u0022TRAFIK HULU SELANGOR\\\u0022,\\n  \\\u0022officerName\\\u0022 : \\\u0022MOHAMAD AFIQ FIKRI BIN MOHD NIZAM\\\u0022,\\n  \\\u0022officerServiceNo\\\u0022 : \\\u0022R216864\\\u0022,\\n  \\\u0022reportNo\\\u0022 : \\\u0022TRAFIK HULU SELANGOR/005040/23\\\u0022,\\n  \\\u0022reportDatetime\\\u0022 : \\\u002230/07/2023 19:05\\\u0022,\\n  \\\u0022incidentDatetime\\\u0022 : \\\u002230/07/2023 18:15\\\u0022,\\n  \\\u0022reporterName\\\u0022 : \\\u0022KHAIRUMAN BIN HAMIM\\\u0022,\\n  \\\u0022organizationName\\\u0022 : \\\u0022NA\\\u0022,\\n  \\\u0022incidentLocation\\\u0022 : \\\u0022JALAN MASJID FELDA SOEHARTO, KUALA KUBU BHARU\\\u0022,\\n  \\\u0022summaryOfIncidentReported\\\u0022 : \\\u0022On 30/07/2023 at around 18:15 hrs, I was driving my vehicle with registration number SYG 6703, a metallic gray Honda City, from Kuala Kubu Bharu towards Tanjung Malim via Kuala Lumpur - Ipoh road. Upon approaching the intersection at the said road, while making a right turn, my vehicle was hit from behind by another vehicle bearing registration number DMA 3550, a white Yamaha 135LC. As a result of the collision, my car sustained damage on the rear bumper and right rear lamp, while the other vehicle\u0027s right rear fender, bumper, and lamps were also damaged. The objective of this report is to provide a reference for insurance purposes.\\\u0022,\\n  \\\u0022natureOfIncidentReported\\\u0022 : \\\u0022Accident\\\u0022,\\n  \\\u0022vehicleRegistrationNumbers\\\u0022 : [\\\u0022SYG 6703\\\u0022, \\\u0022DMA 3550\\\u0022],\\n  \\\u0022vehicleModels\\\u0022 : [\\\u0022Honda City\\\u0022, \\\u0022Yamaha 135LC\\\u0022],\\n  \\\u0022vehicleColours\\\u0022 : [\\\u0022Metallic Gray\\\u0022, \\\u0022White\\\u0022],\\n  \\\u0022identityNo\\\u0022 : \\\u0022550106125821\\\u0022,\\n  \\\u0022armedForceId\\\u0022 : \\\u0022A0645678\\\u0022,\\n  \\\u0022passportNo\\\u0022 : \\\u0022NA\\\u0022\\n}\u0022,\n        \u0022refusal\u0022: null\n      },\n      \u0022logprobs\u0022: null,\n      \u0022finish_reason\u0022: \u0022stop\u0022\n    }\n  ],\n  \u0022usage\u0022: {\n    \u0022prompt_tokens\u0022: 988,\n    \u0022completion_tokens\u0022: 423,\n    \u0022total_tokens\u0022: 1411\n  },\n  \u0022system_fingerprint\u0022: \u0022fp_fde2829a40\u0022\n}\n";
                    break;
                default:
                    break;
            }
            return result;
        }

        public async Task<string> GenerateSummary(string inputData, string promptName, string caseNo)
        {
            var result = "";
            switch (promptName)
            {
                case "GeneratePoliceReportSummary":
                    result = "{\r\n  \"summary\": \"On 30/07/2023, at approximately 18:15 hrs, a road accident occurred on Jalan Kuala Lumpur - Ipoh involving a Honda City with registration number SYG 6703, driven by a Malay man, and a motorcycle with registration number BKM 3508, a Yamaha 135LC. The incident happened when the Honda City was overtaking another vehicle, during which a collision occurred with the motorcycle that was following behind. The motorcyclist fell onto the road and sustained injuries to the left hand and bruises on the left knee. The Honda City suffered damage to its rear bumper and rear right side, while the motorcycle was damaged with a broken cover set and engine damage. The accident was reported for insurance purposes.\",\r\n\r\n  \"discrepancies\": \"There are several discrepancies between the statements provided: /n 1. There is inconsistency in the registration number of the motorcycle involved in the accident. The third-party statement mentions a motorcycle with registration number BKM 3508, while the police report states it as BKM 3950. /n 2. There is a conflict regarding the vehicles involved. The claimant mentions a collision with a Toyota Camry (JDM 8500), which was not mentioned in either the third-party or police statements. /n 3. The location of the accident is inconsistently reported. The claimant describes the location as 'an unclear location' on Jalan Kuala Lumpur - Ipoh, the third-party specifies 'KM 57 Jalan KL - Ipoh,' and the police report mentions 'an unknown KM of Jalan Kuala Lumpur - Ipoh.'\"\r\n}\r\n";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
