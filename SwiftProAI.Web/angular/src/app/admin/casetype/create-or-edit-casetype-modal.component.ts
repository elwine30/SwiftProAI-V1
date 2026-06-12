import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CaseTypeServiceProxy, CreateOrEditCaseTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';




@Component({
    selector: 'createOrEditCasetypeModal',
    templateUrl: './create-or-edit-casetype-modal.component.html'
})
export class CreateOrEditCasetypeModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;


    casetype: CreateOrEditCaseTypeDto = new CreateOrEditCaseTypeDto();


    constructor(
        injector: Injector,
        private _caseTypeServiceProxy: CaseTypeServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(casetypeId?: number): void {


        if (!casetypeId) {
            this.casetype = new CreateOrEditCaseTypeDto();
            this.casetype.id = casetypeId;


            this.active = true;
            this.modal.show();
        } else {
            this._caseTypeServiceProxy.getCasetypeForEdit(casetypeId).subscribe(result => {
                this.casetype = result.caseType;

                this.active = true;
                this.modal.show();
            });
        }


    }

    save(): void {
        this.saving = true;



        this._caseTypeServiceProxy.createOrEdit(this.casetype)
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
