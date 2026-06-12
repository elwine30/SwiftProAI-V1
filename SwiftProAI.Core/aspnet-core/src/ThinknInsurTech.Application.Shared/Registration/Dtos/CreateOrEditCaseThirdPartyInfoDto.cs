using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CreateOrEditCaseThirdPartyInfoDto : EntityDto<int?>
    {

        public int? Age { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxSexLength, MinimumLength = CaseThirdPartyInfoConsts.MinSexLength)]
        public string Sex { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxMaritalStatusLength, MinimumLength = CaseThirdPartyInfoConsts.MinMaritalStatusLength)]
        public string MaritalStatus { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxThirdPartyTypeLength, MinimumLength = CaseThirdPartyInfoConsts.MinThirdPartyTypeLength)]
        public string ThirdPartyType { get; set; }

        public DateTime? AdmittedDate1 { get; set; }

        public DateTime? AdmittedDate2 { get; set; }

        public DateTime? AdmittedDate3 { get; set; }

        public DateTime? DischargeDate1 { get; set; }

        public DateTime? DischargeDate2 { get; set; }

        public DateTime? DischargeDate3 { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxEmployerPriorLength, MinimumLength = CaseThirdPartyInfoConsts.MinEmployerPriorLength)]
        public string EmployerPrior { get; set; }

        public DateTime? EmployedDateFrom { get; set; }

        public DateTime? EmployedDateTo { get; set; }

        public int? EPF { get; set; }

        public int? SOCSO { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxMedicalBenefitLength, MinimumLength = CaseThirdPartyInfoConsts.MinMedicalBenefitLength)]
        public string MedicalBenefit { get; set; }

        public double? IncomeLoss { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxEmployerAdministrativeLength, MinimumLength = CaseThirdPartyInfoConsts.MinEmployerAdministrativeLength)]
        public string EmployerAdministrative { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxAfterAccidentEmployerNameLength, MinimumLength = CaseThirdPartyInfoConsts.MinAfterAccidentEmployerNameLength)]
        public string AfterAccidentEmployerName { get; set; }

        public double? AfterAccidentEmployerIncome { get; set; }

        public double? AfterAccidentEmployerIncomeReduction { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxAfterAccidentEmployerAddressLength, MinimumLength = CaseThirdPartyInfoConsts.MinAfterAccidentEmployerAddressLength)]
        public string AfterAccidentEmployerAddress { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxAfterAccidentEmployerJobLength, MinimumLength = CaseThirdPartyInfoConsts.MinAfterAccidentEmployerJobLength)]
        public string AfterAccidentEmployerJob { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxInjuriesSustainedLength, MinimumLength = CaseThirdPartyInfoConsts.MinInjuriesSustainedLength)]
        public string InjuriesSustained { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxMedicalLeaveLength, MinimumLength = CaseThirdPartyInfoConsts.MinMedicalLeaveLength)]
        public string MedicalLeave { get; set; }

        public DateTime? DisablementPeriodFrom { get; set; }

        public DateTime? DisablementPeriodTo { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxPresentConditionLength, MinimumLength = CaseThirdPartyInfoConsts.MinPresentConditionLength)]
        public string PresentCondition { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxCurrentDisabilitiesLength, MinimumLength = CaseThirdPartyInfoConsts.MinCurrentDisabilitiesLength)]
        public string CurrentDisabilities { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxSolicitorNameLength, MinimumLength = CaseThirdPartyInfoConsts.MinSolicitorNameLength)]
        public string SolicitorName { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxSolicitorAddressLength, MinimumLength = CaseThirdPartyInfoConsts.MinSolicitorAddressLength)]
        public string SolicitorAddress { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxSolicitorContactLength, MinimumLength = CaseThirdPartyInfoConsts.MinSolicitorContactLength)]
        public string SolicitorContact { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxSolicitorReferenceNoLength, MinimumLength = CaseThirdPartyInfoConsts.MinSolicitorReferenceNoLength)]
        public string SolicitorReferenceNo { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxOtherMedicalBenefitLength, MinimumLength = CaseThirdPartyInfoConsts.MinOtherMedicalBenefitLength)]
        public string OtherMedicalBenefit { get; set; }

        public bool FatalCaseCheck { get; set; }

        [StringLength(CaseThirdPartyInfoConsts.MaxVehicleNoLength, MinimumLength = CaseThirdPartyInfoConsts.MinVehicleNoLength)]
        public string VehicleNo { get; set; }

        public int? RegisterId { get; set; }

        public int? HospitalId1 { get; set; }

        public int? HospitalId2 { get; set; }

        public int? HospitalId3 { get; set; }

        public int? CaseInsuredPersonId { get; set; }

        public CreateOrEditInsuredPersonDto CaseInsuredPerson { get; set; }

        public string EmpToken { get; set; }
        public string EmpReferenceNo { get; set; } //Uploaded File
        public string EmpFileName { get; set; }

        public string DetToken { get; set; }
        public string DetReferenceNo { get; set; } //Uploaded File
        public string DetFileName { get; set; }

        public string DlDetBackToken { get; set; }
        public string DlDetBackReferenceNo { get; set; } //Uploaded File
        public string DlDetBackFileName { get; set; }

        public string DlDetFrontToken { get; set; }
        public string DlDetFrontReferenceNo { get; set; } //Uploaded File
        public string DlDetFrontFileName { get; set; }

        public string DlNricBackToken { get; set; }
        public string DlNricBackReferenceNo { get; set; } //Uploaded File
        public string DlNricBackFileName { get; set; }

        public string NricDetFrontToken { get; set; }
        public string NricDetFrontReferenceNo { get; set; } //Uploaded File
        public string NricDetFrontFileName { get; set; }

        public string NoiToken { get; set; }
        public string NoiReferenceNo { get; set; } //Uploaded File
        public string NoiFileName { get; set; }

        public string HospToken { get; set; }
        public string HospReferenceNo { get; set; } //Uploaded File
        public string HospFileName { get; set; }

    }
}