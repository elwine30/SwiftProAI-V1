import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { WorkshopsServiceProxy, CreateOrEditWorkshopDto, CreateOrEditViewThirdPartyCaseDto, CreateOrEditViewThirdPartyCaseRequestDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';




@Component({
    selector: 'createOrEditWorkshopModal',
    templateUrl: './create-or-edit-workshop-modal.component.html'
})
export class CreateOrEditWorkshopModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    isHideRemark = true;
    isApproved = false;
    isCancelled = false;

    workshop: CreateOrEditWorkshopDto = new CreateOrEditWorkshopDto();
    viewThirdPartyCaseRequest: CreateOrEditViewThirdPartyCaseRequestDto = new CreateOrEditViewThirdPartyCaseRequestDto();
    defaultAllowCompanyToViewAssignedCases = false; 



    constructor(
        injector: Injector,
        private _workshopsServiceProxy: WorkshopsServiceProxy,
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

            if(this.workshop.viewThirdPartyCaseRequest){
                this.workshop.viewThirdPartyCaseRequest.cancelRemark = "";
            }
        } else{
            this.isHideRemark = false;
        }
    }
    
    show(workshopId?: number): void {
    

        if (!workshopId) {
            this.workshop = new CreateOrEditWorkshopDto();
            this.workshop.id = workshopId;


            this.active = true;
            this.modal.show();
        } else {
            this._workshopsServiceProxy.getWorkshopForEdit(workshopId).subscribe(result => {
                this.workshop = result.workshop;
                this.defaultAllowCompanyToViewAssignedCases = result.workshop.allowToViewAssignedCases;
                this.isApproved = result.workshop.viewThirdPartyCaseRequest ? result.workshop.viewThirdPartyCaseRequest.status == 'Approved' : false;
                this.isCancelled = result.workshop.viewThirdPartyCaseRequest ? result.workshop.viewThirdPartyCaseRequest.status == 'Cancelled' : false;
                this.isHideRemark = result.workshop.viewThirdPartyCaseRequest ? !result.workshop.viewThirdPartyCaseRequest.cancelRemark : true;
                this.viewThirdPartyCaseRequest = result.workshop.viewThirdPartyCaseRequest;

                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            if(this.workshop.allowToViewAssignedCases && !this.workshop.businessRegistrationNo){
                this.swalAlert.fire({
                    title: this.l("CompanyRegNumNotFound"),
                    icon: "warning",
                    showConfirmButton: true,
                });
                
                return;
            }

            // if(this.defaultAllowCompanyToViewAssignedCases && !this.workshop.allowToViewAssignedCases && !this.viewThirdPartyCaseRequest.cancelRemark){
            //     this.isHideRemark = false;
            //     return;
            // }

            this.workshop.viewThirdPartyCaseRequest = this.viewThirdPartyCaseRequest;
			
            this.saving = true;

            this._workshopsServiceProxy.createOrEdit(this.workshop)
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
