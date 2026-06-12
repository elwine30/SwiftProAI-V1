import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetOpenAIIntegrationLogForViewDto, OpenAIIntegrationLogDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOpenAIIntegrationLogModal',
    templateUrl: './view-openAIIntegrationLog-modal.component.html'
})
export class ViewOpenAIIntegrationLogModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOpenAIIntegrationLogForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetOpenAIIntegrationLogForViewDto();
        this.item.openAIIntegrationLog = new OpenAIIntegrationLogDto();
    }

    show(item: GetOpenAIIntegrationLogForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
