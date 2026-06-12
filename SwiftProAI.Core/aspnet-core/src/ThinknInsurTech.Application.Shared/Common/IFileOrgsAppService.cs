using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Common.Dto;
using ThinknInsurTech.Common.Dtos;

namespace ThinknInsurTech.Common
{
    public interface IFileOrgsAppService : IApplicationService
    {
        Task<FileMetadataDto> UploadFile(FileUploadInput input);
        Task<String> RenameFileByReference(Guid referenceNo, string newFileName);
        Task DeleteFileByReference(Guid referenceNo);

        List<FileMetadataDto> GetMetadataByFolderAndCase(FileViewInputByFolderAndCase input);
        Task<FilesViewDto> ViewFilesByFolderAndCase(FileViewInputByFolderAndCase input);
        //Task<FilesViewDto> ViewFilesByFieldNameAndCase(FileViewInputByFolderAndCase input);

        Task<FileViewDto> ViewFileByReference(FileViewInputByReference input);
    }
}