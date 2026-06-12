using Abp.IO.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ThinknInsurTech.Configuration;
using ThinknInsurTech.Integration;
using ThinknInsurTech.Integration.Dto;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Web.Controllers
{
    [Authorize]
    public class CasePoliceReportsController : ThinknInsurTechControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private readonly IOCRPromptService _OCRPromptService;

        private readonly IConfigurationRoot _appConfiguration;

        private const long MaxReportFileUploadLength = 5242880; //5MB
        private const string MaxReportFileUploadLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] ReportFileUploadAllowedFileTypes = [];
        //{ "jpeg", "jpg", "png" };

        public CasePoliceReportsController(ITempFileCacheManager tempFileCacheManager, IOCRPromptService ocrPromptService, IAppConfigurationAccessor appConfigurationAccessor)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _OCRPromptService = ocrPromptService;
            _appConfiguration = appConfigurationAccessor.Configuration;
            ReportFileUploadAllowedFileTypes = _appConfiguration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();
        }

        public async Task<JsonResult> UploadReportFileUploadFile()
        {
            var caseNo = HttpUtility.ParseQueryString(Request.QueryString.Value)["caseId"];

            //Check input
            if (Request.Form.Files.Count == 0)
            {
                throw new UserFriendlyException(L("NoFileFoundError"));
            }

            var maxFileSize = Convert.ToInt32(_appConfiguration.GetSection("OCR")["MaxFileSize"]);
            var maxFileSizeFriendlyValue = _appConfiguration.GetSection("OCR")["FriendlyValue"];
            var allowedFileTypes = _appConfiguration.GetSection("FileUpload")["AllowedFileTypes"];

            var file = Request.Form.Files.First();
            if (file.Length > MaxReportFileUploadLength)
            {
                throw new UserFriendlyException(L("Warn_File_SizeLimit", maxFileSizeFriendlyValue));
            }

            var fileType = Path.GetExtension(file.FileName).Substring(1);
            if (allowedFileTypes != null && allowedFileTypes.Length > 0 && !allowedFileTypes.Contains(fileType))
            {
                throw new UserFriendlyException(L("FileNotInAllowedFileTypes", allowedFileTypes));
            }

            byte[] fileBytes;
            var imageBase64String = "";

            using (var stream = file.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
                imageBase64String = Convert.ToBase64String(fileBytes);
            }

            var fileToken = Guid.NewGuid().ToString("N");
            _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

            var output = await _OCRPromptService.UploadDocument(UploadDocTypeEnum.PoliceReport, imageBase64String, file.FileName, caseNo);

            return Json(new FileOCROutput()
            {
                fileToken = fileToken,
                output = output
            });
        }

        public string[] GetReportFileUploadFileAllowedTypes()
        {
            return ReportFileUploadAllowedFileTypes;
        }

    }
}