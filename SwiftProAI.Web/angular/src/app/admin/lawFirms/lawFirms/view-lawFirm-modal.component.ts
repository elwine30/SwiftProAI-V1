import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetLawFirmForViewDto, LawFirmDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewLawFirmModal',
    templateUrl: './view-lawFirm-modal.component.html'
})
export class ViewLawFirmModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetLawFirmForViewDto;
    remark;

    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetLawFirmForViewDto();
        this.item.lawFirm = new LawFirmDto();
    }

    show(item: GetLawFirmForViewDto): void {
        this.item = item;
        this.remark = item.lawFirm.viewThirdPartyCaseRequest ? item.lawFirm.viewThirdPartyCaseRequest.cancelRemark : "";
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
