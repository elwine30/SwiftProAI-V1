import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CaseThirdPartyVehicleDto, GetCaseThirdPartyVehicleForViewDto, GetVehicleForViewDto, VehicleDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewcaseThirdPartyVehicleModal',
    templateUrl: './view-caseThirdPartyVehicle.modal.component.html'
})
export class ViewcaseThirdPartyVehicleModalComponent extends AppComponentBase {

    @ViewChild('viewModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCaseThirdPartyVehicleForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetCaseThirdPartyVehicleForViewDto();
        this.item.caseThirdPartyVehicle = new CaseThirdPartyVehicleDto();
    }

    show(item: GetCaseThirdPartyVehicleForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
