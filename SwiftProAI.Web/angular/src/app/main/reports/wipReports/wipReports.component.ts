import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WIPReportsServiceProxy, WIPReportDto, StaffGroupLookupTableDto, StaffsServiceProxy, CaseLawyersServiceProxy, CaseLawyerLawFirmLookupTableDto, CaseAdjusterLookupTableDto, CommonDropdownServiceProxy, CommonAdjusterDropdownDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

import { HttpClient } from '@angular/common/http';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs';

@Component({
    templateUrl: './wipReports.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class WIPReportsComponent extends AppComponentBase implements OnInit{
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    allGroups: StaffGroupLookupTableDto[];
    allLawFirms : CaseLawyerLawFirmLookupTableDto[];
    allAdjuster : CaseAdjusterLookupTableDto[];
    allCompany : CommonAdjusterDropdownDto[];

    advancedFiltersAreShown = false;
    filterText = '';
    reportDateRangeFilter: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    maxReportDateFilter: DateTime;
    minReportDateFilter: DateTime;
    insuranceCompanyFilter: number;
    adjusterIDFilter: number;
    lawyerCompanyFilter: number;
    groupFilter : number;

    uploadUrl: string;

    constructor(
        injector: Injector,
        private _wipReportsServiceProxy: WIPReportsServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _staffsServiceProxy: StaffsServiceProxy,
        private _caseLawyersServiceProxy: CaseLawyersServiceProxy,
        private _CommonDropdownService: CommonDropdownServiceProxy,
    ) {
        super(injector);

        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/WIPReports/ImportFromExcel';
    }

    ngOnInit(): void {
        //TODO : Call one time to BE
        this._CommonDropdownService.getAllLawFirmForTableDropdown().subscribe(result=>{
            this.allLawFirms = result;
        })
        this._CommonDropdownService.getAllGroupForTableDropdown().subscribe(result=>{
            this.allGroups = result;
        })
        this._CommonDropdownService.getAllAdjusterForTableDropdown(undefined).subscribe(result=>{
            this.allAdjuster = result;
        })
        this._CommonDropdownService.getAllCompanyForTableDropdown().subscribe(result=>{
            this.allCompany = result;
        })
    }

    getWIPReports(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }
        this.primengTableHelper.showLoadingIndicator();
        this._wipReportsServiceProxy
            .getAll(
                this.filterText,
                this.maxReportDateFilter === undefined
                    ? this.reportDateRangeFilter[1]
                    : this._dateTimeService.getEndOfDayForDate(this.maxReportDateFilter),
                this.minReportDateFilter === undefined
                    ? this.reportDateRangeFilter[0]
                    : this._dateTimeService.getStartOfDayForDate(this.minReportDateFilter),
                this.insuranceCompanyFilter,
                this.lawyerCompanyFilter,
                this.adjusterIDFilter,
                this.groupFilter,

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
        this._wipReportsServiceProxy
            .getWIPReportsToExcel(
                this.filterText,
                this.maxReportDateFilter === undefined
                ? this.reportDateRangeFilter[1]
                : this._dateTimeService.getEndOfDayForDate(this.maxReportDateFilter),
                this.minReportDateFilter === undefined
                ? this.reportDateRangeFilter[0]
                : this._dateTimeService.getStartOfDayForDate(this.minReportDateFilter),
                this.insuranceCompanyFilter,
                this.lawyerCompanyFilter,
                this.adjusterIDFilter,
                this.groupFilter,
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxReportDateFilter = undefined;
        this.minReportDateFilter = undefined;
        this.insuranceCompanyFilter = undefined;
        this.adjusterIDFilter = undefined;
        this.lawyerCompanyFilter = undefined;
        this.groupFilter = undefined;

        this.getWIPReports();
    }

    searchReport(): void {
        this.getWIPReports();
    }
}
