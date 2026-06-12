import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetVehicleForViewDto, VehicleDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewVehicleModal',
    templateUrl: './view-vehicle-modal.component.html'
})
export class ViewVehicleModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetVehicleForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetVehicleForViewDto();
        this.item.vehicle = new VehicleDto();
    }

    show(item: GetVehicleForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
