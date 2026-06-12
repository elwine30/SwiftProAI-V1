import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetCaseTypeForViewDto, CaseTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewCasetypeModal',
    templateUrl: './view-casetype-modal.component.html'
})
export class ViewCasetypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCaseTypeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetCaseTypeForViewDto();
        this.item.caseType = new CaseTypeDto();
    }

    show(item: GetCaseTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
