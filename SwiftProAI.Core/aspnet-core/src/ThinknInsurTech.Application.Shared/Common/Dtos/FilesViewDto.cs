using System.Collections.Generic;

namespace ThinknInsurTech.Common.Dtos
{
    public class FilesViewDto
    {
        public List<FileViewDto> Files { get; set; }

        public FilesViewDto()
        {
            Files = new List<FileViewDto>();
        }
    }
}
