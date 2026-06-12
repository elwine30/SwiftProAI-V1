using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllCaseInvoicesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ServiceRemarkFilter { get; set; }

        public decimal? MaxServiceAmountFilter { get; set; }
        public decimal? MinServiceAmountFilter { get; set; }

        public decimal? MaxServiceSSTFilter { get; set; }
        public decimal? MinServiceSSTFilter { get; set; }

        public string MileageRemarkFilter { get; set; }

        public decimal? MaxMileageUnitPriceFilter { get; set; }
        public decimal? MinMileageUnitPriceFilter { get; set; }

        public decimal? MaxMileageKMFilter { get; set; }
        public decimal? MinMileageKMFilter { get; set; }

        public decimal? MaxMileageAmountFilter { get; set; }
        public decimal? MinMileageAmountFilter { get; set; }

        public decimal? MaxMileageSSTFilter { get; set; }
        public decimal? MinMileageSSTFilter { get; set; }

        public string PhotographRemarkFilter { get; set; }

        public decimal? MaxPhotographChargeFilter { get; set; }
        public decimal? MinPhotographChargeFilter { get; set; }

        public int? MaxPhotographQtyFilter { get; set; }
        public int? MinPhotographQtyFilter { get; set; }

        public decimal? MaxPhotographTotalPriceFilter { get; set; }
        public decimal? MinPhotographTotalPriceFilter { get; set; }

        public decimal? MaxPhotographSSTFilter { get; set; }
        public decimal? MinPhotographSSTFilter { get; set; }

        public string TollRemarkFilter { get; set; }

        public decimal? MaxTollAmountFilter { get; set; }
        public decimal? MinTollAmountFilter { get; set; }

        public decimal? MaxTollSSTFilter { get; set; }
        public decimal? MinTollSSTFilter { get; set; }

        public string BridgeTollRemarkFilter { get; set; }

        public decimal? MaxBridgeTollAmountFilter { get; set; }
        public decimal? MinBridgeTollAmountFilter { get; set; }

        public decimal? MaxBridgeTollSSTFilter { get; set; }
        public decimal? MinBridgeTollSSTFilter { get; set; }

        public decimal? MaxPoliceAmountFilter { get; set; }
        public decimal? MinPoliceAmountFilter { get; set; }

        public decimal? MaxPoliceSSTFilter { get; set; }
        public decimal? MinPoliceSSTFilter { get; set; }

        public decimal? MaxStatutoryDeclarationAmountFilter { get; set; }
        public decimal? MinStatutoryDeclarationAmountFilter { get; set; }

        public decimal? MaxStatutoryDeclarationSSTFilter { get; set; }
        public decimal? MinStatutoryDeclarationSSTFilter { get; set; }

        public string SurveillanceRemarkFilter { get; set; }

        public decimal? MaxSurveillanceAmountFilter { get; set; }
        public decimal? MinSurveillanceAmountFilter { get; set; }

        public decimal? MaxSurveillanceSSTFilter { get; set; }
        public decimal? MinSurveillanceSSTFilter { get; set; }

        public decimal? MaxTelcoAmountFilter { get; set; }
        public decimal? MinTelcoAmountFilter { get; set; }

        public decimal? MaxTelcoSSTFilter { get; set; }
        public decimal? MinTelcoSSTFilter { get; set; }

        public decimal? MaxThirdPartyAmountFilter { get; set; }
        public decimal? MinThirdPartyAmountFilter { get; set; }

        public decimal? MaxThirdPartySSTFilter { get; set; }
        public decimal? MinThirdPartySSTFilter { get; set; }

        public string CourtAttendanceRemarkFilter { get; set; }

        public decimal? MaxCourtAttendanceAmountFilter { get; set; }
        public decimal? MinCourtAttendanceAmountFilter { get; set; }

        public decimal? MaxCourtAttendanceSSTFilter { get; set; }
        public decimal? MinCourtAttendanceSSTFilter { get; set; }

        public decimal? MaxSearchFeeAmountFilter { get; set; }
        public decimal? MinSearchFeeAmountFilter { get; set; }

        public decimal? MaxSearchFeeSSTFilter { get; set; }
        public decimal? MinSearchFeeSSTFilter { get; set; }

        public decimal? MaxAirFareAmountFilter { get; set; }
        public decimal? MinAirFareAmountFilter { get; set; }

        public decimal? MaxAirFareSSTFilter { get; set; }
        public decimal? MinAirFareSSTFilter { get; set; }

        public decimal? MaxCharteredAmountFilter { get; set; }
        public decimal? MinCharteredAmountFilter { get; set; }

        public decimal? MaxCharteredSSTFilter { get; set; }
        public decimal? MinCharteredSSTFilter { get; set; }

        public decimal? MaxTaxiFareAmountFilter { get; set; }
        public decimal? MinTaxiFareAmountFilter { get; set; }

        public decimal? MaxTaxiFareSSTFilter { get; set; }
        public decimal? MinTaxiFareSSTFilter { get; set; }

        public decimal? MaxAccommodationAmountFilter { get; set; }
        public decimal? MinAccommodationAmountFilter { get; set; }

        public decimal? MaxAccommodationSSTFilter { get; set; }
        public decimal? MinAccommodationSSTFilter { get; set; }

        public decimal? MaxMiscAmountFilter { get; set; }
        public decimal? MinMiscAmountFilter { get; set; }

        public string MiscSSTFilter { get; set; }

        public decimal? MaxAmountExcludeSSTFilter { get; set; }
        public decimal? MinAmountExcludeSSTFilter { get; set; }

        public decimal? MaxAmountWithSSTFilter { get; set; }
        public decimal? MinAmountWithSSTFilter { get; set; }

        public string TotalInTextFormFilter { get; set; }

        public decimal? MaxTotalAmountFilter { get; set; }
        public decimal? MinTotalAmountFilter { get; set; }

        public string InvoiceRefNoFilter { get; set; }

        public DateTime? MaxInvoiceDateFilter { get; set; }
        public DateTime? MinInvoiceDateFilter { get; set; }

        public string InvoiceFlagFilter { get; set; }

        public string InvoiceTypeFilter { get; set; }

        public string DebitRefNoFilter { get; set; }

        public DateTime? MaxDebitDateFilter { get; set; }
        public DateTime? MinDebitDateFilter { get; set; }

        public decimal? MaxNetAmountFilter { get; set; }
        public decimal? MinNetAmountFilter { get; set; }

        public string CheckNoFilter { get; set; }

        public DateTime? MaxDatePaidFilter { get; set; }
        public DateTime? MinDatePaidFilter { get; set; }

        public string LawyerFilter { get; set; }

        public string PaymentFlagFilter { get; set; }

        public decimal? MaxAmountPaidFilter { get; set; }
        public decimal? MinAmountPaidFilter { get; set; }

        public decimal? MaxCreditAmountFilter { get; set; }
        public decimal? MinCreditAmountFilter { get; set; }

        public decimal? MaxDebitAmountFilter { get; set; }
        public decimal? MinDebitAmountFilter { get; set; }

        public string PaymentModeFilter { get; set; }

        public string MainRegistrationVehicleNoFilter { get; set; }

        public string CompanyNameFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string UserName2Filter { get; set; }

        public string CaseTypeShortNameFilter { get; set; }

    }
}