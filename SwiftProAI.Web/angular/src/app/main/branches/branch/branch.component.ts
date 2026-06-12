import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BranchServiceProxy, BranchDto } from '@shared/service-proxies/service-proxies';
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
import { CreateOrEditBranchModalComponent } from './create-or-edit-branch-modal.component';
import { ViewBranchModalComponent } from './view-branch-modal.component';

@Component({
    templateUrl: './branch.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class BranchComponent extends AppComponentBase {
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('createEditModal', { static: true }) createEditModal: CreateOrEditBranchModalComponent;
    @ViewChild('viewModal', { static: false }) viewModal: ViewBranchModalComponent;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    shortNameFilter = '';

    ngOnInit(): void {
        this.createEditModal.modalSave.subscribe(() => {
            console.log('modalSave event received');
            this.reloadPage();
        });
    }
    constructor(
        injector: Injector,
        private _branchServiceProxy: BranchServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _router: Router,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
    }

    getBranch(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._branchServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.shortNameFilter,
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

    createBranch(): void {
        this.createEditModal.show();
    }

    viewBranch(branch: number): void {
        this.viewModal.show(branch);
    }

    editBranch(branch: number): void {
        this.createEditModal.show(branch);
    }

    deleteBranch(branch: BranchDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._branchServiceProxy.delete(branch.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.shortNameFilter = '';

        this.getBranch();
    }
}
