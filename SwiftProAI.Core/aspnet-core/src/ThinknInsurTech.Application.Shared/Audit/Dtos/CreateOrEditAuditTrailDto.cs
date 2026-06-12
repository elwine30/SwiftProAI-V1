using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Audit.Dtos
{
    public abstract class CreateOrEditAuditTrailDtoBase : EntityDto<int?>
    {

        public string Operation { get; set; }

        public string TableName { get; set; }

        public string ChangedBy { get; set; }

        public string OrganizationUnit { get; set; }

        public DateTime ChangedDate { get; set; }

    }
}