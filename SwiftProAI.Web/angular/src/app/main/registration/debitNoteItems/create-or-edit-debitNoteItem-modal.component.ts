import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { DebitNoteItemsServiceProxy, CreateOrEditDebitNoteItemDto ,DebitNoteItemMainRegistrationLookupTableDto
					} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ActivatedRoute } from '@angular/router';



@Component({
    selector: 'createOrEditDebitNoteItemModal',
    templateUrl: './create-or-edit-debitNoteItem-modal.component.html'
})

export class CreateOrEditDebitNoteItemModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    registerId: number;

    debitNoteItem: CreateOrEditDebitNoteItemDto = new CreateOrEditDebitNoteItemDto();

    mainRegistrationVehicleNo = '';					

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _debitNoteItemsServiceProxy: DebitNoteItemsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(itemType?: string, debitNoteItemId?: number): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        if (!debitNoteItemId) {
            this.debitNoteItem = new CreateOrEditDebitNoteItemDto();
            this.debitNoteItem.id = debitNoteItemId;
            this.debitNoteItem.registerId = this.registerId;
            if (itemType != null){
                this.debitNoteItem.itemType = itemType;
            }

            this.active = true;
            this.modal.show();
        } else {
            this._debitNoteItemsServiceProxy.getDebitNoteItemForEdit(debitNoteItemId).subscribe(result => {
                this.debitNoteItem = result.debitNoteItem;
                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
        this.saving = true;		
        this._debitNoteItemsServiceProxy.createOrEdit(this.debitNoteItem)
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
