import { Component, Injector, ViewEncapsulation, ViewChild, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseSearchFeesServiceProxy, CaseSearchFeeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditCaseSearchFeeModalComponent } from './create-or-edit-caseSearchFee-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { filter as _filter } from 'lodash-es';
import { CaseClaimDataService } from '../caseClaims/caseClaimDataService';

@Component({
    selector: 'caseSearchFeesTable',
    templateUrl: './caseSearchFees.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class CaseSearchFeesComponent extends AppComponentBase {
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('createOrEditCaseSearchFeeModal', { static: true })
    createOrEditCaseSearchFeeModal: CreateOrEditCaseSearchFeeModalComponent;
    @Output() tableChange: EventEmitter<any> = new EventEmitter<any>();


    advancedFiltersAreShown = false;
    registerIdFilter = '';
    statusId: number | null = null;

    constructor(
        injector: Injector,
        private _caseSearchFeesServiceProxy: CaseSearchFeesServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _caseClaimDataService: CaseClaimDataService,
    ) {
        super(injector);
        this._caseClaimDataService.currentStatusId.subscribe((statusId) => (this.statusId = statusId));
    }

    getCaseSearchFees(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this.registerIdFilter = this._activatedRoute.snapshot.queryParams['id'];
        this._caseSearchFeesServiceProxy
            .getAll(
                this.registerIdFilter,
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

    getTotalSearchFee(): number {
        let total = 0;
        this.primengTableHelper.records.forEach((record) => {
            total += record.caseSearchFee.amount;
        });
        return total;
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createCaseSearchFee(): void {
        this.createOrEditCaseSearchFeeModal.show();
    }

    emitTableChange(): void {
        this.getCaseSearchFees();
        this.tableChange.emit(null);
    }

    deleteCaseSearchFee(caseSearchFee: CaseSearchFeeDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._caseSearchFeesServiceProxy.delete(caseSearchFee.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.tableChange.emit(null);
                });
            }
        });
    }
}
