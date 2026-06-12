using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Audit.Dtos
{
    public class GetAuditEntryForEditOutput
    {
        public CreateOrEditAuditEntryDto AuditEntry { get; set; }

        public string AuditTrailDisplayProperty { get; set; }

    }
}