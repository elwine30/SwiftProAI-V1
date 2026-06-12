using System;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Registration.Dtos;

namespace ThinknInsurTech.Reports.Dtos
{
    public class InvoiceReportDto : EntityDto
    {
        public DateTime ReportDate { get; set; }

        public int CaseReference { get; set; }

        public int InsuranceCompanyId { get; set; }

        public string InsuranceCompany { get; set; }

        public string InsurerRef { get; set; }

        public string VehicleNo { get; set; }

        public string CaseType { get; set; }

        public int CaseInvoiceId { get; set; }

        public int GroupId { get; set; }

        public int AdjusterId { get; set; }

        public string AdjusterName { get; set; }

        public decimal InvService { get; set; }

        public decimal InvMileage { get; set; }

        public decimal InvPhoto { get; set; }

        public decimal InvToll { get; set; }

        public decimal InvBridge { get; set; }

        public decimal InvPolice { get; set; }

        public decimal InvStatutory { get; set; }

        public decimal InvSurveillance { get; set; }

        public decimal InvTelco { get; set; }

        public decimal InvThirdParty { get; set; }

        public decimal InvAir { get; set; }

        public decimal InvSearch { get; set; }

        public decimal InvCharteredTransport { get; set; }

        public decimal InvTaxi { get; set; }

        public decimal InvAccomodation { get; set; }

        public decimal InvMiscellaneous { get; set; }

        public decimal InvTotal { get; set; }

        public decimal InvGST { get; set; }

        public DateTime InvCreditDebitDate { get; set; }

        public DateTime InvDateSent { get; set; }

        public string InvChequeNo { get; set; }

        public DateTime? InvDatePaid { get; set; }

        public decimal InvNetAmount { get; set; }

        public decimal InvAmountPaid { get; set; }

        public string InvoiceRef { get; set; }

        //SST Items
        public decimal InvServiceSST { get; set; }

        public decimal InvMileageSST { get; set; }

        public decimal InvPhotoSST { get; set; }

        public decimal InvTollSST { get; set; }

        public decimal InvBridgeSST { get; set; }

        public decimal InvPoliceSST { get; set; }

        public decimal InvStatutorySST { get; set; }

        public decimal InvSurveillanceSST { get; set; }

        public decimal InvTelcoSST { get; set; }

        public decimal InvThirdPartySST { get; set; }

        public decimal InvAirSST { get; set; }

        public decimal InvSearchSST { get; set; }

        public decimal InvCharteredTransportSST { get; set; }

        public decimal InvTaxiSST { get; set; }

        public decimal InvAccomodationSST { get; set; }

        public decimal InvMiscellaneousSST { get; set; }

        public string CaseNo { get; set; }

    }
}