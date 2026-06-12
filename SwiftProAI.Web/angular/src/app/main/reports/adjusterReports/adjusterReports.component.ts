import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { AdjusterReportsServiceProxy, CommonDropdownServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
    templateUrl: './adjusterReports.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class AdjusterReportsComponent extends AppComponentBase implements OnInit {
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    monthFilter: DateTime;
    bsConfigMonth: Partial<BsDatepickerConfig>;
    bsConfigYear: Partial<BsDatepickerConfig>;
    yearFilter: DateTime;
    userIdFilter: number;
    userIdFilterEmpty: number;
    userList: any[] = [];

    constructor(
        injector: Injector,
        private _CommonDropdownService: CommonDropdownServiceProxy,
        private _adjusterReportsServiceProxy: AdjusterReportsServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.bsConfigMonth = {
            dateInputFormat: 'MMMM YYYY', // Format to display
            minMode: 'month', // Only show months and years
            adaptivePosition: true // Adjust position to fit within the viewport
          };
          this.bsConfigYear = {
            dateInputFormat: 'YYYY', // Format to display
            minMode: 'year', // Only show years
            adaptivePosition: true // Adjust position to fit within the viewport
          };
        this._CommonDropdownService.getAllAdjusterForTableDropdown(undefined).subscribe(result=>{
            this.userList = result;
        })
    }

    onMonthFilterChange() {
        //Set the year filter to be equal as monthFilter because backend will handle getting the month and year
        this.yearFilter = this.monthFilter;
    }

    onYearFilterChange() {
        //Reset the monthFilter so you won't have conflicts like monthFilter: June 2023 and year filter 2025
        this.monthFilter = undefined;
    }

    getAdjusterReports(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }
        this.primengTableHelper.showLoadingIndicator();
        this._adjusterReportsServiceProxy
            .getAll(
                this.monthFilter === undefined
                    ? this.monthFilter
                    : this._dateTimeService.getEndOfDayForDate(this.monthFilter),
                this.yearFilter === undefined
                    ? this.yearFilter
                    : this._dateTimeService.getStartOfDayForDate(this.yearFilter),
                this.userIdFilter == null ? this.userIdFilterEmpty : this.userIdFilter,
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
        this._adjusterReportsServiceProxy
            .getAdjusterReportsToExcel(
                this.monthFilter === undefined
                    ? this.monthFilter
                    : this._dateTimeService.getEndOfDayForDate(this.monthFilter),
                this.yearFilter === undefined
                    ? this.yearFilter
                    : this._dateTimeService.getStartOfDayForDate(this.yearFilter),
                this.userIdFilter == null ? this.userIdFilterEmpty : this.userIdFilter,
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.monthFilter = undefined;
        this.yearFilter = undefined;
        this.userIdFilter = this.userIdFilterEmpty;
        this.getAdjusterReports();
    }
}
