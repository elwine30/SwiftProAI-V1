import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { GroupsServiceProxy, GroupDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditGroupModalComponent } from './create-or-edit-group-modal.component';

import { ViewGroupModalComponent } from './view-group-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { EnumGroupType } from '@app/shared/common/registration/enum';


@Component({
    templateUrl: './groups.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class GroupsComponent extends AppComponentBase {
    
    @ViewChild('createOrEditGroupModal', { static: true }) createOrEditGroupModal: CreateOrEditGroupModalComponent;
    @ViewChild('viewGroupModal', { static: true }) viewGroupModal: ViewGroupModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    groupTypeFilter = -1;
    isActiveFilter = -1;
    branchNameFilter = '';
    allGroupTypes: {id: number, displayName: string}[] = [];					


    constructor(
        injector: Injector,
        private _groupsServiceProxy: GroupsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.allGroupTypes = Object.keys(EnumGroupType)
            .filter(key => isNaN(Number(key)))
            .map(key => ({id: EnumGroupType[key], displayName: key}));
        
    }

    getGroups(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._groupsServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.groupTypeFilter,
            this.isActiveFilter,
            this.branchNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    getGroupTypeDisplayName(groupType: number): string {
        return EnumGroupType[groupType];
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createGroup(): void {
        this.createOrEditGroupModal.show();        
    }

    deleteGroup(group: GroupDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._groupsServiceProxy.delete(group.id)
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
        this.nameFilter = '';
        this.groupTypeFilter = -1;
        this.isActiveFilter = -1;
		this.branchNameFilter = '';
					
        this.getGroups();
    }
}
