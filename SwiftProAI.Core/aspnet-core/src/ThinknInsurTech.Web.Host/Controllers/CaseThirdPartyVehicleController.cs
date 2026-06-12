using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
    public class CaseThirdPartyVehicleController : ThinknInsurTechControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IOCRPromptService _OCRPromptService;
        private readonly IConfigurationRoot _appConfiguration;

        private const long MaxTpvFileUploadLength = 5242880; //5MB
        private const string MaxTpvFileUploadLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] TpvFileUploadAllowedFileTypes = [];
        //{ "jpeg", "jpg", "png" };

        private const long MaxDriverCarGrantLength = 5242880; //5MB
        private const string MaxDriverCarGrantLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] DriverCarGrantAllowedFileTypes = { "jpeg", "jpg", "png", "pdf" };

        public CaseThirdPartyVehicleController(ITempFileCacheManager tempFileCacheManager, IOCRPromptService ocrPromptService, IAppConfigurationAccessor appConfigurationAccessor)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _OCRPromptService = ocrPromptService;
            _appConfiguration = appConfigurationAccessor.Configuration;
            TpvFileUploadAllowedFileTypes = _appConfiguration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();
            DriverCarGrantAllowedFileTypes = _appConfiguration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();

        }


        public List<FileUploadCacheOutput> TpvFileUpload()
        {
            var output = new List<FileUploadCacheOutput>();

            try
            {
                // Check input
                if (Request.Form.Files.Count == 0)
                {
                    throw new UserFriendlyException(L("NoFileFoundError"));
                }

                foreach (var file in Request.Form.Files)
                {
                    if (file.Length > MaxTpvFileUploadLength)
                    {
                        output.Add(new FileUploadCacheOutput(new ErrorInfo(L("Warn_File_SizeLimit", MaxTpvFileUploadLengthUserFriendlyValue))));
                        continue;
                    }

                    var fileType = Path.GetExtension(file.FileName).Substring(1);
                    if (TpvFileUploadAllowedFileTypes != null && TpvFileUploadAllowedFileTypes.Length > 0 && !TpvFileUploadAllowedFileTypes.Contains(fileType))
                    {
                        output.Add(new FileUploadCacheOutput(new ErrorInfo(L("FileNotInAllowedFileTypes", TpvFileUploadAllowedFileTypes))));
                        continue;
                    }

                    byte[] fileBytes;
                    using (var stream = file.OpenReadStream())
                    {
                        fileBytes = stream.GetAllBytes();
                    }

                    var fileToken = Guid.NewGuid().ToString("N");
                    _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

                    output.Add(new FileUploadCacheOutput(fileToken, file.FileName, null));
                }
            }
            catch (UserFriendlyException ex)
            {
                output.Add(new FileUploadCacheOutput(new ErrorInfo(ex.Message)));
            }

            return output;
        }

        public async Task<JsonResult> UploadDriverCarGrantFile()
        {
            var caseNo = HttpUtility.ParseQueryString(Request.QueryString.Value)["caseId"];
            var file = Request.Form.Files.First();
            var fileType = Path.GetExtension(file.FileName).Substring(1);

            ValidateFile(file, fileType);

            byte[] fileBytes;
            var imageBase64String = "";

            using (var stream = file.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
                imageBase64String = Convert.ToBase64String(fileBytes);
            }

            var fileToken = Guid.NewGuid().ToString("N");
            _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

            var output = await _OCRPromptService.UploadDocument(UploadDocTypeEnum.CarGrant, imageBase64String, file.FileName, caseNo);

            return Json(new FileOCROutput()
            {
                fileToken = fileToken,
                output = output
            });
        }
        private void ValidateFile(IFormFile file, string fileType)
        {
            //Check input
            if (Request.Form.Files.Count == 0)
            {
                throw new UserFriendlyException(L("NoFileFoundError"));
            }

            var maxFileSize = Convert.ToInt32(_appConfiguration.GetSection("OCR")["MaxFileSize"]);
            var maxFileSizeFriendlyValue = _appConfiguration.GetSection("OCR")["FriendlyValue"];
            var allowedFileTypes = _appConfiguration.GetSection("FileUpload")["AllowedFileTypes"];

            if (file.Length > maxFileSize)
            {
                throw new UserFriendlyException(L("Warn_File_SizeLimit", maxFileSizeFriendlyValue));
            }

            if (allowedFileTypes != null && allowedFileTypes.Length > 0 && !allowedFileTypes.Contains(fileType))
            {
                throw new UserFriendlyException(L("FileNotInAllowedFileTypes", allowedFileTypes));
            }
        }


        public string[] GetTpvFileAllowedType()
        {
            return TpvFileUploadAllowedFileTypes;
        }
        public string[] GetDriverCarGrantFileAllowedTypes()
        {
            return DriverCarGrantAllowedFileTypes;
        }

    }
}
