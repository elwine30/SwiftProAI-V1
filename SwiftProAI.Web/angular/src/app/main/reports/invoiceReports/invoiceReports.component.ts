import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    InvoiceReportsServiceProxy,
    InvoiceReportDto,
    CompanyCaseTypeLookupTableDto,
    CommonDropdownServiceProxy,
    CommonDropdownDto,
    CommonAdjusterDropdownDto,
    InvoiceTypeEnum,
    ReportDateTypeEnum,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';

import { ViewInvoiceReportModalComponent } from './view-invoiceReport-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter, result } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './invoiceReports.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class InvoiceReportsComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewInvoiceReportModal', { static: true }) viewInvoiceReportModal: ViewInvoiceReportModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    companies: CommonDropdownDto[];
    adjusters: CommonDropdownDto[];
    allGroups: CommonDropdownDto[];
    //invoiceTypes: CommonDropdownDto[];
    invoiceTypes = [
        { id: 1, displayName: 'Invoice' },
        { id: 2, displayName: 'Credit Invoice' },
        { id: 3, displayName: 'Debit Invoice' },
    ];
    dateTypes = [
        { id: 1, displayName: 'Invoice Date' },
        { id: 2, displayName: 'Paid Date' },
    ];
    advancedFiltersAreShown = false;
    filterText = '';
    reportDateRangeFilter: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    maxReportDateFilter: DateTime;
    minReportDateFilter: DateTime;
    insuranceCompanyFilter: number;
    AdjusterIdFilter: number;
    groupIdFilter: number;
    invoiceTypeFilter: number;
    dateTypeFilter: number;

    constructor(
        injector: Injector,
        private _invoiceReportsServiceProxy: InvoiceReportsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _CommonDropdownService: CommonDropdownServiceProxy,
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this._CommonDropdownService.getAllCompanyForTableDropdown().subscribe((result) => {
            this.companies = result;
        });
        this._CommonDropdownService.getAllAdjusterForTableDropdown(undefined).subscribe((result) => {
            this.adjusters = result;
        });
        this._CommonDropdownService.getAllGroupForTableDropdown().subscribe((result) => {
            this.allGroups = result;
        });
        // this._CommonDropdownService.getAllInvoiceTypeForTableDropdown().subscribe(result=>{
        //     this.invoiceTypes = result;
        // })
        this.invoiceTypeFilter = InvoiceTypeEnum.CaseInvoice;
    }

    getInvoiceReports(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._invoiceReportsServiceProxy
            .getAll(
                this.dateTypeFilter == null ? undefined: this.dateTypeFilter,
                this.maxReportDateFilter === undefined
                    ? this.reportDateRangeFilter[1]
                    : this._dateTimeService.getEndOfDayForDate(this.maxReportDateFilter),
                this.minReportDateFilter === undefined
                    ? this.reportDateRangeFilter[0]
                    : this._dateTimeService.getEndOfDayForDate(this.minReportDateFilter),
                this.insuranceCompanyFilter == null ? undefined : this.insuranceCompanyFilter,
                this.AdjusterIdFilter == null ? undefined : this.AdjusterIdFilter,
                this.invoiceTypeFilter == null ? undefined : this.invoiceTypeFilter,
                this.groupIdFilter == null ? undefined : this.groupIdFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event),
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    exportToExcel(): void {
        this._invoiceReportsServiceProxy
            .getInvoiceReportsToExcel(
                this.dateTypeFilter == null ? undefined: this.dateTypeFilter,
                this.maxReportDateFilter === undefined
                    ? this.reportDateRangeFilter[1]
                    : this._dateTimeService.getEndOfDayForDate(this.maxReportDateFilter),
                this.minReportDateFilter === undefined
                    ? this.reportDateRangeFilter[0]
                    : this._dateTimeService.getEndOfDayForDate(this.minReportDateFilter),
                this.insuranceCompanyFilter == null ? undefined : this.insuranceCompanyFilter,
                this.AdjusterIdFilter == null ? undefined : this.AdjusterIdFilter,
                this.invoiceTypeFilter == null ? undefined : this.invoiceTypeFilter,
                this.groupIdFilter == null ? undefined : this.groupIdFilter,
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.dateTypeFilter = undefined;
        this.groupIdFilter = undefined;
        this.invoiceTypeFilter = InvoiceTypeEnum.CaseInvoice;
        this.maxReportDateFilter = undefined;
        this.minReportDateFilter = undefined;
        this.insuranceCompanyFilter = undefined;
        this.AdjusterIdFilter = undefined;

        this.getInvoiceReports();
    }
}
