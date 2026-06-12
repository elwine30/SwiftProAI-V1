using Abp.IO.Extensions;
using Abp.UI;
using GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PdfiumViewer;
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
    public class FileOcrController : ThinknInsurTechControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private readonly IOCRPromptService _OCRPromptService;

        private readonly IConfigurationRoot _appConfiguration;

        private long FileLength = 10485760; //5MB
        private string LengthUserFriendlyValue = "10MB"; //5MB
        private string[] AllowedFileTypes = ["jpeg", "jpg", "png", "pdf"];
        public FileOcrController(ITempFileCacheManager tempFileCacheManager, IOCRPromptService ocrPromptService, IAppConfigurationAccessor appConfigurationAccessor)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _OCRPromptService = ocrPromptService;
            _appConfiguration = appConfigurationAccessor.Configuration;
            //!Once confirmed can add pdf into appsettings and uncomment below
            AllowedFileTypes = _appConfiguration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();
            LengthUserFriendlyValue = _appConfiguration.GetSection("FileUpload:FriendlyValue").Get<string>();
            FileLength = _appConfiguration.GetSection("FileUpload:MaxFileSize").Get<long>();

        }

        public string[] GetFileAllowedTypes()
        {
            return AllowedFileTypes;
        }
        public async Task<JsonResult> UploadFile()
        {
            var caseNo = HttpUtility.ParseQueryString(Request.QueryString.Value)["caseId"];
            //newly added
            var docTypeValue = HttpUtility.ParseQueryString(Request.QueryString.Value)["docTypeValue"];

            UploadDocTypeEnum docTypeEnum = UploadDocTypeEnum.None;
            if (!string.IsNullOrEmpty(docTypeValue))
            {
                if (Enum.TryParse(docTypeValue, out UploadDocTypeEnum parsedEnum))
                {
                    docTypeEnum = parsedEnum;
                }
            }
            else
            {
                throw new UserFriendlyException("DocType not given");
            }

            //! This line only takes first file
            var file = Request.Form.Files.First();
            var fileType = Path.GetExtension(file.FileName).Substring(1);

            ValidateFile(file, fileType);

            byte[] fileBytes;
            var fileBase64String = "";
            string[] fileBase64StringList = [];


            using (var stream = file.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
                fileBase64String = Convert.ToBase64String(fileBytes);
                // Default if its image
                fileBase64StringList = new string[] { fileBase64String };
            }
            // If Pdf convert all into image(s)
            if (fileType == "pdf" && docTypeEnum != UploadDocTypeEnum.None && docTypeEnum != UploadDocTypeEnum.HospitalDetail)
            {
                fileBase64StringList = await ConvertPdfToImage(file);
            }
            var fileToken = Guid.NewGuid().ToString("N");
            _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

            var output = String.Empty;
            if (docTypeEnum != UploadDocTypeEnum.None && docTypeEnum != UploadDocTypeEnum.HospitalDetail)
            {
                output = await _OCRPromptService.UploadBulkDocument(docTypeEnum, fileBase64StringList, file.FileName, caseNo);
            }
            return Json(new FileOCROutput()
            {
                fileToken = fileToken,
                output = output
            });
        }

        public async Task<JsonResult> UploadFiles()
        {
            var caseNo = HttpUtility.ParseQueryString(Request.QueryString.Value)["caseId"];
            // newly added
            var docTypeValue = HttpUtility.ParseQueryString(Request.QueryString.Value)["docTypeValue"];

            UploadDocTypeEnum docTypeEnum = UploadDocTypeEnum.None;
            if (!string.IsNullOrEmpty(docTypeValue))
            {
                if (Enum.TryParse(docTypeValue, out UploadDocTypeEnum parsedEnum))
                {
                    docTypeEnum = parsedEnum;
                }
            }
            else
            {
                throw new UserFriendlyException("DocType not given");
            }

            var fileResults = new List<FileOCROutput>();

            foreach (var file in Request.Form.Files)
            {
                var fileType = Path.GetExtension(file.FileName).Substring(1);

                ValidateFile(file, fileType);

                byte[] fileBytes;
                var fileBase64String = "";
                string[] fileBase64StringList = [];

                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                    fileBase64String = Convert.ToBase64String(fileBytes);
                    fileBase64StringList = new string[] { fileBase64String };
                }

                //if (fileType == "pdf" && docTypeEnum != UploadDocTypeEnum.None && docTypeEnum != UploadDocTypeEnum.HospitalDetail)
                //{
                //    fileBase64StringList = await ConvertPdfToImage(file);
                //}

                var fileToken = Guid.NewGuid().ToString("N");
                _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

                var output = string.Empty;
                //if (docTypeEnum != UploadDocTypeEnum.None && docTypeEnum != UploadDocTypeEnum.HospitalDetail)
                //{
                //    output = await _OCRPromptService.UploadBulkDocument(docTypeEnum, fileBase64StringList, file.FileName, caseNo);
                //}

                string fileName = file.FileName;
                if (fileName.Length <= 23)
                {
                    fileName = fileName;
                }
                else
                {
                    fileName = $"{fileName.Substring(0, 10)}...{fileName.Substring(fileName.Length - 10)}";
                }

                fileResults.Add(new FileOCROutput()
                {
                    fileToken = fileToken,
                    output = output,
                    fileName = fileName
                });
            }

            return Json(fileResults);
        }

        private async Task<string[]> ConvertPdfToImage(IFormFile file)
        {
            var base64Strings = new List<string>();

            using (var pdfDocument = PdfDocument.Load(file.OpenReadStream()))
            {
                for (int i = 0; i < pdfDocument.PageCount; i++)
                {
                    using (var image = pdfDocument.Render(i, 300, 300, PdfRenderFlags.Annotations))
                    {
                        using (var ms = new MemoryStream())
                        {

                            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                            var base64String = Convert.ToBase64String(ms.ToArray());
                            base64Strings.Add(base64String);
                        }
                    }
                }
            }
            return base64Strings.ToArray();
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
            var allowedFileTypes = AllowedFileTypes;

            if (file.Length > maxFileSize)
            {
                throw new UserFriendlyException(L("Warn_File_SizeLimit", maxFileSizeFriendlyValue));
            }

            if (allowedFileTypes != null && allowedFileTypes.Length > 0 && !allowedFileTypes.Contains(fileType))
            {
                throw new UserFriendlyException(L("FileNotInAllowedFileTypes", allowedFileTypes));
            }
        }
    }
}
