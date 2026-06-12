import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { InvoiceItemsServiceProxy, CreateOrEditInvoiceItemDto ,InvoiceItemMainRegistrationLookupTableDto
					} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ActivatedRoute } from '@angular/router';



@Component({
    selector: 'createOrEditInvoiceItemModal',
    templateUrl: './create-or-edit-invoiceItem-modal.component.html'
})
export class CreateOrEditInvoiceItemModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    registerId: number;

    invoiceItem: CreateOrEditInvoiceItemDto = new CreateOrEditInvoiceItemDto();

    mainRegistrationVehicleNo = '';					

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _invoiceItemsServiceProxy: InvoiceItemsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(itemType?: string, invoiceItemId?: number): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        if (!invoiceItemId) {
            this.invoiceItem = new CreateOrEditInvoiceItemDto();
            this.invoiceItem.id = invoiceItemId;
            this.invoiceItem.registerId = this.registerId;
            if (itemType != null){
                this.invoiceItem.itemType = itemType;
            }

            this.active = true;
            this.modal.show();
        } else {
            this._invoiceItemsServiceProxy.getInvoiceItemForEdit(invoiceItemId).subscribe(result => {
                this.invoiceItem = result.invoiceItem;
                this.invoiceItem.amount = parseFloat(this.invoiceItem.amount.toFixed(2));
                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
        this.saving = true;		
        this._invoiceItemsServiceProxy.createOrEdit(this.invoiceItem)
            .pipe(finalize(() => { this.saving = false;}))
            .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.close();
            this.modalSave.emit(null);
            });
    }



    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     } 

}
