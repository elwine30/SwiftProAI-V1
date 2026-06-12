import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { DeclarationQuestionsServiceProxy, CreateOrEditDeclarationQuestionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FormArray, FormControl } from '@angular/forms';

@Component({
    selector: 'createOrEditDeclarationQuestionModal',
    templateUrl: './create-or-edit-declarationQuestion-modal.component.html'
})
export class CreateOrEditDeclarationQuestionModalComponent extends AppComponentBase implements OnInit {
    options = new FormArray([]);
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    declarationQuestion: CreateOrEditDeclarationQuestionDto = new CreateOrEditDeclarationQuestionDto();

    optionTypes = [ 
        { value: 'SINGLE_LINE', display: 'Single Line Input' },
        { value: 'RADIO', display: 'Radio Buttons' },
        { value: 'CHECKBOX', display: 'Multi-select Checkbox' },   
    ];

    constructor(
        injector: Injector,
        private _declarationQuestionsServiceProxy: DeclarationQuestionsServiceProxy
    ) {
        super(injector);
    }

    show(declarationQuestionId?: number): void {
        if (!declarationQuestionId) {
            this.declarationQuestion = new CreateOrEditDeclarationQuestionDto();
            this.declarationQuestion.id = declarationQuestionId;
            this.active = true;
            this.modal.show();
        } else {
            this._declarationQuestionsServiceProxy.getDeclarationQuestionForEdit(declarationQuestionId).subscribe(result => {
                this.declarationQuestion = result.declarationQuestion;
                this.populateOptions(result.declarationQuestion.optionValues);
                this.active = true;
                this.modal.show();
            });
        }
    }

    populateOptions(optionValues: string): void {
        this.options.clear();
        if (optionValues) {
            const optionsArray = optionValues.split(',');
            optionsArray.forEach(option => this.options.push(new FormControl(option.trim())));
        }
    }

    addOption(option: string): void {
        if (option) {
            this.options.push(new FormControl(option));
        }
    }

    removeOption(index: number): void {
        this.options.removeAt(index);
    }

    save(): void {
        this.saving = true;
        if (this.declarationQuestion.optionType=="SINGLE_LINE"){
            this.declarationQuestion.optionValues = null;
        } else {
            this.declarationQuestion.optionValues = this.options.controls.map(control => control.value).join(',');
        }

        this._declarationQuestionsServiceProxy.createOrEdit(this.declarationQuestion)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void { }
}
