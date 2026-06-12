import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CaseInvoicesServiceProxy, CreateOrEditCaseInvoiceDto, LookupsServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'paymentUpdateModal',
    templateUrl: './payment-update-modal.component.html',
})
export class PaymentUpdateModalComponent extends AppComponentBase implements OnInit {
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    @ViewChild('createModal', { static: true }) modal: ModalDirective;

    payment: CreateOrEditCaseInvoiceDto = new CreateOrEditCaseInvoiceDto();

    active: boolean = false;
    saving: boolean = false;

    paymentModeList: any;
    registerId: number;

    constructor(
        injector: Injector,
        private _caseInvoiceService: CaseInvoicesServiceProxy,
        private _lookupService: LookupsServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
    }

    show(id?: number): void {
        this.registerId = id;

        this._lookupService.getByGroup('PaymentMode')
        .subscribe((result) => {
            this.paymentModeList = result;
        });

        this._caseInvoiceService.getCaseInvoiceForEdit(this.registerId)
        .subscribe((result) => {
            this.payment = result.caseInvoice;
            //Auto Populate the amount paid from caseInvoice. This does not save value, only populate field.
            this.payment.amountPaid = result.caseInvoice.totalAmount;
        });

        this.active = true;
        this.modal.show();
    }

    save(): void {
        this.saving = true;

        this._caseInvoiceService.createOrEdit(this.payment)
            .pipe(finalize(() => { this.saving = false;}))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.payment = new CreateOrEditCaseInvoiceDto();
                this.modalSave.emit(null);
            })
        
        this.close();
    }

    onShown(): void {
    }

    close(): void {
        this.modal.hide();
        this.active = false;
    }
}