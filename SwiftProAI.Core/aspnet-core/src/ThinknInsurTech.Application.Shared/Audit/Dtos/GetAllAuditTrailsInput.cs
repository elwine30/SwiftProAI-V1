using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Audit.Dtos
{
    public abstract class GetAllAuditTrailsInputBase : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string OperationFilter { get; set; }

        public string TableNameFilter { get; set; }

        public string ChangedByFilter { get; set; }

        public DateTime? MaxChangedDateFilter { get; set; }
        public DateTime? MinChangedDateFilter { get; set; }

    }
}