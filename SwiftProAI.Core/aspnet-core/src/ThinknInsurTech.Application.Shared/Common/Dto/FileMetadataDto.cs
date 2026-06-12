using System;

namespace ThinknInsurTech.Common.Dto
{
    public class FileMetadataDto
    {
        public int Id { get; set; }
        public Guid ReferenceNo { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public DateTime LastModified { get; set; }
        public bool? IsReadOnly { get; set; }
    }
}
