import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { LawFirmsServiceProxy, CreateOrEditLawFirmDto, CreateOrEditViewThirdPartyCaseRequestDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditLawFirmModal',
    templateUrl: './create-or-edit-lawFirm-modal.component.html'
})
export class CreateOrEditLawFirmModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    isHideRemark = true;
    isApproved = false;
    isCancelled = false;

    lawFirm: CreateOrEditLawFirmDto = new CreateOrEditLawFirmDto();
    viewThirdPartyCaseRequest: CreateOrEditViewThirdPartyCaseRequestDto = new CreateOrEditViewThirdPartyCaseRequestDto();
    defaultAllowCompanyToViewAssignedCases = false; 


    constructor(
        injector: Injector,
        private _lawFirmsServiceProxy: LawFirmsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    toggleAllowCompanyToViewAssignedCases(e){
        if(!e.target.checked && this.isApproved){
            this.swalAlert.fire({
                title: this.l("RemoveViewPermisson"),
                icon: "warning",
                showConfirmButton: true,
            });
        }
        
        if(e.target.checked){
            this.isHideRemark = true;

            if(this.lawFirm.viewThirdPartyCaseRequest){
                this.lawFirm.viewThirdPartyCaseRequest.cancelRemark = "";
            }
        } else{
            this.isHideRemark = false;
        }
    }
    
    show(lawFirmId?: number): void {
    

        if (!lawFirmId) {
            this.lawFirm = new CreateOrEditLawFirmDto();
            this.lawFirm.id = lawFirmId;


            this.active = true;
            this.modal.show();
        } else {
            this._lawFirmsServiceProxy.getLawFirmForEdit(lawFirmId).subscribe(result => {
                this.lawFirm = result.lawFirm;
                this.defaultAllowCompanyToViewAssignedCases = result.lawFirm.allowToViewAssignedCases;
                this.isApproved = result.lawFirm.viewThirdPartyCaseRequest ? result.lawFirm.viewThirdPartyCaseRequest.status == 'Approved' : false;
                this.isCancelled = result.lawFirm.viewThirdPartyCaseRequest ? result.lawFirm.viewThirdPartyCaseRequest.status == 'Cancelled' : false;
                this.isHideRemark = result.lawFirm.viewThirdPartyCaseRequest ? !result.lawFirm.viewThirdPartyCaseRequest.cancelRemark : true;
                this.viewThirdPartyCaseRequest = result.lawFirm.viewThirdPartyCaseRequest;

                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
        if(this.lawFirm.allowToViewAssignedCases && !this.lawFirm.businessRegistrationNo){
            this.swalAlert.fire({
                title: this.l("CompanyRegNumNotFound"),
                icon: "warning",
                showConfirmButton: true,
            });
            
            return;
        }

        // if(this.defaultAllowCompanyToViewAssignedCases && !this.lawFirm.allowToViewAssignedCases && !this.viewThirdPartyCaseRequest.cancelRemark){
        //     this.isHideRemark = false;
        //     return;
        // }

        this.lawFirm.viewThirdPartyCaseRequest = this.viewThirdPartyCaseRequest;

        this.saving = true;
			
        this._lawFirmsServiceProxy.createOrEdit(this.lawFirm)
            .pipe(finalize(() => { this.saving = false;}))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.isHideRemark = true;
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
