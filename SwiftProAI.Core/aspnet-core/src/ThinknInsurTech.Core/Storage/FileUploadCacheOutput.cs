using Abp.Web.Models;

namespace ThinknInsurTech.Storage
{
    public class FileUploadCacheOutput : ErrorInfo
    {
        public string FileToken { get; set; }
        public string FileName { get; set; }
        public string Output { get; set; }
        public FileUploadCacheOutput(string fileToken)
        {
            FileToken = fileToken;
            FileName = "";
            Output = "";
        }
        public FileUploadCacheOutput(string fileToken, string fileName, string output)
        {
            FileToken = fileToken;
            FileName = fileName;
            Output = output;
        }

        public FileUploadCacheOutput(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }
    }
}