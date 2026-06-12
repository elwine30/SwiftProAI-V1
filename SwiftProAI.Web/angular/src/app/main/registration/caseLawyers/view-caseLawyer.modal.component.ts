
import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CaseLawyerDto, CaseThirdPartyVehicleDto, GetCaseLawyerForViewDto, GetCaseThirdPartyVehicleForViewDto, GetVehicleForViewDto, VehicleDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewcaseLawyerModal',
    templateUrl: './view-caseLawyer.modal.component.html'
})
export class ViewcaseLawyerModalComponent extends AppComponentBase {

    @ViewChild('viewModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCaseLawyerForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetCaseLawyerForViewDto();
        this.item.caseLawyer = new CaseLawyerDto();
    }

    show(item: GetCaseLawyerForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}