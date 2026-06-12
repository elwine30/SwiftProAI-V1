import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetAuditTrailForViewDto, AuditTrailDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewAuditTrailModal',
    templateUrl: './view-auditTrail-modal.component.html'
})
export class ViewAuditTrailModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetAuditTrailForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetAuditTrailForViewDto();
        this.item.auditTrail = new AuditTrailDto();
    }

    show(item: GetAuditTrailForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
