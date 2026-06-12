using Abp.Auditing;
using Abp.Extensions;
using Abp.MimeTypes;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.Configuration;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Web.Controllers
{
    public class FileController : ThinknInsurTechControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IFileOrgManager _fileOrgManager;
        private readonly IFolderManager _folderManager;
        private readonly IHostEnvironment _env;
        private readonly IMimeTypeMap _mimeTypeMap;
        private static IConfigurationRoot _configuration;

        public FileController(
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager,
            IFileOrgManager fileOrgManager,
            IFolderManager folderManager,
            IHostEnvironment env,
            IMimeTypeMap mimeTypeMap,
           IAppConfigurationAccessor appConfigurationAccessor
        )
        {
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _fileOrgManager = fileOrgManager;
            _folderManager = folderManager;
            _env = env;
            _mimeTypeMap = mimeTypeMap;
            _configuration = appConfigurationAccessor.Configuration;
        }

        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            if (fileBytes == null)
            {
                return NotFound(L("RequestedFileDoesNotExists"));
            }

            return File(fileBytes, file.FileType, file.FileName);
        }

        [DisableAuditing]
        public async Task<ActionResult> DownloadBinaryFile(Guid id, string contentType, string fileName)
        {
            var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
            if (fileObject == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            if (fileName.IsNullOrEmpty())
            {
                if (!fileObject.Description.IsNullOrEmpty() &&
                    !Path.GetExtension(fileObject.Description).IsNullOrEmpty())
                {
                    fileName = fileObject.Description;
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }

            if (contentType.IsNullOrEmpty())
            {
                if (!Path.GetExtension(fileName).IsNullOrEmpty())
                {
                    contentType = _mimeTypeMap.GetMimeType(fileName);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }

            return File(fileObject.Bytes, contentType, fileName);
        }

        [DisableAuditing]
        public async Task<ActionResult> DownloadBinaryFileFromFileOrg(Guid id, string contentType, string fileName)
        {
            var fileObject = await _fileOrgManager.GetOrNullAsyncByReferenceNo(id);
            if (fileObject == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            var path = "";
            var folderId = (int)fileObject.FolderId;
            var caseNo = fileObject.MainRegistrationId.ToString();

            var folder = await _folderManager.GetOrNullAsync(folderId);
            var rootFolder = _configuration.GetSection("Folder")["root"];
            Console.WriteLine(rootFolder);
            var root = Path.Combine(Directory.GetDirectoryRoot(_env.ContentRootPath), _configuration.GetSection("Folder")["root"]);

            path = Path.Combine(root, AbpSession.TenantId.ToString(),
                caseNo,
                folder.MainEntity,
                folder.Field);

            var filePath = Path.Combine(path, fileObject.FileName);

            if (!System.IO.File.Exists(filePath))
            {
                throw new UserFriendlyException("File not found on disk.");
            }

            var fileContent = await System.IO.File.ReadAllBytesAsync(filePath);

            if (fileName.IsNullOrEmpty())
            {
                if (!fileObject.FileName.IsNullOrEmpty())
                {
                    fileName = fileObject.FileName;
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }

            if (contentType.IsNullOrEmpty())
            {
                if (!Path.GetExtension(fileName).IsNullOrEmpty())
                {
                    contentType = _mimeTypeMap.GetMimeType(fileName);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }

            return File(fileContent, contentType, fileName);
        }
    }
}
