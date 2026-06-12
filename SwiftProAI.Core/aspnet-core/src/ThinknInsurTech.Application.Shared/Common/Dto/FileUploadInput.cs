using ThinknInsurTech.Common.Dtos;

namespace ThinknInsurTech.Common.Dto
{
    public class FileUploadInput
    {
        public FileOrgDto FileOrgOb { get; set; }
        public string ContentType { get; set; }
        public byte[] FileContent { get; set; }

    }

}
