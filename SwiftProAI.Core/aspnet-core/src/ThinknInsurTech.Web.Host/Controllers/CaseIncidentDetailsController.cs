using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using ThinknInsurTech.Configuration;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Web.Controllers
{
    [Authorize]
    public class CaseIncidentDetailsController : ThinknInsurTechControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private static IConfigurationRoot _configuration;

        private const long MaxCircumstancesFileUploadLength = 5242880; //5MB
        private const string MaxCircumstancesFileUploadLengthUserFriendlyValue = "5MB"; //5MB
        private string[] CircumstancesFileUploadAllowedFileTypes = [];
        //{ "jpeg", "jpg", "png" };

        public CaseIncidentDetailsController(ITempFileCacheManager tempFileCacheManager, IAppConfigurationAccessor appConfigurationAccessor)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _configuration = appConfigurationAccessor.Configuration;
            CircumstancesFileUploadAllowedFileTypes = _configuration.GetSection("FileUpload:AllowedFileTypes").Get<string[]>();
        }

        public FileUploadCacheOutput UploadCircumstancesFileUploadFile()
        {
            try
            {
                //Check input
                if (Request.Form.Files.Count == 0)
                {
                    throw new UserFriendlyException(L("NoFileFoundError"));
                }

                var file = Request.Form.Files.First();
                if (file.Length > MaxCircumstancesFileUploadLength)
                {
                    throw new UserFriendlyException(L("Warn_File_SizeLimit", MaxCircumstancesFileUploadLengthUserFriendlyValue));
                }

                var fileType = Path.GetExtension(file.FileName).Substring(1);
                if (CircumstancesFileUploadAllowedFileTypes != null && CircumstancesFileUploadAllowedFileTypes.Length > 0 && !CircumstancesFileUploadAllowedFileTypes.Contains(fileType))
                {
                    throw new UserFriendlyException(L("FileNotInAllowedFileTypes", CircumstancesFileUploadAllowedFileTypes));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var fileToken = Guid.NewGuid().ToString("N");
                _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

                return new FileUploadCacheOutput(fileToken);
            }
            catch (UserFriendlyException ex)
            {
                return new FileUploadCacheOutput(new ErrorInfo(ex.Message));
            }
        }

        public string[] GetCircumstancesFileUploadFileAllowedTypes()
        {
            return CircumstancesFileUploadAllowedFileTypes;
        }

    }
}