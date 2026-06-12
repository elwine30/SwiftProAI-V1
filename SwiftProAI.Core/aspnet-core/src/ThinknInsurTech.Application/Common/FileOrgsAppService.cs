using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Common.Dto;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Configuration;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Common
{
    [AbpAuthorize(AppPermissions.Pages_FileOrgs)]
    public class FileOrgsAppService : ThinknInsurTechAppServiceBase, IFileOrgsAppService
    {
        private readonly IRepository<FileOrg> _fileOrgRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IRepository<Folder, int> _lookup_folderRepository;
        private readonly IHostEnvironment _env;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private static IConfigurationRoot _configuration;


        public FileOrgsAppService(IHostEnvironment env, IAppConfigurationAccessor appConfigurationAccessor, IRepository<FileOrg> fileOrgRepository, IRepository<MainRegistration, int> lookup_mainRegistrationRepository, IRepository<Folder, int> lookup_folderRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _env = env;
            _fileOrgRepository = fileOrgRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _lookup_folderRepository = lookup_folderRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = appConfigurationAccessor.Configuration;
        }

        public List<FileMetadataDto> GetMetadataByFolderAndCase(FileViewInputByFolderAndCase input)
        {
            using (UnitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                if (!int.TryParse(input.CaseNo, out int caseNo))
                {
                    // Handle the case where the input.CaseNo is not a valid integer
                    throw new ArgumentException("Invalid CaseNo", nameof(input.CaseNo));
                }

                var foundFolder = new Folder();

                if (input.FieldName != null)
                {
                    foundFolder = _lookup_folderRepository.FirstOrDefault(f => f.Field == input.FieldName);
                    input.FolderId = foundFolder.Id;
                }

                var metadataList = _fileOrgRepository.GetAll()
                    .Where(f => f.MainRegistrationId == caseNo)
                    .WhereIf(input.FolderId != null, f => f.FolderId == input.FolderId)
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
                }

                return metadataList;

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


        //<summary>
        //Upload File does :
        //Save into server with a generatedPath then create a record under database FileOrg
        //</summary>
        public async Task<FileMetadataDto> UploadFile(FileUploadInput input)
        {

            if (input.FileContent == null)
            {
                throw new UserFriendlyException("File is not uploaded");
            }

            var uploadFolder = generateDirectory((int)input.FileOrgOb.FolderId, input.FileOrgOb.MainRegistrationId.ToString());

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var filePath = Path.Combine(uploadFolder, input.FileOrgOb.FileName);

            await File.WriteAllBytesAsync(filePath, input.FileContent);


            if (AbpSession.GetCurrentOUId() != null)
            {
                input.FileOrgOb.OrganizationUnitId = AbpSession.GetCurrentOUId();
            }
            input.FileOrgOb.TenantId = AbpSession.GetTenantId();
            input.FileOrgOb.ReferenceNo = Guid.NewGuid();
            await _fileOrgRepository.InsertAsync(ObjectMapper.Map<FileOrg>(input.FileOrgOb));
            return new FileMetadataDto
            {
                Id = input.FileOrgOb.Id,
                FileName = input.FileOrgOb.FileName,
                ReferenceNo = input.FileOrgOb.ReferenceNo,
                FileType = GetFileType(input.FileOrgOb.FileName),
            };

        }

        public async Task DeleteFileByReference(Guid referenceNo)
        {
            // Find the file record by reference number
            using (UnitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var fileOrg = await _fileOrgRepository.FirstOrDefaultAsync(f => f.ReferenceNo == referenceNo);

                if (fileOrg == null)
                {
                    throw new UserFriendlyException("File not found.");
                }

                // Construct the file path using the information from the file record
                var filePath = Path.Combine(
                    generateDirectory((int)fileOrg.FolderId, fileOrg.MainRegistrationId.ToString()),
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



        public async Task<String> RenameFileByReference(Guid referenceNo, string newFileName)
        {
            FileOrg fileOrg;

            // Find the file record by reference number
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                fileOrg = await _fileOrgRepository.FirstOrDefaultAsync(f => f.ReferenceNo == referenceNo);
            }


            if (fileOrg == null)
            {
                throw new UserFriendlyException("File not found.");
            }

            var originalExtension = Path.GetExtension(fileOrg.FileName);

            var newFileNameWithoutExtension = Path.GetFileNameWithoutExtension(newFileName);

            var newFileNameWithOriginalExtension = $"{newFileNameWithoutExtension}{originalExtension}";

            var currentFilePath = Path.Combine(
                generateDirectory((int)fileOrg.FolderId, fileOrg.MainRegistrationId.ToString()),
                fileOrg.FileName);

            var newFilePath = Path.Combine(
                generateDirectory((int)fileOrg.FolderId, fileOrg.MainRegistrationId.ToString()),
                newFileNameWithOriginalExtension);

            if (File.Exists(currentFilePath))
            {
                File.Move(currentFilePath, newFilePath);
            }
            else
            {
                throw new UserFriendlyException("File not found on disk.");
            }

            // Update the record in the database with the new file name
            fileOrg.FileName = newFileNameWithOriginalExtension;
            await _fileOrgRepository.UpdateAsync(fileOrg);
            return newFileNameWithOriginalExtension;
        }

        //public async Task<FilesViewDto> ViewFilesByFieldNameAndCase(FileViewInputByFolderAndCase input)
        //{
        //    var path = generateDirectory(input.FolderId, input.CaseNo);
        //    var files = Directory.GetFiles(path).Select(filePath => new FileInfo(filePath)).ToList();

        //    var filesViewDto = new FilesViewDto();

        //    foreach (var file in files)
        //    {
        //        var fileContent = await File.ReadAllBytesAsync(file.FullName);

        //        filesViewDto.Files.Add(new FileViewDto
        //        {
        //            FileName = file.Name,
        //            FileContent = fileContent
        //        });
        //    }

        //    return filesViewDto;
        //}

        public async Task<FilesViewDto> ViewFilesByFolderAndCase(FileViewInputByFolderAndCase input)
        {
            var path = generateDirectory((int)input.FolderId, input.CaseNo);
            var files = Directory.GetFiles(path).Select(filePath => new FileInfo(filePath)).ToList();

            var filesViewDto = new FilesViewDto();

            foreach (var file in files)
            {
                var fileContent = await File.ReadAllBytesAsync(file.FullName);

                filesViewDto.Files.Add(new FileViewDto
                {
                    FileName = file.Name,
                    FileContent = fileContent
                });
            }

            return filesViewDto;
        }



        public async Task<FileViewDto> ViewFileByReference(FileViewInputByReference input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                var fileOrg = _fileOrgRepository.FirstOrDefault(f => f.ReferenceNo == input.ReferenceNo);

                if (fileOrg == null)
                {
                    throw new UserFriendlyException("File not found.");
                }

                var path = generateDirectory((int)fileOrg.FolderId, fileOrg.MainRegistrationId.ToString());
                var filePath = Path.Combine(path, fileOrg.FileName);

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
        }


        private string generateDirectory(int folderId, string caseNo)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {

                var path = "";
                //Generate combined base root 

                //Sample generated : C://Files
                Folder folder = _lookup_folderRepository.Get(folderId);

                var root = Path.Combine(Directory.GetDirectoryRoot(_env.ContentRootPath), _configuration.GetSection("Folder")["root"]);

                //Sample generated : C://Files/[TenantId]/[MainRegId]/CaseInsurer/Dri-Lic-Back
                path = Path.Combine(root, AbpSession.TenantId.ToString(),
                    caseNo,
                    folder.MainEntity,
                    folder.Field);

                return path;
            }


        }




    }
}