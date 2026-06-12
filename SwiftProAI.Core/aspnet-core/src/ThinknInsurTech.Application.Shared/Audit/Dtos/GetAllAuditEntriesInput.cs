using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Audit.Dtos
{
    public class GetAllAuditEntriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string TableName { get; set; }
        public int? OrganizationUnitId { get; set; }

    }
}