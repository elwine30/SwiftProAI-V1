import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CompanyServiceProxy, CreateOrEditCompanyDto ,CompanyCaseTypeLookupTableDto, CreateOrEditViewThirdPartyCaseRequestDto
					} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';




@Component({
    selector: 'createOrEditCompanyModal',
    templateUrl: './create-or-edit-company-modal.component.html'
})
export class CreateOrEditCompanyModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    isHideRemark = true;
    isApproved = false;
    isCancelled = false;

    company: CreateOrEditCompanyDto = new CreateOrEditCompanyDto();
    viewThirdPartyCaseRequest: CreateOrEditViewThirdPartyCaseRequestDto = new CreateOrEditViewThirdPartyCaseRequestDto();
    defaultAllowCompanyToViewAssignedCases = false; 
    caseTypeDescription = '';

	allCaseTypes: CompanyCaseTypeLookupTableDto[];
					

    constructor(
        injector: Injector,
        private _companiesServiceProxy: CompanyServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    toggleAllowCompanyToViewAssignedCases(e){
        // user is allowed to untick the checkbox if requested is approved with notification
        if(!e.target.checked && this.isApproved){
            this.swalAlert.fire({
                title: this.l("RemoveViewPermisson"),
                icon: "warning",
                showConfirmButton: true,
            });
        }

        if(e.target.checked){
            this.isHideRemark = true;

            if(this.company.viewThirdPartyCaseRequest){
                this.company.viewThirdPartyCaseRequest.cancelRemark = "";
            }
        } else{
            this.isHideRemark = false;
        }
    }
    
    show(companyId?: number): void {
        if (!companyId) {
            this.company = new CreateOrEditCompanyDto();
            this.company.id = companyId;
            this.caseTypeDescription = '';
            this.isHideRemark = true;

            this.active = true;
            this.modal.show();
        } else {
            this._companiesServiceProxy.getCompanyForEdit(companyId).subscribe(result => {
                this.company = result.company;
                this.defaultAllowCompanyToViewAssignedCases = result.company.allowToViewAssignedCases;
                this.isApproved = result.company.viewThirdPartyCaseRequest ? result.company.viewThirdPartyCaseRequest.status == 'Approved' : false;
                this.isCancelled = result.company.viewThirdPartyCaseRequest ? result.company.viewThirdPartyCaseRequest.status == 'Cancelled' : false;
                this.isHideRemark = result.company.viewThirdPartyCaseRequest ? !result.company.viewThirdPartyCaseRequest.cancelRemark : true;
                this.viewThirdPartyCaseRequest = result.company.viewThirdPartyCaseRequest;
                this.caseTypeDescription = result.caseTypeDescription;

                this.active = true;
                this.modal.show();
            });
        }
        this._companiesServiceProxy.getAllCaseTypeForTableDropdown().subscribe(result => {						
						this.allCaseTypes = result;
					});
					
        
    }

    save(): void {
        if(this.company.allowToViewAssignedCases && !this.company.businessRegistrationNo){
            this.swalAlert.fire({
                title: this.l("CompanyRegNumNotFound"),
                icon: "warning",
                showConfirmButton: true,
            });
            
            return;
        }


        this.company.viewThirdPartyCaseRequest = this.viewThirdPartyCaseRequest;

        this.saving = true;
            this._companiesServiceProxy.createOrEdit(this.company)
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
