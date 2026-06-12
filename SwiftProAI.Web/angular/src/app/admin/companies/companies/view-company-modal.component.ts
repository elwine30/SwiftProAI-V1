import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetCompanyForViewDto, CompanyDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewCompanyModal',
    templateUrl: './view-company-modal.component.html'
})
export class ViewCompanyModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCompanyForViewDto;
    remark;

    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetCompanyForViewDto();
        this.item.company = new CompanyDto();
    }

    show(item: GetCompanyForViewDto): void {
        this.item = item;
        this.remark = item.company.viewThirdPartyCaseRequest ? item.company.viewThirdPartyCaseRequest.cancelRemark : "";
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
