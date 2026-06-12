import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { DebitNoteItemsServiceProxy, DebitNoteItemDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditDebitNoteItemModalComponent } from './create-or-edit-debitNoteItem-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'debitNoteItemsTable',
    templateUrl: './debitNoteItems.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class DebitNoteItemsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditDebitNoteItemModal', { static: true }) createOrEditDebitNoteItemModal: CreateOrEditDebitNoteItemModalComponent;    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
    @Input() itemType: string;
    @Output() tableChange: EventEmitter<any> = new EventEmitter<any>();

    advancedFiltersAreShown = false;
    registerIdFilter = '';
    filterText = '';
    itemTypeFilter = '';

    constructor(
        injector: Injector,
        private _debitNoteItemsServiceProxy: DebitNoteItemsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        
    }

    getDebitNoteItems(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.itemTypeFilter = this.itemType;
        this.primengTableHelper.showLoadingIndicator();
        this.registerIdFilter = this._activatedRoute.snapshot.queryParams['id'];

        this._debitNoteItemsServiceProxy.getAll(
            this.registerIdFilter,
            this.itemTypeFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            20
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createDebitNoteItem(itemType: string): void {
        this.createOrEditDebitNoteItemModal.show(itemType);        
    }

    emitTableChange(): void {
        this.getDebitNoteItems();
        this.tableChange.emit(null);
    }


    deleteDebitNoteItem(debitNoteItem: DebitNoteItemDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._debitNoteItemsServiceProxy.delete(debitNoteItem.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                            this.tableChange.emit(null);
                        });
                }
            }
        );
    }
    

}
