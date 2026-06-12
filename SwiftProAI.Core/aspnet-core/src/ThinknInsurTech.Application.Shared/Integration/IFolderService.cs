using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.Common.Dtos;


namespace ThinknInsurTech.Integration
{
    public interface IFolderService
    {
        List<FolderDto> GetAll(GetAllFoldersInput input);
        List<FolderDto> GetAllByMainEntityAndId(string mainEntity, int mainEntityId);
        Task<List<FolderDto>> GetAllByMainEntityAndIdAsync(string mainEntity, int mainEntityId);

        Task<int> CreateByMainEntityAndField(string mainEntity, string field, int mainEntityId);

        Task<Dictionary<string, Dictionary<string, int>>> GetAllInDictionary(int registerId);

        Task MoveIntoFolderAsync(string sorucePath, string targetPath);

        Task<string> GenerateDirectory(string folderField, string caseNo);
        Task<string> GenerateDirectoryByFolderId(int folderId, string caseNo);
        Task DeleteFolder(int folderId, int registerId);



    }
}
