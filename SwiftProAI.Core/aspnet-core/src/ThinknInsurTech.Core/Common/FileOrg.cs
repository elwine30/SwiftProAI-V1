using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ThinknInsurTech.Registration;

namespace ThinknInsurTech.Common
{
    [Table("FileOrgs")]
    public class FileOrg : FullAuditedEntity, IMayHaveTenant, IMayHaveOrganizationUnit
    {
        public int? TenantId { get; set; }

        public Guid ReferenceNo { get; set; }

        // File name should also contain fileExtension
        public string FileName { get; set; }

        public int MainRegistrationId { get; set; }

        [ForeignKey("MainRegistrationId")]
        public MainRegistration MainRegistrationFk { get; set; }

        public int? FolderId { get; set; }

        [ForeignKey("FolderId")]
        public Folder FolderFk { get; set; }
        public long? OrganizationUnitId { get; set; }

    }
}