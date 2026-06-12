import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseReportsServiceProxy, CaseReportDto } from '@shared/service-proxies/service-proxies';
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

@Component({
    templateUrl: './caseReports.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class CaseReportsComponent extends AppComponentBase {
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    reportFilter = '';
    reportTypeFilter = '';
    reportDateRangeFilter: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    maxMonthRangeFilter: DateTime;
    minMonthRangeFilter: DateTime;
    dateRangeExceedsLimit: boolean = false;

    columnHeaders: string[] = [];
    rowHeaders: string[] = [];
    pivotData: { [key: string]: { [key: string]: number } } = {};

    constructor(
        injector: Injector,
        private _caseReportsServiceProxy: CaseReportsServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
    }

    getCaseReports(event?: LazyLoadEvent) {
        console.log('searching');
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }
        this.validateDateRange();
        if (this.dateRangeExceedsLimit) {
            this.notify.error(this.l('Date range should not exceed 12 months.'));
            return;
        }
        this.primengTableHelper.showLoadingIndicator();
        this._caseReportsServiceProxy
            .getAll(
                this.reportFilter === '' ? (this.reportFilter = 'insuranceCompany') : this.reportFilter,
                this.reportTypeFilter === '' ? (this.reportTypeFilter = 'caseDetails') : this.reportTypeFilter,
                this.maxMonthRangeFilter === undefined
                    ? this.reportDateRangeFilter[1]
                    : this._dateTimeService.getEndOfDayForDate(this.maxMonthRangeFilter),
                this.minMonthRangeFilter === undefined
                    ? this.reportDateRangeFilter[0]
                    : this._dateTimeService.getStartOfDayForDate(this.minMonthRangeFilter),
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event),
            )
            .subscribe((result) => {
                this.columnHeaders = result.columnHeader;
                this.rowHeaders = result.rowHeader;
                this.pivotData = result.reportData;
                const recordsArray = [];
                for (const columnKey in this.pivotData) {
                    if (this.pivotData.hasOwnProperty(columnKey)) {
                        const rowData = { column: columnKey };
                        for (const rowKey in this.pivotData[columnKey]) {
                            if (this.pivotData[columnKey].hasOwnProperty(rowKey)) {
                                rowData[rowKey] = this.pivotData[columnKey][rowKey];
                            }
                        }
                        recordsArray.push(rowData);
                    }
                }
                this.primengTableHelper.records = recordsArray;
                this.primengTableHelper.totalRecordsCount =recordsArray.length;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    getCellData(row: any, column: string): number {
        return row[column] !== undefined ? row[column] : 0;
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    exportToExcel(): void {
        this._caseReportsServiceProxy
            .getCaseReportsToExcel(
                this.reportFilter === '' ? (this.reportFilter = 'insuranceCompany') : this.reportFilter,
                this.reportTypeFilter === '' ? (this.reportTypeFilter = 'caseDetails') : this.reportTypeFilter,
                    this.maxMonthRangeFilter === undefined
                    ? this.reportDateRangeFilter[1]
                    : this._dateTimeService.getEndOfDayForDate(this.maxMonthRangeFilter),
                this.minMonthRangeFilter === undefined
                    ? this.reportDateRangeFilter[0]
                    : this._dateTimeService.getEndOfDayForDate(this.minMonthRangeFilter),
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    validateDateRange() {
        if (this.minMonthRangeFilter && this.maxMonthRangeFilter) {
            const startDate = new Date(this.minMonthRangeFilter.toString());
            const endDate = new Date(this.maxMonthRangeFilter.toString());

            // Calculate the difference in months between the start and end dates
            const diffMonths =
                (endDate.getFullYear() - startDate.getFullYear()) * 12 + (endDate.getMonth() - startDate.getMonth());

            // Check if the date range exceeds the limit
            this.dateRangeExceedsLimit = diffMonths > 6;
        }
    }

    searchReport(): void {
        this.getCaseReports();
    }

    resetFilters(): void {
        this.reportFilter = '';
        this.reportTypeFilter = '';
        this.maxMonthRangeFilter = undefined;
        this.minMonthRangeFilter = undefined;
        this.getCaseReports();
    }
}
