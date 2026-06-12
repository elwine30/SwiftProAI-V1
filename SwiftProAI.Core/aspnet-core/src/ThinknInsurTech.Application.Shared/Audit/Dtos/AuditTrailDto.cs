using System;
using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Audit.Dtos
{
    public abstract class AuditTrailDtoBase : EntityDto
    {
        public string Operation { get; set; }

        public string TableName { get; set; }

        public string ChangedBy { get; set; }

        public string OrganizationUnit { get; set; }

        public DateTime ChangedDate { get; set; }

    }
}