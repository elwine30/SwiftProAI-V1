import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CreditNoteItemsServiceProxy, CreateOrEditCreditNoteItemDto ,CreditNoteItemMainRegistrationLookupTableDto
					} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';



@Component({
    selector: 'createOrEditCreditNoteItemModal',
    templateUrl: './create-or-edit-creditNoteItem-modal.component.html'
})

export class CreateOrEditCreditNoteItemModalComponent extends AppComponentBase{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    registerId: number;

    creditNoteItem: CreateOrEditCreditNoteItemDto = new CreateOrEditCreditNoteItemDto();

    mainRegistrationVehicleNo = '';					

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _creditNoteItemsServiceProxy: CreditNoteItemsServiceProxy
    ) {
        super(injector);
    }
    
    show(itemType?: string, creditNoteItemId?: number): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        if (!creditNoteItemId) {
            this.creditNoteItem = new CreateOrEditCreditNoteItemDto();
            this.creditNoteItem.id = creditNoteItemId;
            this.creditNoteItem.registerId = this.registerId;
            if (itemType != null){
                this.creditNoteItem.itemType = itemType;
            }

            this.active = true;
            this.modal.show();
        } else {
            this._creditNoteItemsServiceProxy.getCreditNoteItemForEdit(creditNoteItemId).subscribe(result => {
                this.creditNoteItem = result.creditNoteItem;
                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
        this.saving = true;		
        this._creditNoteItemsServiceProxy.createOrEdit(this.creditNoteItem)
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
}
