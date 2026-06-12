import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetDeclarationQuestionForViewDto, DeclarationQuestionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewDeclarationQuestionModal',
    templateUrl: './view-declarationQuestion-modal.component.html'
})
export class ViewDeclarationQuestionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetDeclarationQuestionForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetDeclarationQuestionForViewDto();
        this.item.declarationQuestion = new DeclarationQuestionDto();
    }

    show(item: GetDeclarationQuestionForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
