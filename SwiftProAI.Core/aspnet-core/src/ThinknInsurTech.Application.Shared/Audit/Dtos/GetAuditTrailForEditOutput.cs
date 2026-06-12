using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Audit.Dtos
{
    public abstract class GetAuditTrailForEditOutputBase
    {
        public CreateOrEditAuditTrailDto AuditTrail { get; set; }

    }
}