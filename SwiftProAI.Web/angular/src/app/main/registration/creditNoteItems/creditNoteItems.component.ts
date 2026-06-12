import { Component, Injector, ViewEncapsulation, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { CreditNoteItemsServiceProxy, CreditNoteItemDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditCreditNoteItemModalComponent } from './create-or-edit-creditNoteItem-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'creditNoteItemsTable',
    templateUrl: './creditNoteItems.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CreditNoteItemsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditCreditNoteItemModal', { static: true }) createOrEditCreditNoteItemModal: CreateOrEditCreditNoteItemModalComponent;    
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
        private _creditNoteItemsServiceProxy: CreditNoteItemsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        
    }

    getCreditNoteItems(event?: LazyLoadEvent) {
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

        this._creditNoteItemsServiceProxy.getAll(
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

    createCreditNoteItem(itemType: string): void {
        this.createOrEditCreditNoteItemModal.show(itemType);        
    }

    emitTableChange(): void {
        this.getCreditNoteItems();
        this.tableChange.emit(null);
    }


    deleteCreditNoteItem(creditNoteItem: CreditNoteItemDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._creditNoteItemsServiceProxy.delete(creditNoteItem.id)
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
