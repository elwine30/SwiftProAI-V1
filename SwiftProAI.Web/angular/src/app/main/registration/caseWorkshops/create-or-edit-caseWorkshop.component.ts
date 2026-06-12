import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { finalize } from 'rxjs/operators';
import { CaseWorkshopsServiceProxy, CreateOrEditCaseWorkshopDto ,CaseWorkshopWorkshopLookupTableDto
					} from '@shared/service-proxies/service-proxies';
import { WorkshopsServiceProxy, WorkshopDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StepNavComponent } from '@app/shared/common/registration/step-nav.component';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import { CreateOrEditCaseInsurerComponent } from '../caseInsurers/create-or-edit-caseInsurer.component';
import { NgForm } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';




@Component({
    selector : 'createCaseWorkshop',
    templateUrl: './create-or-edit-caseWorkshop.component.html',
    animations: [appModuleAnimation()]
})
export class CreateOrEditCaseWorkshopComponent extends AppComponentBase implements OnInit {
    
    active = false;
    saving = false;
    isHidden = AppConsts.isComponentDisabled;
    @ViewChild(StepNavComponent) stepNav: StepNavComponent;
    @ViewChild(CreateOrEditCaseInsurerComponent) createCaseInsurer: CreateOrEditCaseInsurerComponent;
    @ViewChild('caseWorkshopForm') caseWorkshopForm: NgForm;
    
    caseWorkshop: CreateOrEditCaseWorkshopDto = new CreateOrEditCaseWorkshopDto();

    caseWorkshopName = '';
    caseWorkshopAddress = '';

	allWorkshops: CaseWorkshopWorkshopLookupTableDto[];


    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,        
        private _caseWorkshopsServiceProxy: CaseWorkshopsServiceProxy,
        private _workshopsServiceProxy: WorkshopsServiceProxy,
        private _router: Router,
        public navigationService: NavigationService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngAfterViewInit(): void {
        if(AppConsts.isComponentDisabled)
        {
            setTimeout(() => {
                if (this.caseWorkshopForm && this.caseWorkshopForm.controls) {
                    this.disableForm();
                }
            });
        }
        
    }
    
    disableForm(): void {
        if (this.caseWorkshopForm && this.caseWorkshopForm.controls) {
            Object.keys(this.caseWorkshopForm.controls).forEach(controlName => {
                const control = this.caseWorkshopForm.controls[controlName];
                control.disable();
            });
        } else {
          console.log("Form or controls not available");
        }
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(registerId?: number): void {

        this._caseWorkshopsServiceProxy.getCaseWorkshopForEdit(registerId).subscribe(result => {
            this.caseWorkshop = result.caseWorkshop;
            if (!this.caseWorkshop) {
                this.caseWorkshop = new CreateOrEditCaseWorkshopDto();
                this.caseWorkshopName = '';
                this.caseWorkshopAddress = '';
            }
            else {
                this.caseWorkshopName = result.workshopWorkshopName;
 
                this._workshopsServiceProxy.getWorkshopForView(result.caseWorkshop.workshopId).subscribe(w => {						
                    this.caseWorkshopAddress = w.workshop.address;
                });
            }
 
            this.caseWorkshop.registerId = registerId;
            this.active = true;
        });
        this._caseWorkshopsServiceProxy.getAllWorkshopForTableDropdown().subscribe(result => {						
						this.allWorkshops = result;
					});
    }

    updateWorkshopAddress(workshopId: number): void {
        if (workshopId) {
            this._workshopsServiceProxy.getWorkshopForView(workshopId).subscribe(w => {
                this.caseWorkshopAddress = w.workshop.address;
            });
        } else {
            this.caseWorkshopAddress = "";
        }
    }
    
    save(): void {
        this.saving = true;
        this.createCaseInsurer.save();
        this._caseWorkshopsServiceProxy.createOrEdit(this.caseWorkshop)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {
                 this.saving = false;               
                 this.notify.info(this.l('SavedSuccessfully'));

                 this.markFormAsPristine(this.caseWorkshopForm);
            })
    }

}
