import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { WorkshopsServiceProxy, WorkshopDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditWorkshopModalComponent } from './create-or-edit-workshop-modal.component';

import { ViewWorkshopModalComponent } from './view-workshop-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    templateUrl: './workshops.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class WorkshopsComponent extends AppComponentBase {
    
    
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('createOrEditWorkshopModal', { static: true }) createOrEditWorkshopModal: CreateOrEditWorkshopModalComponent;
    @ViewChild('viewWorkshopModal', { static: true }) viewWorkshopModal: ViewWorkshopModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    

    advancedFiltersAreShown = false;
    filterText = '';
    workshopNoFilter = '';
    workshopNameFilter = '';
    addressFilter = '';
    maxClaimRateFilter : number;
		maxClaimRateFilterEmpty : number;
		minClaimRateFilter : number;
		minClaimRateFilterEmpty : number;
    isActiveFilter = -1;


    _entityTypeFullName = 'ThinknInsurTech.Workshops.Workshop';
    entityHistoryEnabled = false;




    constructor(
        injector: Injector,
        private _workshopsServiceProxy: WorkshopsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
        
    }

    ngOnInit(): void {
        this.entityHistoryEnabled = this.setIsEntityHistoryEnabled();
    }

    private setIsEntityHistoryEnabled(): boolean {
        let customSettings = (abp as any).custom;
        return this.isGrantedAny('Pages.Administration.AuditLogs') && customSettings.EntityHistory && customSettings.EntityHistory.isEnabled && _filter(customSettings.EntityHistory.enabledEntities, entityType => entityType === this._entityTypeFullName).length === 1;
    }

    getWorkshops(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._workshopsServiceProxy.getAll(
            this.filterText,
            this.workshopNoFilter,
            this.workshopNameFilter,
            this.addressFilter,
            this.maxClaimRateFilter == null ? this.maxClaimRateFilterEmpty: this.maxClaimRateFilter,
            this.minClaimRateFilter == null ? this.minClaimRateFilterEmpty: this.minClaimRateFilter,
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

    createWorkshop(): void {
        this.createOrEditWorkshopModal.show();        
    }


    showHistory(workshop: WorkshopDto): void {
        this.entityTypeHistoryModal.show({
            entityId: workshop.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteWorkshop(workshop: WorkshopDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._workshopsServiceProxy.delete(workshop.id)
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
            this.workshopNoFilter = '';
    this.workshopNameFilter = '';
    this.addressFilter = '';
    this.maxClaimRateFilter = this.maxClaimRateFilterEmpty;
		this.minClaimRateFilter = this.maxClaimRateFilterEmpty;
    this.isActiveFilter = -1;

        this.getWorkshops();
    }
}
