import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { PromptsServiceProxy, CreateOrEditPromptDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';




@Component({
    selector: 'createOrEditPromptModal',
    templateUrl: './create-or-edit-prompt-modal.component.html'
})
export class CreateOrEditPromptModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    

    prompt: CreateOrEditPromptDto = new CreateOrEditPromptDto();




    constructor(
        injector: Injector,
        private _promptsServiceProxy: PromptsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(promptId?: number): void {
    

        if (!promptId) {
            this.prompt = new CreateOrEditPromptDto();
            this.prompt.id = promptId;


            this.active = true;
            this.modal.show();
        } else {
            this._promptsServiceProxy.getPromptForEdit(promptId).subscribe(result => {
                this.prompt = result.prompt;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._promptsServiceProxy.createOrEdit(this.prompt)
             .pipe(finalize(() => { this.saving = false;}))
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
    
     ngOnInit(): void {
        
     }    
}
