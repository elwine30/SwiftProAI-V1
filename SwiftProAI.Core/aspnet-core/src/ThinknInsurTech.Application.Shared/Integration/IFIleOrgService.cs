using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Common.Dto;

namespace ThinknInsurTech.Integration
{
    public interface IFileOrgService
    {

        Task<Guid?> GetBinaryObjectFromCache(string fileToken, int registerId, string folderField);
        Task<Guid?> GetBinaryObjectFromCacheToId(string fileToken, int registerId, int folderId);

        List<FileMetadataDto> GetMetadataByFolderIdAndCaseNo(int folderId, int caseNo);
        FileMetadataDto GetMetadataByReference(Guid fileId);

        Task<string> GetBinaryFileName(Guid? fileId);

        Task DeleteFileByReference(Guid? referenceNo);
        Task DeleteFileByMainEntityAndMainEntityID(string mainEntity, int mainEntityId);

    }
}
