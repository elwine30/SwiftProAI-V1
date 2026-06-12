using ThinknInsurTech.Companies;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CaseInsurers")]
    [Auditable]
    public class CaseInsurer : FullAuditedEntity<int>, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [AuditedTrail]
        public string ReferenceNo { get; set; }

        [AuditedTrail]
        public string Name { get; set; }

        [AuditedTrail]
        public string Contact { get; set; }

        [AuditedTrail]
        public string Email { get; set; }

        public int RegisterId { get; set; }

        [AuditedTrail]
        public virtual int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public InsuranceCompany CompanyFk { get; set; }

        public long? OrganizationUnitId { get; set; }

    }
}