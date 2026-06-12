using System.Threading.Tasks;
using ThinknInsurTech.Registration;

namespace ThinknInsurTech.Integration
{
    public interface IOCRPromptService
    {
        Task<string> UploadDocument(UploadDocTypeEnum docType, string imageBase64String, string filename, string caseNo);
        Task<string> UploadBulkDocument(UploadDocTypeEnum docType, string[] base64Files, string filename, string caseNo);
        Task<string> GenerateSummary(string inputData, string promptName, string caseNo);
    }
}
