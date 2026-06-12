using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ThinknInsurTech.DataExporting.Excel.MiniExcel;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Storage;

namespace ThinknInsurTech.Reports.Exporting
{
    public class InvoiceReportsExcelExporter : MiniExcelExcelExporterBase, IInvoiceReportsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public InvoiceReportsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetInvoiceReportForViewDto> invoiceReports)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var invoiceReport in invoiceReports)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {"Report Date", invoiceReport.InvoiceReport.ReportDate},
                        {"Case No", invoiceReport.InvoiceReport.CaseNo},
                        {"Insurance Company", invoiceReport.InvoiceReport.InsuranceCompany},
                        {"Insurer Ref", invoiceReport.InvoiceReport.InsurerRef},
                        {"Vehicle No", invoiceReport.InvoiceReport.VehicleNo},
                        {"Case Type", invoiceReport.InvoiceReport.CaseType},
                        {"Service", invoiceReport.InvoiceReport.InvService},
                        {"Service SST", invoiceReport.InvoiceReport.InvServiceSST},
                        {"Mileage", invoiceReport.InvoiceReport.InvMileage},
                        {"Mileage SST", invoiceReport.InvoiceReport.InvMileageSST},
                        {"Photo", invoiceReport.InvoiceReport.InvPhoto},
                        {"Photo SST",invoiceReport.InvoiceReport.InvPhotoSST},
                        {"Toll", invoiceReport.InvoiceReport.InvToll},
                        {"Toll SST", invoiceReport.InvoiceReport.InvTollSST},
                        {"Bridge", invoiceReport.InvoiceReport.InvBridge},
                        {"Bridge SST", invoiceReport.InvoiceReport.InvBridgeSST},
                        {"Police", invoiceReport.InvoiceReport.InvPolice},
                        {"Police SST", invoiceReport.InvoiceReport.InvPoliceSST},
                        {"Statutory", invoiceReport.InvoiceReport.InvStatutory},
                        {"Statutory SST", invoiceReport.InvoiceReport.InvStatutorySST},
                        {"Surveillance", invoiceReport.InvoiceReport.InvSurveillance},
                        {"Surveillance SST", invoiceReport.InvoiceReport.InvSurveillanceSST},
                        {"Telco", invoiceReport.InvoiceReport.InvTelco},
                        {"Telco SST", invoiceReport.InvoiceReport.InvTelcoSST},
                        {"Third Party", invoiceReport.InvoiceReport.InvThirdParty},
                        {"Third Party SST", invoiceReport.InvoiceReport.InvThirdPartySST},
                        {"Air Fare", invoiceReport.InvoiceReport.InvAir},
                        {"Air Fare SST", invoiceReport.InvoiceReport.InvAirSST},
                        {"Search Fee", invoiceReport.InvoiceReport.InvSearch},
                        {"Search Fee SST", invoiceReport.InvoiceReport.InvSearchSST},
                        {"Adjuster", invoiceReport.InvoiceReport.AdjusterName},
                        {"Date Sent", invoiceReport.InvoiceReport.InvDateSent},
                        {"Cheque No", invoiceReport.InvoiceReport.InvChequeNo},
                        {"Date Paid", invoiceReport.InvoiceReport.InvDatePaid},
                        {"Net Amount", invoiceReport.InvoiceReport.InvNetAmount},
                        {"SST Amount" , invoiceReport.InvoiceReport.InvGST},
                        {"Total Amount", invoiceReport.InvoiceReport.InvTotal},
                        {"Amount Paid", invoiceReport.InvoiceReport.InvAmountPaid},
                        {"Invoice Ref", invoiceReport.InvoiceReport.InvoiceRef},

                    });
            }

            return CreateExcelPackage("InvoiceReports.xlsx", items);

        }
    }
}