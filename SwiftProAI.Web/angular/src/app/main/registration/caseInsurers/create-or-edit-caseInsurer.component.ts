import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    CaseInsurersServiceProxy, CreateOrEditCaseInsurerDto,
    CommonDropdownServiceProxy,
    CommonDropdownDto
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { NgForm } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';




@Component({
    selector: 'createCaseInsurer',
    templateUrl: './create-or-edit-caseInsurer.component.html',
    animations: [appModuleAnimation()]
})
export class CreateOrEditCaseInsurerComponent extends AppComponentBase implements OnInit {
    @ViewChild('caseInsurerForm') caseInsurerForm: NgForm;

    active = false;
    saving = false;

    caseInsurer: CreateOrEditCaseInsurerDto = new CreateOrEditCaseInsurerDto();

    companyName = '';

    allCompanys: CommonDropdownDto[];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseInsurersServiceProxy: CaseInsurersServiceProxy,
        private _CommonDropdownService: CommonDropdownServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngAfterViewInit(): void {
        if(AppConsts.isComponentDisabled)
        {
            setTimeout(() => {
                if (this.caseInsurerForm && this.caseInsurerForm.controls) {
                    this.disableForm();
                }
            });
        }
        
    }
    
    disableForm(): void {
        if (this.caseInsurerForm && this.caseInsurerForm.controls) {
            Object.keys(this.caseInsurerForm.controls).forEach(controlName => {
                const control = this.caseInsurerForm.controls[controlName];
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

        // if (!caseInsurerId) {
        //     this.caseInsurer = new CreateOrEditCaseInsurerDto();
        //     this.caseInsurer.id = caseInsurerId;
        //     this.companyName = '';


        //     this.active = true;
        // } else {
        this._caseInsurersServiceProxy.getCaseInsurerForEdit(registerId).subscribe(result => {

            this.caseInsurer = result.caseInsurer;
            if (!this.caseInsurer) {
                this.caseInsurer = new CreateOrEditCaseInsurerDto();
                this.companyName = '';
            }

            this.caseInsurer.registerId = registerId;
            this.companyName = result.companyName;


            this.active = true;
        });
        // }
        this._CommonDropdownService.getAllCompanyForTableDropdown().subscribe(result => {
            this.allCompanys = result;
        });


    }

    save(): void {
        this.saving = true;

        this._caseInsurersServiceProxy.createOrEdit(this.caseInsurer)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                //  this._router.navigate( ['/app/main/registration/caseInsurers']);

                this.markFormAsPristine(this.caseInsurerForm);
            })
    }

    next(): void {
        // this._router.navigate(['/app/main/registration/caseIncident?id={{this.registerId}}']);
    }

    // saveAndNew(): void {
    //     this.saving = true;

    //     this._caseInsurersServiceProxy.createOrEdit(this.caseInsurer)
    //         .pipe(finalize(() => {
    //             this.saving = false;
    //         }))
    //         .subscribe(x => {
    //             this.saving = false;               
    //             this.notify.info(this.l('SavedSuccessfully'));
    //             this.caseInsurer = new CreateOrEditCaseInsurerDto();
    //         });
    // }

















}
