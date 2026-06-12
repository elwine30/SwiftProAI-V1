using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThinknInsurTech.Common;
using ThinknInsurTech.Common.Dto;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Runtime;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Integration
{
    public class FileOrgService : IFileOrgService, ITransientDependency
    {
        private readonly IRepository<Folder, int> _folderRepository;
        private readonly IRepository<FileOrg> _fileOrgRepository;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IAbpSession _abpSession;
        private readonly IObjectMapper _objectMapper;
        private readonly IFolderService _folderService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        public FileOrgService(IRepository<Folder, int> folderRepository, IRepository<FileOrg> fileOrgRepository, ITempFileCacheManager tempFileCacheManager, IObjectMapper objectMapper, IAbpSession abpSession, IFolderService folderService, IUnitOfWorkManager unitOfWorkManager)
        {
            _folderRepository = folderRepository;
            _fileOrgRepository = fileOrgRepository;

            _tempFileCacheManager = tempFileCacheManager;

            _objectMapper = objectMapper;
            _abpSession = abpSession;
            _unitOfWorkManager = unitOfWorkManager;
            _folderService = folderService;
        }

        /// <summary>
        /// Retrieves a file from cache using the given token, saves it to a directory, and records its details in the database.
        /// Returns a unique ID for the stored file.
        ///
        /// Steps:
        /// 1. Checks validity of token and retrieve file
        /// 2. Creates the directory (if not exist)
        /// 3. Saves the file to the directory.
        /// 4. Records the file details in the database.
        /// 5.  Returns the unique GUID of the file.
        /// </summary>
        public virtual async Task<Guid?> GetBinaryObjectFromCache(string fileToken, int registerId, string folderField)
        {

            // Get the binary object stored in Cache via fileToken

            if (fileToken.IsNullOrWhiteSpace())
            {
                return null;
            }

            var fileCache = _tempFileCacheManager.GetFileInfo(fileToken);

            if (fileCache == null)
            {
                throw new UserFriendlyException("There is no such file with the token: " + fileToken);
            }

            // Store the binary object retrieved in the directory 

            var uploadFolder = await _folderService.GenerateDirectory(folderField, registerId.ToString());

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            Folder folder;
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                folder = await _folderRepository.FirstOrDefaultAsync(w => w.Field.Equals(folderField));
            }

            if (folder == null)
            {
                throw new UserFriendlyException($"Error: Folder with field {folderField} is not found in Folder table.");
            }

            var filePath = Path.Combine(uploadFolder, fileCache.FileName);

            await File.WriteAllBytesAsync(filePath, fileCache.File);

            var fileOrgOb = new FileOrg();
            fileOrgOb.ReferenceNo = Guid.NewGuid();
            fileOrgOb.FileName = fileCache.FileName;
            fileOrgOb.MainRegistrationId = registerId;
            fileOrgOb.FolderId = folder.Id;
            fileOrgOb.TenantId = _abpSession.TenantId;
            if (_abpSession.GetCurrentOUId() != null)
            {
                fileOrgOb.OrganizationUnitId = _abpSession.GetCurrentOUId().Value;
            }

            await _fileOrgRepository.InsertAsync(_objectMapper.Map<FileOrg>(fileOrgOb));

            return fileOrgOb.ReferenceNo;

        }

        public virtual async Task<Guid?> GetBinaryObjectFromCacheToId(string fileToken, int registerId, int folderId)
        {
            // Get the binary object stored in Cache via fileToken

            if (fileToken.IsNullOrWhiteSpace())
            {
                return null;
            }

            var fileCache = _tempFileCacheManager.GetFileInfo(fileToken);

            if (fileCache == null)
            {
                throw new UserFriendlyException("There is no such file with the token: " + fileToken);
            }

            // Store the binary object retrieved in the directory 

            var uploadFolder = await _folderService.GenerateDirectoryByFolderId(folderId, registerId.ToString());

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            var filePath = Path.Combine(uploadFolder, fileCache.FileName);

            await File.WriteAllBytesAsync(filePath, fileCache.File);

            var fileOrgOb = new FileOrg();
            fileOrgOb.ReferenceNo = Guid.NewGuid();
            fileOrgOb.FileName = fileCache.FileName;
            fileOrgOb.MainRegistrationId = registerId;
            fileOrgOb.FolderId = folderId;
            fileOrgOb.TenantId = _abpSession.TenantId;
            if (_abpSession.GetCurrentOUId() != null)
            {
                fileOrgOb.OrganizationUnitId = _abpSession.GetCurrentOUId().Value;
            }

            await _fileOrgRepository.InsertAsync(_objectMapper.Map<FileOrg>(fileOrgOb));

            return fileOrgOb.ReferenceNo;
        }

        public FileMetadataDto GetMetadataByReference(Guid fileId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var metadata = _fileOrgRepository.GetAll()
                .Where(f => f.ReferenceNo == fileId)
                .Select(f => new FileMetadataDto
                {
                    Id = f.Id,
                    ReferenceNo = f.ReferenceNo,
                    FileName = f.FileName,
                })
                .FirstOrDefault();
                metadata.FileName = metadata.FileName.Length <= 23 ? metadata.FileName : $"{metadata.FileName.Substring(0, 10)}...{metadata.FileName.Substring(metadata.FileName.Length - 10)}";
                return metadata;
            }
        }


        public List<FileMetadataDto> GetMetadataByFolderIdAndCaseNo(int folderId, int caseNo)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var metadataList = _fileOrgRepository.GetAll()
                .Where(f => f.MainRegistrationId == caseNo)
                .Where(f => f.FolderId == folderId)
                .Select(f => new FileMetadataDto
                {
                    Id = f.Id,
                    ReferenceNo = f.ReferenceNo,
                    FileName = f.FileName,
                })
                .ToList();

                foreach (var metadata in metadataList)
                {
                    var tempData = GetFileType(metadata.FileName);
                    metadata.FileType = tempData;
                    metadata.FileName = metadata.FileName.Length <= 23 ? metadata.FileName : $"{metadata.FileName.Substring(0, 10)}...{metadata.FileName.Substring(metadata.FileName.Length - 10)}";
                }

                return metadataList;
            }
        }

        public virtual async Task<string> GetBinaryFileName(Guid? fileId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                if (!fileId.HasValue)
                {
                    return null;
                }

                var fileOrg = await _fileOrgRepository.FirstOrDefaultAsync(f => f.ReferenceNo == fileId);
                if (fileOrg == null || string.IsNullOrEmpty(fileOrg.FileName))
                {
                    return null;
                }

                if (fileOrg.FileName.Length <= 23)
                {
                    return fileOrg.FileName;
                }

                string truncatedFileName = $"{fileOrg.FileName.Substring(0, 10)}...{fileOrg.FileName.Substring(fileOrg.FileName.Length - 10)}";

                return truncatedFileName;
            }
        }



        public async Task DeleteFileByMainEntityAndMainEntityID(string mainEntity, int mainEntityId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var fileOrg = await _fileOrgRepository.GetAll().Where(f => f.FolderFk.MainEntity.Contains(mainEntity) && f.FolderFk.MainEntityId == mainEntityId).ToListAsync();

                foreach (var fileEntity in fileOrg)
                {
                    var filePath = Path.Combine(
                       await _folderService.GenerateDirectoryByFolderId((int)fileEntity.FolderId, fileEntity.MainRegistrationId.ToString()),
                       fileEntity.FileName);

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    await _fileOrgRepository.DeleteAsync(fileEntity);

                }
            }
        }

        public async Task<FileViewDto> GetFileByReferenceNo(Guid? referenceNo)
        {
            if (!referenceNo.HasValue)
            {
                throw new UserFriendlyException("File reference number is empty.");
            }
            // Find the file record by reference number
            var fileOrg = await _fileOrgRepository.FirstOrDefaultAsync(f => f.ReferenceNo == referenceNo);
            if (fileOrg == null)
            {
                throw new UserFriendlyException("File not found.");
            }
            // Construct the file path using the information from the file record
            var filePath = Path.Combine(
                await _folderService.GenerateDirectoryByFolderId((int)fileOrg.FolderId, fileOrg.MainRegistrationId.ToString()),
                fileOrg.FileName);

            // Check if the file exists 
            if (!File.Exists(filePath))
            {
                throw new UserFriendlyException("File not found on disk.");
            }
            var fileContent = await File.ReadAllBytesAsync(filePath);

            return new FileViewDto
            {
                ContentType = GetFileType(fileOrg.FileName),
                FileName = fileOrg.FileName,
                FileContent = fileContent
            };
        }


        public virtual async Task DeleteFileByReference(Guid? referenceNo)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                if (!referenceNo.HasValue)
                {
                    throw new UserFriendlyException("File reference number is empty.");
                }

                // Find the file record by reference number
                var fileOrg = await _fileOrgRepository.FirstOrDefaultAsync(f => f.ReferenceNo == referenceNo);

                if (fileOrg == null)
                {
                    throw new UserFriendlyException("File not found.");
                }

                // Construct the file path using the information from the file record
                var filePath = Path.Combine(
                    await _folderService.GenerateDirectoryByFolderId((int)fileOrg.FolderId, fileOrg.MainRegistrationId.ToString()),
                    fileOrg.FileName);

                // Check if the file exists and delete it
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Remove the record from the database
                await _fileOrgRepository.DeleteAsync(fileOrg);
            }
        }

        private string GetFileType(string fileName)
        {
            string extension = GetFileExtension(fileName);

            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                    return "Image";
                case ".pdf":
                    return "PDF";
                case ".doc":
                case ".docx":
                    return "Word";
                case ".xls":
                case ".xlsx":
                    return "Excel";
                case ".txt":
                    return "Text";
                case ".zip":
                case ".rar":
                    return "Archive";
                default:
                    return "Unknown";
            }
        }

        public string GetFileExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("Filename cannot be null or empty", nameof(fileName));
            }

            return Path.GetExtension(fileName).ToLowerInvariant();
        }
    }
}
