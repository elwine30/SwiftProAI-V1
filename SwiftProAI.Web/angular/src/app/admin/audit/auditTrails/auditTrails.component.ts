import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuditTrailsServiceProxy, AuditTrailDto, AuditEntriesServiceProxy, CommonDropdownServiceProxy, CommonDropdownDto } from '@shared/service-proxies/service-proxies';
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


@Component({
    templateUrl: './auditTrails.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class AuditTrailsComponent extends AppComponentBase implements OnInit {


    // @ViewChild('createOrEditAuditTrailModal', { static: true }) createOrEditAuditTrailModal: CreateOrEditAuditTrailModalComponent;
    // @ViewChild('viewAuditTrailModal', { static: true }) viewAuditTrailModal: ViewAuditTrailModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;


    advancedFiltersAreShown = false;
    filterText = '';
    organizationUnitIdFilter :number;
    operationFilter = '';
    tableNameFilter = '';
    changedByFilter = '';
    allOrganizationUnit : CommonDropdownDto[];

    constructor(
        injector: Injector,
        private _auditTrailsServiceProxy: AuditTrailsServiceProxy,
        private _auditEntriesServiceProxy: AuditEntriesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _commonDropdownService : CommonDropdownServiceProxy,
    ) {
        super(injector);

    }
    ngOnInit(): void {
        this._commonDropdownService.getAllOrganizationUnitForDropdown().subscribe(result=>{
            this.allOrganizationUnit = result;
        })
    }

    getAuditTrails(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._auditEntriesServiceProxy.getAll(
            this.filterText,
            this.tableNameFilter,
            this.organizationUnitIdFilter,
            this.changedByFilter,
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            console.log(result);
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    exportToExcel(): void {
        this._auditEntriesServiceProxy.getAuditEntriesToExcel(
            this.filterText,
            this.tableNameFilter
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.tableNameFilter = '';
        this.organizationUnitIdFilter = undefined;

        this.getAuditTrails();
    }
}
