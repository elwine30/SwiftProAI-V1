using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Audit.Dtos
{
    public class GetAllAuditEntriesForExcelInput
    {
        public string Filter { get; set; }
        public string TableName { get; set; }

    }
}