using System.Collections.Generic;
using ThinknInsurTech.Common.Dtos;

namespace ThinknInsurTech.Integration.Dto
{
    public class MoveFileInput
    {
        public List<FileOrgDto> toMoveFiles;
        public int folderId;
    }
}
