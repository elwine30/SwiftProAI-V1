import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { finalize } from 'rxjs/operators';
import { CaseDeclarationAnswersServiceProxy, CreateOrEditCaseDeclarationAnswerDto} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NgForm } from '@angular/forms';

@Component({
    selector: 'createOrEditCaseDeclarationAnswerModal',
    templateUrl: './create-or-edit-caseDeclarationAnswer-modal.component.html',
})
export class CreateOrEditCaseDeclarationAnswerModalComponent extends AppComponentBase {
    
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('caseDeclarationForm') caseDeclarationForm: NgForm;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    
    active = false;
    saving = false;
    
    caseDeclarationList: any[] = [];
    caseDeclarationAnswerList: CreateOrEditCaseDeclarationAnswerDto[] = [];

    constructor(
        injector: Injector,     
        private _caseDeclarationAnswersServiceProxy: CaseDeclarationAnswersServiceProxy,
    ) {
        super(injector);
    }


    show(registerId?: number): void {
        this.caseDeclarationAnswerList = [];
        this._caseDeclarationAnswersServiceProxy.getCaseDeclarationAnswerForEdit(registerId).subscribe(result => {
            this.caseDeclarationList = result.declarationQuestionAnswerList;
            if(this.caseDeclarationList) {
                this.caseDeclarationList.forEach(item => {
                    let caseDeclarationAnswer = new CreateOrEditCaseDeclarationAnswerDto();
                    if (item.answerId){
                        caseDeclarationAnswer.id = item.answerId;
                        caseDeclarationAnswer.answer = item.answer;
                    } 
                    caseDeclarationAnswer.registerId = registerId;
                    caseDeclarationAnswer.questionId = item.questionId;
                    this.caseDeclarationAnswerList.push(caseDeclarationAnswer);
                });
            }
            this.modal.show();
            this.active = true;
        });
        
    }

    updateAnswerForCheckboxInput(index: number, option: string, checked: boolean): void {
        let answerArray = this.caseDeclarationAnswerList[index].answer ? this.caseDeclarationAnswerList[index].answer.split(',') : [];
        if (checked) {
          answerArray.push(option);
        } else {
          const optionIndex = answerArray.indexOf(option);
          if (optionIndex > -1) {
            answerArray.splice(optionIndex, 1);
          }
        }
        this.caseDeclarationAnswerList[index].answer = answerArray.join(',');
      }

    isOptionChecked(index: number, option: string): boolean {
    return this.caseDeclarationAnswerList[index].answer ? this.caseDeclarationAnswerList[index].answer.split(',').includes(option) : false;
    }
    
    save(): void {
        this.saving = true;
        this._caseDeclarationAnswersServiceProxy.createOrEdit(this.caseDeclarationAnswerList)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {           
                 this.notify.info(this.l('SavedSuccessfully'));
                    this.markFormAsPristine(this.caseDeclarationForm);
                    this.close();
            })
    }

    close(): void {
        if(!this.caseDeclarationForm.dirty) {
            this.active = false;
            this.modal.hide();
        } else {
            confirm('Are you sure you want to leave? Your changes will be lost.') ? this.modal.hide() : '';
        }
    }
    
}
