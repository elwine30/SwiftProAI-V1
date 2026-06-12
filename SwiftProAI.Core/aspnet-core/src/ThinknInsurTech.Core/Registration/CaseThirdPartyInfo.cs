using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ThinknInsurTech.Audit;
using Abp.Organizations;

namespace ThinknInsurTech.Registration
{
    [Table("CaseThirdPartyInfos")]
    [Auditable]
    public class CaseThirdPartyInfo : FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit
    {
        public int TenantId { get; set; }

        [AuditedTrail]
        public int? Age { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxSexLength, MinimumLength = CaseThirdPartyInfoConsts.MinSexLength)]
        [AuditedTrail]
        public string Sex { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxMaritalStatusLength, MinimumLength = CaseThirdPartyInfoConsts.MinMaritalStatusLength)]
        [AuditedTrail]
        public string MaritalStatus { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxThirdPartyTypeLength, MinimumLength = CaseThirdPartyInfoConsts.MinThirdPartyTypeLength)]
        [AuditedTrail]
        public string ThirdPartyType { get; set; }

        [AuditedTrail]
        public DateTime? AdmittedDate1 { get; set; }

        [AuditedTrail]
        public DateTime? AdmittedDate2 { get; set; }

        [AuditedTrail]
        public DateTime? AdmittedDate3 { get; set; }

        [AuditedTrail]
        public DateTime? DischargeDate1 { get; set; }

        [AuditedTrail]
        public DateTime? DischargeDate2 { get; set; }

        [AuditedTrail]
        public DateTime? DischargeDate3 { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxEmployerPriorLength, MinimumLength = CaseThirdPartyInfoConsts.MinEmployerPriorLength)]
        [AuditedTrail]
        public string EmployerPrior { get; set; }

        [AuditedTrail]
        public DateTime? EmployedDateFrom { get; set; }

        [AuditedTrail]
        public DateTime? EmployedDateTo { get; set; }

        [AuditedTrail]
        public int? EPF { get; set; }

        [AuditedTrail]
        public int? SOCSO { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxMedicalBenefitLength, MinimumLength = CaseThirdPartyInfoConsts.MinMedicalBenefitLength)]
        [AuditedTrail]
        public string MedicalBenefit { get; set; }

        [AuditedTrail]
        public double? IncomeLoss { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxEmployerAdministrativeLength, MinimumLength = CaseThirdPartyInfoConsts.MinEmployerAdministrativeLength)]
        [AuditedTrail]
        public string EmployerAdministrative { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxAfterAccidentEmployerNameLength, MinimumLength = CaseThirdPartyInfoConsts.MinAfterAccidentEmployerNameLength)]
        [AuditedTrail]
        public string AfterAccidentEmployerName { get; set; }

        [AuditedTrail]
        public double? AfterAccidentEmployerIncome { get; set; }

        [AuditedTrail]
        public double? AfterAccidentEmployerIncomeReduction { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxAfterAccidentEmployerAddressLength, MinimumLength = CaseThirdPartyInfoConsts.MinAfterAccidentEmployerAddressLength)]
        [AuditedTrail]
        public string AfterAccidentEmployerAddress { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxAfterAccidentEmployerJobLength, MinimumLength = CaseThirdPartyInfoConsts.MinAfterAccidentEmployerJobLength)]
        [AuditedTrail]
        public string AfterAccidentEmployerJob { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxInjuriesSustainedLength, MinimumLength = CaseThirdPartyInfoConsts.MinInjuriesSustainedLength)]
        [AuditedTrail]
        public string InjuriesSustained { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxMedicalLeaveLength, MinimumLength = CaseThirdPartyInfoConsts.MinMedicalLeaveLength)]
        [AuditedTrail]
        public string MedicalLeave { get; set; }

        [AuditedTrail]
        public DateTime? DisablementPeriodFrom { get; set; }

        [AuditedTrail]
        public DateTime? DisablementPeriodTo { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxPresentConditionLength, MinimumLength = CaseThirdPartyInfoConsts.MinPresentConditionLength)]
        [AuditedTrail]
        public string PresentCondition { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxCurrentDisabilitiesLength, MinimumLength = CaseThirdPartyInfoConsts.MinCurrentDisabilitiesLength)]
        [AuditedTrail]
        public string CurrentDisabilities { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxSolicitorNameLength, MinimumLength = CaseThirdPartyInfoConsts.MinSolicitorNameLength)]
        [AuditedTrail]
        public string SolicitorName { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxSolicitorAddressLength, MinimumLength = CaseThirdPartyInfoConsts.MinSolicitorAddressLength)]
        [AuditedTrail]
        public string SolicitorAddress { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxSolicitorContactLength, MinimumLength = CaseThirdPartyInfoConsts.MinSolicitorContactLength)]
        [AuditedTrail]
        public string SolicitorContact { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxSolicitorReferenceNoLength, MinimumLength = CaseThirdPartyInfoConsts.MinSolicitorReferenceNoLength)]
        [AuditedTrail]
        public string SolicitorReferenceNo { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxOtherMedicalBenefitLength, MinimumLength = CaseThirdPartyInfoConsts.MinOtherMedicalBenefitLength)]
        [AuditedTrail]
        public string OtherMedicalBenefit { get; set; }

        [AuditedTrail]
        public bool FatalCaseCheck { get; set; }

        [AuditedTrail]
        [StringLength(CaseThirdPartyInfoConsts.MaxVehicleNoLength, MinimumLength = CaseThirdPartyInfoConsts.MinVehicleNoLength)]
        public string VehicleNo { get; set; }

        public int? RegisterId { get; set; }

        [ForeignKey("RegisterId")]
        public MainRegistration RegisterFk { get; set; }

        [AuditedTrail]
        public int? HospitalId1 { get; set; }

        [ForeignKey("HospitalId1")]
        public Hospital HospitalId1Fk { get; set; }

        [AuditedTrail]
        public int? HospitalId2 { get; set; }

        [ForeignKey("HospitalId2")]
        public Hospital HospitalId2Fk { get; set; }

        [AuditedTrail]
        public int? HospitalId3 { get; set; }

        [ForeignKey("HospitalId3")]
        public Hospital HospitalId3Fk { get; set; }

        [AuditedTrail]
        public int? CaseInsuredPersonId { get; set; }

        [ForeignKey("CaseInsuredPersonId")]
        public CaseInsuredPerson CaseInsuredPersonFk { get; set; }
        public long? OrganizationUnitId { get; set; }

    }
}