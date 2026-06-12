using Abp.IO.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class InsuredPersonsController : ThinknInsurTechControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private readonly IOCRPromptService _OCRPromptService;

        private readonly IConfigurationRoot _appConfiguration;

        private const long MaxDriverICFrontLength = 5242880; //5MB
        private const string MaxDriverICFrontLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] DriverICFrontAllowedFileTypes = [];
        //            { "jpeg", "jpg", "png" };

        private const long MaxDriverICBackLength = 5242880; //5MB
        private const string MaxDriverICBackLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] DriverICBackAllowedFileTypes = [];
        //{ "jpeg", "jpg", "png" };

        private const long MaxDriverLicenseFrontLength = 5242880; //5MB
        private const string MaxDriverLicenseFrontLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] DriverLicenseFrontAllowedFileTypes = [];
        //{ "jpeg", "jpg", "png" };

        private const long MaxDriverLicenseBackLength = 5242880; //5MB
        private const string MaxDriverLicenseBackLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] DriverLicenseBackAllowedFileTypes = [];
        //{ "jpeg", "jpg", "png" };

        private const long MaxDriverEmploymentDetailLength = 5242880; //5MB
        private const string MaxDriverEmploymentDetailLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] DriverEmploymentDetailAllowedFileTypes = [];
        //{ "jpeg", "jpg", "png" };

        private const long MaxDriverHospitalDetailLength = 5242880; //5MB
        private const string MaxDriverHospitalDetailLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] DriverHospitalDetailAllowedFileTypes = [];
        //{ "jpeg", "jpg", "png" };

        private const long MaxDriverCarGrantLength = 5242880; //5MB
        private const string MaxDriverCarGrantLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] DriverCarGrantAllowedFileTypes = [];
        //{ "jpeg", "jpg", "png" };

        public InsuredPersonsController(ITempFileCacheManager tempFileCacheManager, IOCRPromptService ocrPromptService, IAppConfigurationAccessor appConfigurationAccessor)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _OCRPromptService = ocrPromptService;
            _appConfiguration = appConfigurationAccessor.Configuration;
            DriverICFrontAllowedFileTypes = _appConfiguration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();
            DriverICBackAllowedFileTypes = _appConfiguration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();
            DriverLicenseFrontAllowedFileTypes = _appConfiguration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();
            DriverLicenseBackAllowedFileTypes = _appConfiguration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();
            DriverEmploymentDetailAllowedFileTypes = _appConfiguration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();
            DriverHospitalDetailAllowedFileTypes = _appConfiguration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();
            DriverCarGrantAllowedFileTypes = _appConfiguration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();
        }

        // Changed file size value to 2MB for OCR

        public async Task<JsonResult> UploadDriverICFrontFile()
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

            var output = await _OCRPromptService.UploadDocument(UploadDocTypeEnum.ICFront, imageBase64String, file.FileName, caseNo);

            return Json(new FileOCROutput()
            {
                fileToken = fileToken,
                output = output
            });
        }

        public JsonResult UploadDriverICBackFile()
        {
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

            return Json(new FileOCROutput()
            {
                fileToken = fileToken,
                output = null
            });
        }

        public async Task<JsonResult> UploadDriverLicenseFrontFile()
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

            var output = await _OCRPromptService.UploadDocument(UploadDocTypeEnum.LicenseFront, imageBase64String, file.FileName, caseNo);

            return Json(new FileOCROutput()
            {
                fileToken = fileToken,
                output = output
            });
        }

        public async Task<JsonResult> UploadDriverLicenseBackFile()
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

            var output = await _OCRPromptService.UploadDocument(UploadDocTypeEnum.LicenseBack, imageBase64String, file.FileName, caseNo);

            return Json(new FileOCROutput()
            {
                fileToken = fileToken,
                output = output
            });
        }

        public async Task<JsonResult> UploadDriverEmploymentDetailFile()
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

            var output = await _OCRPromptService.UploadDocument(UploadDocTypeEnum.EmploymentDetail, imageBase64String, file.FileName, caseNo);

            return Json(new FileOCROutput()
            {
                fileToken = fileToken,
                output = output
            });
        }

        public async Task<JsonResult> UploadDriverHospitalDetailFile()
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

            //var output = await _OCRPromptService.UploadDocument(UploadDocTypeEnum.HospitalDetail, imageBase64String, file.FileName, caseNo);

            return Json(new FileOCROutput()
            {
                fileToken = fileToken,
                output = null
            });
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

        public string[] GetDriverICFrontFileAllowedTypes()
        {
            return DriverICFrontAllowedFileTypes;
        }

        public string[] GetDriverICBackFileAllowedTypes()
        {
            return DriverICBackAllowedFileTypes;
        }

        public string[] GetDriverLicenseFrontFileAllowedTypes()
        {
            return DriverLicenseFrontAllowedFileTypes;
        }

        public string[] GetDriverLicenseBackFileAllowedTypes()
        {
            return DriverLicenseBackAllowedFileTypes;
        }

        public string[] GetDriverEmploymentDetailFileAllowedTypes()
        {
            return DriverEmploymentDetailAllowedFileTypes;
        }

        public string[] GetDriverHospitalDetailFileAllowedTypes()
        {
            return DriverHospitalDetailAllowedFileTypes;
        }

        public string[] GetDriverCarGrantFileAllowedTypes()
        {
            return DriverCarGrantAllowedFileTypes;
        }

    }
}