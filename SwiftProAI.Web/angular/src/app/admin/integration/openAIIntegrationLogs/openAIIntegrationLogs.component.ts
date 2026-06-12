import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    OpenAIIntegrationLogsServiceProxy,
    OpenAIIntegrationLogDto,
    OrganizationUnitServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';

import { ViewOpenAIIntegrationLogModalComponent } from './view-openAIIntegrationLog-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

@Component({
    templateUrl: './openAIIntegrationLogs.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class OpenAIIntegrationLogsComponent extends AppComponentBase {
    @ViewChild('viewOpenAIIntegrationLogModal', { static: true })
    viewOpenAIIntegrationLogModal: ViewOpenAIIntegrationLogModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    actionUrlFilter = '';
    requestFilter = '';
    responseFilter = '';
    maxPromptTokenFilter: number;
    maxPromptTokenFilterEmpty: number;
    minPromptTokenFilter: number;
    minPromptTokenFilterEmpty: number;
    maxCompletionTokenFilter: number;
    maxCompletionTokenFilterEmpty: number;
    minCompletionTokenFilter: number;
    minCompletionTokenFilterEmpty: number;
    maxTotalCostFilter: number;
    maxTotalCostFilterEmpty: number;
    minTotalCostFilter: number;
    minTotalCostFilterEmpty: number;
    caseNoFilter = '';
    organizationUnitId: number = 0;

    assignmentDateRange: Date[];

    organizationUnitList: any[];

    onDropdownChange(event: any): void {
        this.organizationUnitId = +event.target.value;
    }
    constructor(
        injector: Injector,
        private _openAIIntegrationLogsServiceProxy: OpenAIIntegrationLogsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _organizationUnitService: OrganizationUnitServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit() {
        this._organizationUnitService.getAll().subscribe((result) => {
            this.organizationUnitList = result;
            console.log(this.organizationUnitList);
        });
    }

    getOpenAIIntegrationLogs(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }
        const startDate =
            this.assignmentDateRange && this.assignmentDateRange[0]
                ? DateTime.fromJSDate(this.assignmentDateRange[0])
                : undefined;
        const endDate =
            this.assignmentDateRange && this.assignmentDateRange[1]
                ? DateTime.fromJSDate(this.assignmentDateRange[1])
                : undefined;

        this.primengTableHelper.showLoadingIndicator();

        this._openAIIntegrationLogsServiceProxy
            .getAll(
                this.filterText,
                this.actionUrlFilter,
                this.requestFilter,
                this.responseFilter,
                this.maxPromptTokenFilter == null ? this.maxPromptTokenFilterEmpty : this.maxPromptTokenFilter,
                this.minPromptTokenFilter == null ? this.minPromptTokenFilterEmpty : this.minPromptTokenFilter,
                this.maxCompletionTokenFilter == null
                    ? this.maxCompletionTokenFilterEmpty
                    : this.maxCompletionTokenFilter,
                this.minCompletionTokenFilter == null
                    ? this.minCompletionTokenFilterEmpty
                    : this.minCompletionTokenFilter,
                this.maxTotalCostFilter == null ? this.maxTotalCostFilterEmpty : this.maxTotalCostFilter,
                this.minTotalCostFilter == null ? this.minTotalCostFilterEmpty : this.minTotalCostFilter,
                this.caseNoFilter,
                this.organizationUnitId,
                startDate,
                endDate,
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

    resetFilters(): void {
        this.filterText = '';
        this.actionUrlFilter = '';
        this.requestFilter = '';
        this.responseFilter = '';
        this.maxPromptTokenFilter = this.maxPromptTokenFilterEmpty;
        this.minPromptTokenFilter = this.maxPromptTokenFilterEmpty;
        this.maxCompletionTokenFilter = this.maxCompletionTokenFilterEmpty;
        this.minCompletionTokenFilter = this.maxCompletionTokenFilterEmpty;
        this.maxTotalCostFilter = this.maxTotalCostFilterEmpty;
        this.minTotalCostFilter = this.maxTotalCostFilterEmpty;
        this.caseNoFilter = '';

        this.getOpenAIIntegrationLogs();
    }
}
