import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetWorkshopForViewDto, WorkshopDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewWorkshopModal',
    templateUrl: './view-workshop-modal.component.html'
})
export class ViewWorkshopModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetWorkshopForViewDto;
    remark;

    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetWorkshopForViewDto();
        this.item.workshop = new WorkshopDto();
    }

    show(item: GetWorkshopForViewDto): void {
        this.item = item;
        this.remark = item.workshop.viewThirdPartyCaseRequest ? item.workshop.viewThirdPartyCaseRequest.cancelRemark : "";
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
