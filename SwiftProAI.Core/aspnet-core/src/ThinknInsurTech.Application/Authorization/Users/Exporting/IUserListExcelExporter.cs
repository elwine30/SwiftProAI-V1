using System.Collections.Generic;
using ThinknInsurTech.Authorization.Users.Dto;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos, List<string> selectedColumns);
    }
}