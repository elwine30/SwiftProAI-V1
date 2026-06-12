import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { AuditTrailsServiceProxy, CreateOrEditAuditTrailDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';




@Component({
    selector: 'createOrEditAuditTrailModal',
    templateUrl: './create-or-edit-auditTrail-modal.component.html'
})
export class CreateOrEditAuditTrailModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;


    auditTrail: CreateOrEditAuditTrailDto = new CreateOrEditAuditTrailDto();




    constructor(
        injector: Injector,
        private _auditTrailsServiceProxy: AuditTrailsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(auditTrailId?: number): void {


        if (!auditTrailId) {
            this.auditTrail = new CreateOrEditAuditTrailDto();
            this.auditTrail.id = auditTrailId;
            this.auditTrail.changedDate = this._dateTimeService.getStartOfDay();


            this.active = true;
            this.modal.show();
        } else {
            this._auditTrailsServiceProxy.getAuditTrailForEdit(auditTrailId).subscribe(result => {
                this.auditTrail = result.auditTrail;



                this.active = true;
                this.modal.show();
            });
        }


    }

    save(): void {
        this.saving = true;



        this._auditTrailsServiceProxy.createOrEdit(this.auditTrail)
            .pipe(finalize(() => { this.saving = false; }))
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
