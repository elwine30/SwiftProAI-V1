using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Reports.Exporting;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Dto;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Registration.Dto;
using ThinknInsurTech.Companies;
using ThinknInsurTech.Common;

namespace ThinknInsurTech.Reports
{
    [AbpAuthorize(AppPermissions.Pages_InvoiceReports)]
    public class InvoiceReportsAppService : ThinknInsurTechAppServiceBase, IInvoiceReportsAppService
    {
        private readonly IInvoiceReportsExcelExporter _invoiceReportsExcelExporter;
        private readonly IRepository<CaseInvoice> _caseInvoiceRepository;
        private readonly IRepository<CaseCreditNote> _caseCreditNoteRepository;
        private readonly IRepository<CaseDebitNote> _caseDebitNoteRepository;
        private readonly IRepository<MainRegistration> _mainRegistrationRepository;
        private readonly IRepository<CaseInsurer> _caseInsurerRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<InsuranceCompany, int> _insuranceCompanyRepository;
        private readonly IRepository<Staff, int> _staffRepository;

        public InvoiceReportsAppService(IInvoiceReportsExcelExporter invoiceReportsExcelExporter, IRepository<CaseInvoice> caseInvoiceRepository, IRepository<MainRegistration> mainRegistrationRepository, IRepository<CaseInsurer> caseInsurerRepository, IRepository<User, long> lookup_userRepository, IRepository<InsuranceCompany, int> insuranceCompanyRepository, IRepository<Staff, int> staffRepository, IRepository<CaseCreditNote> caseCreditNoteRepository, IRepository<CaseDebitNote> caseDebitNoteRepository)
        {
            _invoiceReportsExcelExporter = invoiceReportsExcelExporter;
            _caseInvoiceRepository = caseInvoiceRepository;
            _mainRegistrationRepository = mainRegistrationRepository;
            _caseInsurerRepository = caseInsurerRepository;
            _lookup_userRepository = lookup_userRepository;
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _staffRepository = staffRepository;
            _caseCreditNoteRepository = caseCreditNoteRepository;
            _caseDebitNoteRepository = caseDebitNoteRepository;
        }

        public virtual async Task<PagedResultDto<GetInvoiceReportForViewDto>> GetAll(GetAllInvoiceReportsInput input)
        {
            IQueryable<InvoiceReportDto> filteredInvoiceReports = Enumerable.Empty<InvoiceReportDto>().AsQueryable();

            switch (input.InvoiceTypeFilter)
            {
                case InvoiceTypeEnum.CaseInvoice:
                    filteredInvoiceReports = (from o in _caseInvoiceRepository.GetAll()
                                              join mainRegistration in _mainRegistrationRepository.GetAll() on o.RegisterId equals mainRegistration.Id into j2
                                              from s2 in j2.DefaultIfEmpty()
                                              join caseInsurer in _caseInsurerRepository.GetAll() on o.RegisterId equals caseInsurer.RegisterId into j3
                                              from s3 in j3.DefaultIfEmpty()
                                              join user in _lookup_userRepository.GetAll() on o.AdjusterId equals user.Id into j4
                                              from s4 in j4.DefaultIfEmpty()
                                              join staff in _staffRepository.GetAll() on o.AdjusterId equals staff.UserId into j5
                                              from s5 in j5.DefaultIfEmpty()

                                              select new InvoiceReportDto
                                              {
                                                  Id = o.Id,
                                                  CaseInvoiceId = o.Id,
                                                  ReportDate = (DateTime)o.InvoiceDate,
                                                  CaseReference = s2.Id,
                                                  InsuranceCompanyId = s2.Company.Id,
                                                  InsuranceCompany = s2.Company.ShortName,
                                                  InsurerRef = s3.ReferenceNo,
                                                  VehicleNo = s2.VehicleNo,
                                                  CaseType = s2.CaseType.ShortName,
                                                  InvService = o.ServiceAmount,
                                                  InvMileage = o.MileageAmount,
                                                  InvPhoto = o.PhotographTotalPrice,
                                                  InvBridge = o.BridgeTollAmount,
                                                  InvPolice = o.PoliceAmount,
                                                  InvStatutory = o.StatutoryDeclarationAmount,
                                                  InvTelco = o.TelcoAmount,
                                                  InvThirdParty = o.ThirdPartyAmount,
                                                  InvSearch = o.SearchFeeAmount,
                                                  InvAir = o.AirFareAmount,
                                                  InvCharteredTransport = o.CharteredAmount,
                                                  InvTaxi = o.TaxiFareAmount,
                                                  InvAccomodation = o.AccommodationAmount,
                                                  InvMiscellaneous = o.MiscAmount,
                                                  InvTotal = o.TotalAmount,
                                                  InvGST = o.AmountWithSST - o.AmountExcludeSST,
                                                  InvNetAmount = (decimal)o.AmountExcludeSST,
                                                  AdjusterId = (int)o.AdjusterId,
                                                  AdjusterName = s4.Name,
                                                  GroupId = (int)s5.GroupId,
                                                  InvChequeNo = o.CheckNo,
                                                  InvoiceRef = o.InvoiceRefNo,
                                                  //InvAmountPaid = (decimal)o.AmountPaid,
                                                  InvDatePaid = o.DatePaid,
                                                  //InvDateSent = o.DateSent,
                                                  //InvCreditDebitDate = o.DebitDate,
                                                  CaseNo = s2.CaseNo,
                                              })
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.InvoiceDate && input.MinReportDateFilter != null, e => e.ReportDate >= input.MinReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.InvoiceDate && input.MaxReportDateFilter != null, e => e.ReportDate <= input.MaxReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.PaidDate && input.MinReportDateFilter != null, e => e.InvDatePaid >= input.MinReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.PaidDate && input.MaxReportDateFilter != null, e => e.InvDatePaid <= input.MaxReportDateFilter)
                        .WhereIf(input.InsuranceCompanyFilter != null, e => e.InsuranceCompanyId == input.InsuranceCompanyFilter)
                        .WhereIf(input.AdjusterIdFilter != null, e => e.AdjusterId == input.AdjusterIdFilter)
                        .WhereIf(input.GroupIdFilter != null, e => e.GroupId == input.GroupIdFilter);
                    break;
                case InvoiceTypeEnum.CreditNote:
                    filteredInvoiceReports = (from o in _caseCreditNoteRepository.GetAll()
                                              join mainRegistration in _mainRegistrationRepository.GetAll() on o.RegisterId equals mainRegistration.Id into j2
                                              from s2 in j2.DefaultIfEmpty()
                                              join caseInsurer in _caseInsurerRepository.GetAll() on o.RegisterId equals caseInsurer.RegisterId into j3
                                              from s3 in j3.DefaultIfEmpty()
                                              join user in _lookup_userRepository.GetAll() on o.AdjusterId equals user.Id into j4
                                              from s4 in j4.DefaultIfEmpty()
                                              join staff in _staffRepository.GetAll() on o.AdjusterId equals staff.UserId into j5
                                              from s5 in j5.DefaultIfEmpty()

                                              select new InvoiceReportDto
                                              {
                                                  Id = o.Id,
                                                  CaseInvoiceId = o.Id,
                                                  ReportDate = (DateTime)o.InvoiceDate,
                                                  CaseReference = s2.Id,
                                                  InsuranceCompanyId = s2.Company.Id,
                                                  InsuranceCompany = s2.Company.ShortName,
                                                  InsurerRef = s3.ReferenceNo,
                                                  VehicleNo = s2.VehicleNo,
                                                  CaseType = s2.CaseType.ShortName,
                                                  InvService = o.ServiceAmount,
                                                  InvMileage = o.MileageAmount,
                                                  InvPhoto = o.PhotographTotalPrice,
                                                  InvBridge = o.BridgeTollAmount,
                                                  InvPolice = o.PoliceAmount,
                                                  InvStatutory = o.StatutoryDeclarationAmount,
                                                  InvTelco = o.TelcoAmount,
                                                  InvThirdParty = o.ThirdPartyAmount,
                                                  InvSearch = o.SearchFeeAmount,
                                                  InvAir = o.AirFareAmount,
                                                  InvCharteredTransport = o.CharteredAmount,
                                                  InvTaxi = o.TaxiFareAmount,
                                                  InvAccomodation = o.AccommodationAmount,
                                                  InvMiscellaneous = o.MiscAmount,
                                                  InvTotal = o.TotalAmount,
                                                  InvGST = o.AmountWithSST - o.AmountExcludeSST,
                                                  InvNetAmount = (decimal)o.AmountExcludeSST,
                                                  AdjusterId = (int)o.AdjusterId,
                                                  AdjusterName = s4.Name,
                                                  GroupId = (int)s5.GroupId,
                                                  InvChequeNo = o.CheckNo,
                                                  InvoiceRef = o.InvoiceRefNo,
                                                  //InvAmountPaid = (decimal)o.AmountPaid,
                                                  //InvDatePaid = o.DatePaid,
                                                  //InvDateSent = o.DateSent,
                                                  InvCreditDebitDate = (DateTime)o.DebitDate,
                                                  CaseNo = s2.CaseNo,
                                              })
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.InvoiceDate && input.MinReportDateFilter != null, e => e.ReportDate >= input.MinReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.InvoiceDate && input.MaxReportDateFilter != null, e => e.ReportDate <= input.MaxReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.PaidDate && input.MinReportDateFilter != null, e => e.InvDatePaid >= input.MinReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.PaidDate && input.MaxReportDateFilter != null, e => e.InvDatePaid <= input.MaxReportDateFilter)
                        .WhereIf(input.InsuranceCompanyFilter != null, e => e.InsuranceCompanyId == input.InsuranceCompanyFilter)
                        .WhereIf(input.AdjusterIdFilter != null, e => e.AdjusterId == input.AdjusterIdFilter)
                        .WhereIf(input.GroupIdFilter != null, e => e.GroupId == input.GroupIdFilter);
                    break;
                case InvoiceTypeEnum.DebitNote:
                    filteredInvoiceReports = (from o in _caseDebitNoteRepository.GetAll()
                                              join mainRegistration in _mainRegistrationRepository.GetAll() on o.RegisterId equals mainRegistration.Id into j2
                                              from s2 in j2.DefaultIfEmpty()
                                              join caseInsurer in _caseInsurerRepository.GetAll() on o.RegisterId equals caseInsurer.RegisterId into j3
                                              from s3 in j3.DefaultIfEmpty()
                                              join user in _lookup_userRepository.GetAll() on o.AdjusterId equals user.Id into j4
                                              from s4 in j4.DefaultIfEmpty()
                                              join staff in _staffRepository.GetAll() on o.AdjusterId equals staff.UserId into j5
                                              from s5 in j5.DefaultIfEmpty()

                                              select new InvoiceReportDto
                                              {
                                                  Id = o.Id,
                                                  CaseInvoiceId = o.Id,
                                                  //ReportDate = (DateTime)o.InvoiceDate,
                                                  CaseReference = s2.Id,
                                                  InsuranceCompanyId = s2.Company.Id,
                                                  InsuranceCompany = s2.Company.ShortName,
                                                  InsurerRef = s3.ReferenceNo,
                                                  VehicleNo = s2.VehicleNo,
                                                  CaseType = s2.CaseType.ShortName,
                                                  InvService = o.ServiceAmount,
                                                  InvMileage = o.MileageAmount,
                                                  InvPhoto = o.PhotographTotalPrice,
                                                  InvBridge = o.BridgeTollAmount,
                                                  InvPolice = o.PoliceAmount,
                                                  InvStatutory = o.StatutoryDeclarationAmount,
                                                  InvTelco = o.TelcoAmount,
                                                  InvThirdParty = o.ThirdPartyAmount,
                                                  InvSearch = o.SearchFeeAmount,
                                                  InvAir = o.AirFareAmount,
                                                  InvCharteredTransport = o.CharteredAmount,
                                                  InvTaxi = o.TaxiFareAmount,
                                                  InvAccomodation = o.AccommodationAmount,
                                                  InvMiscellaneous = o.MiscAmount,
                                                  InvTotal = o.TotalAmount,
                                                  InvGST = o.AmountWithSST - o.AmountExcludeSST,
                                                  InvNetAmount = (decimal)o.AmountExcludeSST,
                                                  AdjusterId = (int)o.AdjusterId,
                                                  AdjusterName = s4.Name,
                                                  GroupId = (int)s5.GroupId,
                                                  InvChequeNo = o.CheckNo,
                                                  InvoiceRef = o.InvoiceRefNo,
                                                  //InvAmountPaid = (decimal)o.AmountPaid,
                                                  //InvDatePaid = o.DatePaid,
                                                  //InvDateSent = o.DateSent,
                                                  InvCreditDebitDate = (DateTime)o.DebitDate,
                                                  CaseNo = s2.CaseNo,
                                              })
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.InvoiceDate && input.MinReportDateFilter != null, e => e.ReportDate >= input.MinReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.InvoiceDate && input.MaxReportDateFilter != null, e => e.ReportDate <= input.MaxReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.PaidDate && input.MinReportDateFilter != null, e => e.InvDatePaid >= input.MinReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.PaidDate && input.MaxReportDateFilter != null, e => e.InvDatePaid <= input.MaxReportDateFilter)
                        .WhereIf(input.InsuranceCompanyFilter != null, e => e.InsuranceCompanyId == input.InsuranceCompanyFilter)
                        .WhereIf(input.AdjusterIdFilter != null, e => e.AdjusterId == input.AdjusterIdFilter)
                        .WhereIf(input.GroupIdFilter != null, e => e.GroupId == input.GroupIdFilter);
                    break;
            }



            var pagedAndFilteredInvoiceReports = filteredInvoiceReports
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredInvoiceReports.CountAsync();

            var dbList = await pagedAndFilteredInvoiceReports.ToListAsync();
            var results = new List<GetInvoiceReportForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetInvoiceReportForViewDto()
                {
                    InvoiceReport = new InvoiceReportDto
                    {
                        ReportDate = o.ReportDate,
                        CaseReference = o.CaseReference,
                        InsuranceCompany = o.InsuranceCompany,
                        InsurerRef = o.InsurerRef,
                        VehicleNo = o.VehicleNo,
                        CaseType = o.CaseType,
                        CaseInvoiceId = o.CaseInvoiceId,
                        InvService = o.InvService,
                        InvMileage = o.InvMileage,
                        InvPhoto = o.InvPhoto,
                        InvToll = o.InvToll,
                        InvBridge = o.InvBridge,
                        InvPolice = o.InvPolice,
                        InvStatutory = o.InvStatutory,
                        InvSurveillance = o.InvSurveillance,
                        InvTelco = o.InvTelco,
                        InvThirdParty = o.InvThirdParty,
                        InvSearch = o.InvSearch,
                        InvAir = o.InvAir,
                        InvCharteredTransport = o.InvCharteredTransport,
                        InvTaxi = o.InvTaxi,
                        InvAccomodation = o.InvAccomodation,
                        InvMiscellaneous = o.InvMiscellaneous,
                        InvTotal = o.InvTotal,
                        InvGST = o.InvGST,
                        InvNetAmount = o.InvNetAmount,
                        AdjusterId = o.AdjusterId,
                        AdjusterName = o.AdjusterName,
                        InvoiceRef = o.InvoiceRef,
                        Id = o.Id,
                        InvChequeNo = o.InvChequeNo,
                        //InvDateSent = o.InvDateSent,
                        InvDatePaid = o.InvDatePaid,
                        InvCreditDebitDate = o.InvCreditDebitDate,
                        InvAmountPaid = o.InvAmountPaid,
                        CaseNo = o.CaseNo,

                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetInvoiceReportForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<FileDto> GetInvoiceReportsToExcel(GetAllInvoiceReportsForExcelInput input)
        {
            IQueryable<InvoiceReportDto> filteredInvoiceReports = Enumerable.Empty<InvoiceReportDto>().AsQueryable();

            switch (input.InvoiceTypeFilter)
            {
                case InvoiceTypeEnum.CaseInvoice:
                    Console.WriteLine("Case Invoice Is called");
                    filteredInvoiceReports = (from o in _caseInvoiceRepository.GetAll()
                                              join mainRegistration in _mainRegistrationRepository.GetAll() on o.RegisterId equals mainRegistration.Id into j2
                                              from s2 in j2.DefaultIfEmpty()
                                              join caseInsurer in _caseInsurerRepository.GetAll() on o.RegisterId equals caseInsurer.RegisterId into j3
                                              from s3 in j3.DefaultIfEmpty()
                                              join user in _lookup_userRepository.GetAll() on o.AdjusterId equals user.Id into j4
                                              from s4 in j4.DefaultIfEmpty()
                                              join staff in _staffRepository.GetAll() on o.AdjusterId equals staff.UserId into j5
                                              from s5 in j5.DefaultIfEmpty()

                                              select new InvoiceReportDto
                                              {
                                                  Id = o.Id,
                                                  CaseInvoiceId = o.Id,
                                                  ReportDate = (DateTime)o.InvoiceDate,
                                                  CaseReference = s2.Id,
                                                  InsuranceCompanyId = s2.Company.Id,
                                                  InsuranceCompany = s2.Company.ShortName,
                                                  InsurerRef = s3.ReferenceNo,
                                                  VehicleNo = s2.VehicleNo,
                                                  CaseType = s2.CaseType.ShortName,
                                                  InvService = o.ServiceAmount,
                                                  InvMileage = o.MileageAmount,
                                                  InvPhoto = o.PhotographTotalPrice,
                                                  InvBridge = o.BridgeTollAmount,
                                                  InvPolice = o.PoliceAmount,
                                                  InvStatutory = o.StatutoryDeclarationAmount,
                                                  InvTelco = o.TelcoAmount,
                                                  InvThirdParty = o.ThirdPartyAmount,
                                                  InvSearch = o.SearchFeeAmount,
                                                  InvAir = o.AirFareAmount,
                                                  InvCharteredTransport = o.CharteredAmount,
                                                  InvTaxi = o.TaxiFareAmount,
                                                  InvAccomodation = o.AccommodationAmount,
                                                  InvMiscellaneous = o.MiscAmount,
                                                  InvTotal = o.TotalAmount,
                                                  InvGST = o.AmountWithSST - o.AmountExcludeSST,
                                                  InvNetAmount = (decimal)o.AmountExcludeSST,
                                                  AdjusterId = (int)o.AdjusterId,
                                                  AdjusterName = s4.Name,
                                                  GroupId = (int)s5.GroupId,
                                                  InvChequeNo = o.CheckNo,
                                                  InvoiceRef = o.InvoiceRefNo,
                                                  //InvAmountPaid = (decimal)o.AmountPaid,
                                                  InvDatePaid = o.DatePaid,
                                                  //InvDateSent = o.DateSent,
                                                  //InvCreditDebitDate = o.DebitDate,
                                                  CaseNo = s2.CaseNo,
                                              })
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.InvoiceDate && input.MinReportDateFilter != null, e => e.ReportDate >= input.MinReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.InvoiceDate && input.MaxReportDateFilter != null, e => e.ReportDate <= input.MaxReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.PaidDate && input.MinReportDateFilter != null, e => e.InvDatePaid >= input.MinReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.PaidDate && input.MaxReportDateFilter != null, e => e.InvDatePaid <= input.MaxReportDateFilter)
                        .WhereIf(input.InsuranceCompanyFilter != null, e => e.InsuranceCompanyId == input.InsuranceCompanyFilter)
                        .WhereIf(input.AdjusterIdFilter != null, e => e.AdjusterId == input.AdjusterIdFilter)
                        .WhereIf(input.GroupIdFilter != null, e => e.GroupId == input.GroupIdFilter);
                    break;
                case InvoiceTypeEnum.CreditNote:
                    Console.WriteLine("Case Credit Note Is called");
                    filteredInvoiceReports = (from o in _caseCreditNoteRepository.GetAll()
                                              join mainRegistration in _mainRegistrationRepository.GetAll() on o.RegisterId equals mainRegistration.Id into j2
                                              from s2 in j2.DefaultIfEmpty()
                                              join caseInsurer in _caseInsurerRepository.GetAll() on o.RegisterId equals caseInsurer.RegisterId into j3
                                              from s3 in j3.DefaultIfEmpty()
                                              join user in _lookup_userRepository.GetAll() on o.AdjusterId equals user.Id into j4
                                              from s4 in j4.DefaultIfEmpty()
                                              join staff in _staffRepository.GetAll() on o.AdjusterId equals staff.UserId into j5
                                              from s5 in j5.DefaultIfEmpty()

                                              select new InvoiceReportDto
                                              {
                                                  Id = o.Id,
                                                  CaseInvoiceId = o.Id,
                                                  ReportDate = (DateTime)o.InvoiceDate,
                                                  CaseReference = s2.Id,
                                                  InsuranceCompanyId = s2.Company.Id,
                                                  InsuranceCompany = s2.Company.ShortName,
                                                  InsurerRef = s3.ReferenceNo,
                                                  VehicleNo = s2.VehicleNo,
                                                  CaseType = s2.CaseType.ShortName,
                                                  InvService = o.ServiceAmount,
                                                  InvMileage = o.MileageAmount,
                                                  InvPhoto = o.PhotographTotalPrice,
                                                  InvBridge = o.BridgeTollAmount,
                                                  InvPolice = o.PoliceAmount,
                                                  InvStatutory = o.StatutoryDeclarationAmount,
                                                  InvTelco = o.TelcoAmount,
                                                  InvThirdParty = o.ThirdPartyAmount,
                                                  InvSearch = o.SearchFeeAmount,
                                                  InvAir = o.AirFareAmount,
                                                  InvCharteredTransport = o.CharteredAmount,
                                                  InvTaxi = o.TaxiFareAmount,
                                                  InvAccomodation = o.AccommodationAmount,
                                                  InvMiscellaneous = o.MiscAmount,
                                                  InvTotal = o.TotalAmount,
                                                  InvGST = o.AmountWithSST - o.AmountExcludeSST,
                                                  InvNetAmount = (decimal)o.AmountExcludeSST,
                                                  AdjusterId = (int)o.AdjusterId,
                                                  AdjusterName = s4.Name,
                                                  GroupId = (int)s5.GroupId,
                                                  InvChequeNo = o.CheckNo,
                                                  InvoiceRef = o.InvoiceRefNo,
                                                  //InvAmountPaid = (decimal)o.AmountPaid,
                                                  //InvDatePaid = o.DatePaid,
                                                  //InvDateSent = o.DateSent,
                                                  InvCreditDebitDate = (DateTime)o.DebitDate,
                                                  CaseNo = s2.CaseNo,
                                              })
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.InvoiceDate && input.MinReportDateFilter != null, e => e.ReportDate >= input.MinReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.InvoiceDate && input.MaxReportDateFilter != null, e => e.ReportDate <= input.MaxReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.PaidDate && input.MinReportDateFilter != null, e => e.InvDatePaid >= input.MinReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.PaidDate && input.MaxReportDateFilter != null, e => e.InvDatePaid <= input.MaxReportDateFilter)
                        .WhereIf(input.InsuranceCompanyFilter != null, e => e.InsuranceCompanyId == input.InsuranceCompanyFilter)
                        .WhereIf(input.AdjusterIdFilter != null, e => e.AdjusterId == input.AdjusterIdFilter)
                        .WhereIf(input.GroupIdFilter != null, e => e.GroupId == input.GroupIdFilter);
                    break;
                case InvoiceTypeEnum.DebitNote:
                    Console.WriteLine("Case Debit Note Is called");
                    filteredInvoiceReports = (from o in _caseDebitNoteRepository.GetAll()
                                              join mainRegistration in _mainRegistrationRepository.GetAll() on o.RegisterId equals mainRegistration.Id into j2
                                              from s2 in j2.DefaultIfEmpty()
                                              join caseInsurer in _caseInsurerRepository.GetAll() on o.RegisterId equals caseInsurer.RegisterId into j3
                                              from s3 in j3.DefaultIfEmpty()
                                              join user in _lookup_userRepository.GetAll() on o.AdjusterId equals user.Id into j4
                                              from s4 in j4.DefaultIfEmpty()
                                              join staff in _staffRepository.GetAll() on o.AdjusterId equals staff.UserId into j5
                                              from s5 in j5.DefaultIfEmpty()

                                              select new InvoiceReportDto
                                              {
                                                  Id = o.Id,
                                                  CaseInvoiceId = o.Id,
                                                  //ReportDate = (DateTime)o.InvoiceDate,
                                                  CaseReference = s2.Id,
                                                  InsuranceCompanyId = s2.Company.Id,
                                                  InsuranceCompany = s2.Company.ShortName,
                                                  InsurerRef = s3.ReferenceNo,
                                                  VehicleNo = s2.VehicleNo,
                                                  CaseType = s2.CaseType.ShortName,
                                                  InvService = o.ServiceAmount,
                                                  InvMileage = o.MileageAmount,
                                                  InvPhoto = o.PhotographTotalPrice,
                                                  InvBridge = o.BridgeTollAmount,
                                                  InvPolice = o.PoliceAmount,
                                                  InvStatutory = o.StatutoryDeclarationAmount,
                                                  InvTelco = o.TelcoAmount,
                                                  InvThirdParty = o.ThirdPartyAmount,
                                                  InvSearch = o.SearchFeeAmount,
                                                  InvAir = o.AirFareAmount,
                                                  InvCharteredTransport = o.CharteredAmount,
                                                  InvTaxi = o.TaxiFareAmount,
                                                  InvAccomodation = o.AccommodationAmount,
                                                  InvMiscellaneous = o.MiscAmount,
                                                  InvTotal = o.TotalAmount,
                                                  InvGST = o.AmountWithSST - o.AmountExcludeSST,
                                                  InvNetAmount = (decimal)o.AmountExcludeSST,
                                                  AdjusterId = (int)o.AdjusterId,
                                                  AdjusterName = s4.Name,
                                                  GroupId = (int)s5.GroupId,
                                                  InvChequeNo = o.CheckNo,
                                                  InvoiceRef = o.InvoiceRefNo,
                                                  //InvAmountPaid = (decimal)o.AmountPaid,
                                                  //InvDatePaid = o.DatePaid,
                                                  //InvDateSent = o.DateSent,
                                                  InvCreditDebitDate = (DateTime)o.DebitDate,
                                                  CaseNo = s2.CaseNo,
                                              })
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.InvoiceDate && input.MinReportDateFilter != null, e => e.ReportDate >= input.MinReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.InvoiceDate && input.MaxReportDateFilter != null, e => e.ReportDate <= input.MaxReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.PaidDate && input.MinReportDateFilter != null, e => e.InvDatePaid >= input.MinReportDateFilter)
                        .WhereIf(input.DateTypeFilter == ReportDateTypeEnum.PaidDate && input.MaxReportDateFilter != null, e => e.InvDatePaid <= input.MaxReportDateFilter)
                        .WhereIf(input.InsuranceCompanyFilter != null, e => e.InsuranceCompanyId == input.InsuranceCompanyFilter)
                        .WhereIf(input.AdjusterIdFilter != null, e => e.AdjusterId == input.AdjusterIdFilter)
                        .WhereIf(input.GroupIdFilter != null, e => e.GroupId == input.GroupIdFilter);
                    break;
            }

            var totalCount = await filteredInvoiceReports.CountAsync();

            var dbList = await filteredInvoiceReports.ToListAsync();

            var results = await filteredInvoiceReports
                .Select(o => new GetInvoiceReportForViewDto
                {
                    InvoiceReport = new InvoiceReportDto
                    {
                        ReportDate = o.ReportDate,
                        CaseReference = o.CaseReference,
                        InsuranceCompany = o.InsuranceCompany,
                        InsurerRef = o.InsurerRef,
                        VehicleNo = o.VehicleNo,
                        CaseType = o.CaseType,
                        CaseInvoiceId = o.CaseInvoiceId,
                        InvService = o.InvService,
                        InvServiceSST = o.InvServiceSST,

                        InvMileage = o.InvMileage,
                        InvMileageSST = o.InvMileageSST,

                        InvPhoto = o.InvPhoto,
                        InvPhotoSST = o.InvPhotoSST,

                        InvToll = o.InvToll,
                        InvTollSST = o.InvTollSST,

                        InvBridge = o.InvBridge,
                        InvBridgeSST = o.InvBridgeSST,

                        InvPolice = o.InvPolice,
                        InvPoliceSST = o.InvPoliceSST,

                        InvStatutory = o.InvStatutory,
                        InvStatutorySST = o.InvStatutorySST,

                        InvSurveillance = o.InvSurveillance,
                        InvSurveillanceSST = o.InvSurveillanceSST,

                        InvTelco = o.InvTelco,
                        InvTelcoSST = o.InvTelcoSST,

                        InvThirdParty = o.InvThirdParty,
                        InvThirdPartySST = o.InvThirdPartySST,

                        InvSearch = o.InvSearch,
                        InvSearchSST = o.InvSearchSST,

                        InvAir = o.InvAir,
                        InvAirSST = o.InvAirSST,

                        InvCharteredTransport = o.InvCharteredTransport,
                        InvCharteredTransportSST = o.InvCharteredTransportSST,

                        InvTaxi = o.InvTaxi,
                        InvTaxiSST = o.InvTaxiSST,

                        InvAccomodation = o.InvAccomodation,
                        InvAccomodationSST = o.InvAccomodationSST,

                        InvMiscellaneous = o.InvMiscellaneous,
                        InvMiscellaneousSST = o.InvMiscellaneousSST,
                        InvTotal = o.InvTotal,
                        InvGST = o.InvGST,
                        InvNetAmount = o.InvNetAmount,
                        AdjusterId = o.AdjusterId,
                        AdjusterName = o.AdjusterName,
                        InvoiceRef = o.InvoiceRef,
                        Id = o.Id,
                        InvChequeNo = o.InvChequeNo,
                        //InvDateSent = o.InvDateSent,
                        //InvDatePaid = o.InvDatePaid,
                        InvCreditDebitDate = o.InvCreditDebitDate,
                        //InvAmountPaid = o.InvAmountPaid,
                        CaseNo = o.CaseNo,

                    }
                }).ToListAsync();

            return _invoiceReportsExcelExporter.ExportToFile(results);
        }

    }
}