import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CaseSearchFeesServiceProxy, CreateOrEditCaseSearchFeeDto 
					} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';



@Component({
    selector: 'createOrEditCaseSearchFeeModal',
    templateUrl: './create-or-edit-caseSearchFee-modal.component.html'
})
export class CreateOrEditCaseSearchFeeModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    registerId: number;

    caseSearchFee: CreateOrEditCaseSearchFeeDto = new CreateOrEditCaseSearchFeeDto();

    mainRegistrationVehicleNo = '';					
    
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseSearchFeesServiceProxy: CaseSearchFeesServiceProxy,
    ) {
        super(injector);
    }
    
    show(caseSearchFeeId?: number): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        if (!caseSearchFeeId) {
            this.caseSearchFee = new CreateOrEditCaseSearchFeeDto();
            this.caseSearchFee.id = caseSearchFeeId;
            this.caseSearchFee.registerId = this.registerId;

            this.active = true;
            this.modal.show();
        } else {
            this._caseSearchFeesServiceProxy.getCaseSearchFeeForEdit(caseSearchFeeId).subscribe(result => {
                this.caseSearchFee = result.caseSearchFee;
                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;
            this._caseSearchFeesServiceProxy.createOrEdit(this.caseSearchFee)
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
