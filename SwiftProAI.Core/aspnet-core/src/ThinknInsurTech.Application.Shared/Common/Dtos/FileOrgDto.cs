using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Common.Dtos
{
    public class FileOrgDto : EntityDto
    {
        //Backend
        public Guid ReferenceNo { get; set; }
        //FrontEnd
        public string FileName { get; set; }
        //FrontEnd
        public int MainRegistrationId { get; set; }
        //FrontEnd
        public int? FolderId { get; set; }
        public long? OrganizationUnitId { get; set; }
        public int? TenantId { get; set; }
    }
}