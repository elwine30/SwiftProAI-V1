using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class CaseThirdPartyInfoDto : EntityDto
    {
        public int? Age { get; set; }

        public string Sex { get; set; }

        public string MaritalStatus { get; set; }

        public string ThirdPartyType { get; set; }

        public DateTime? AdmittedDate1 { get; set; }

        public DateTime? AdmittedDate2 { get; set; }

        public DateTime? AdmittedDate3 { get; set; }

        public DateTime? DischargeDate1 { get; set; }

        public DateTime? DischargeDate2 { get; set; }

        public DateTime? DischargeDate3 { get; set; }

        public string EmployerPrior { get; set; }

        public DateTime? EmployedDateFrom { get; set; }

        public DateTime? EmployedDateTo { get; set; }

        public int? EPF { get; set; }

        public int? SOCSO { get; set; }

        public string MedicalBenefit { get; set; }

        public double? IncomeLoss { get; set; }

        public string EmployerAdministrative { get; set; }

        public string AfterAccidentEmployerName { get; set; }

        public double? AfterAccidentEmployerIncome { get; set; }

        public double? AfterAccidentEmployerIncomeReduction { get; set; }

        public string AfterAccidentEmployerAddress { get; set; }

        public string AfterAccidentEmployerJob { get; set; }

        public string InjuriesSustained { get; set; }

        public string MedicalLeave { get; set; }

        public DateTime? DisablementPeriodFrom { get; set; }

        public DateTime? DisablementPeriodTo { get; set; }

        public string PresentCondition { get; set; }

        public string CurrentDisabilities { get; set; }

        public string SolicitorName { get; set; }

        public string SolicitorAddress { get; set; }

        public string SolicitorContact { get; set; }

        public string SolicitorReferenceNo { get; set; }

        public string OtherMedicalBenefit { get; set; }

        public bool FatalCaseCheck { get; set; }

        public string VehicleNo { get; set; }

        public int? RegisterId { get; set; }

        public int? HospitalId1 { get; set; }

        public int? HospitalId2 { get; set; }

        public int? HospitalId3 { get; set; }

        public int? CaseInsuredPersonId { get; set; }

        public InsuredPersonDto CaseInsuredPerson { get; set; }

    }
}