using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Audit.Dtos
{
    public class AuditEntryDto : EntityDto
    {
        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public int? AuditTrailId { get; set; }
        public string TableName { get; set; }
        public string Method { get; set; }
        public string ChangedBy { get; set; }
        public string OrganizationUnit { get; set; }
        public DateTime? ChangedDate { get; set;}

    }
}