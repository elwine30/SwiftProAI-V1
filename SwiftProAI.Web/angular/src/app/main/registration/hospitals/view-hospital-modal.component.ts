import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHospitalForViewDto, HospitalDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHospitalModal',
    templateUrl: './view-hospital-modal.component.html'
})
export class ViewHospitalModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHospitalForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetHospitalForViewDto();
        this.item.hospital = new HospitalDto();
    }

    show(item: GetHospitalForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
