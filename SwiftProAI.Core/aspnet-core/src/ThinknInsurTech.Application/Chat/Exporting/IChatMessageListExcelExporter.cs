using System.Collections.Generic;
using Abp;
using ThinknInsurTech.Chat.Dto;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
