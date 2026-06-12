using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ThinknInsurTech.Configuration;
using ThinknInsurTech.Integration;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Web.Controllers
{
    [Authorize]
    public class CaseThirdPartyInfoController : ThinknInsurTechControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IOCRPromptService _OCRPromptService;
        private static IConfigurationRoot _configuration;


        private const long MaxTpiFileUploadLength = 5242880; //5MB
        private const string MaxTpiFileUploadLengthUserFriendlyValue = "5MB"; //5MB
        private string[] TpiFileUploadAllowedFileTypes = [];
        //{ "jpeg", "jpg", "png" };

        public CaseThirdPartyInfoController(ITempFileCacheManager tempFileCacheManager, IOCRPromptService ocrPromptService, IAppConfigurationAccessor appConfigurationAccessor)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _OCRPromptService = ocrPromptService;
            _configuration = appConfigurationAccessor.Configuration;
            TpiFileUploadAllowedFileTypes = _configuration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();

        }

        public async Task<FileUploadCacheOutput> TpiFileUpload()
        {
            try
            {
                //Check input
                if (Request.Form.Files.Count == 0)
                {
                    throw new UserFriendlyException(L("NoFileFoundError"));
                }

                var file = Request.Form.Files.First();
                if (file.Length > MaxTpiFileUploadLength)
                {
                    throw new UserFriendlyException(L("Warn_File_SizeLimit", MaxTpiFileUploadLengthUserFriendlyValue));
                }

                var fileType = Path.GetExtension(file.FileName).Substring(1);
                if (TpiFileUploadAllowedFileTypes != null && TpiFileUploadAllowedFileTypes.Length > 0 && !TpiFileUploadAllowedFileTypes.Contains(fileType))
                {
                    throw new UserFriendlyException(L("FileNotInAllowedFileTypes", TpiFileUploadAllowedFileTypes));
                }

                var caseNo = HttpUtility.ParseQueryString(Request.QueryString.Value)["caseId"];
                var fileUploadType = HttpUtility.ParseQueryString(Request.QueryString.Value)["fileUploadType"];

                byte[] fileBytes;
                var imageBase64String = "";
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                    imageBase64String = Convert.ToBase64String(fileBytes);
                }

                var fileToken = Guid.NewGuid().ToString("N");
                _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

                UploadDocTypeEnum UploadType = UploadDocTypeEnum.None;
                switch (fileUploadType)
                {
                    case "ICFront":
                        UploadType = UploadDocTypeEnum.ICFront;
                        break;
                    case "LicenseFront":
                        UploadType = UploadDocTypeEnum.LicenseFront;
                        break;
                    case "LicenseBack":
                        UploadType = UploadDocTypeEnum.LicenseBack;
                        break;
                    //case "HospitalDetail":
                    //    UploadType = UploadDocTypeEnum.HospitalDetail;
                    //    break;
                    case "EmploymentDetail":
                        UploadType = UploadDocTypeEnum.EmploymentDetail;
                        break;
                    case "PoliceReport":
                        UploadType = UploadDocTypeEnum.PoliceReport;
                        break;
                    default:
                        UploadType = UploadDocTypeEnum.None;
                        break;
                }

                var output = String.Empty;
                if (UploadType != UploadDocTypeEnum.None)
                {
                    output = await _OCRPromptService.UploadDocument(UploadType, imageBase64String, file.FileName, caseNo);
                }

                return new FileUploadCacheOutput(fileToken, file.FileName, output);
            }
            catch (UserFriendlyException ex)
            {
                return new FileUploadCacheOutput(new ErrorInfo(ex.Message));
            }
        }

        public string[] GetTpiFileAllowedType()
        {
            return TpiFileUploadAllowedFileTypes;
        }

    }
}