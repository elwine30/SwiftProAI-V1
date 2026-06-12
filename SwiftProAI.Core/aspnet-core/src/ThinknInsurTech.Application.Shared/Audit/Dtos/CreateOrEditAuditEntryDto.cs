using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Audit.Dtos
{
    public class CreateOrEditAuditEntryDto : EntityDto<int?>
    {

        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public int? AuditTrailId { get; set; }

    }
}