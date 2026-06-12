import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ScopeAssignmentsServiceProxy, ScopeAssignmentDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditScopeAssignmentModalComponent } from './create-or-edit-scopeAssignment-modal.component';

import { ViewScopeAssignmentModalComponent } from './view-scopeAssignment-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    templateUrl: './scopeAssignments.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ScopeAssignmentsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditScopeAssignmentModal', { static: true }) createOrEditScopeAssignmentModal: CreateOrEditScopeAssignmentModalComponent;
    @ViewChild('viewScopeAssignmentModal', { static: true }) viewScopeAssignmentModal: ViewScopeAssignmentModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    

    advancedFiltersAreShown = false;
    filterText = '';
    descriptionFilter = '';
    isActiveFilter = -1;







    constructor(
        injector: Injector,
        private _scopeAssignmentsServiceProxy: ScopeAssignmentsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
        
    }

    getScopeAssignments(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._scopeAssignmentsServiceProxy.getAll(
            this.filterText,
            this.descriptionFilter,
            this.isActiveFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createScopeAssignment(): void {
        this.createOrEditScopeAssignmentModal.show();        
    }


    deleteScopeAssignment(scopeAssignment: ScopeAssignmentDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._scopeAssignmentsServiceProxy.delete(scopeAssignment.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
    
    
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.descriptionFilter = '';
    this.isActiveFilter = -1;

        this.getScopeAssignments();
    }
}
