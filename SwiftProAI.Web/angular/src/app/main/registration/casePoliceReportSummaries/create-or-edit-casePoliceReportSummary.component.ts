import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CasePoliceReportSummariesServiceProxy, CreateOrEditCasePoliceReportSummaryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {Observable} from "@node_modules/rxjs";
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';




@Component({
    templateUrl: './create-or-edit-casePoliceReportSummary.component.html',
    animations: [appModuleAnimation()]
})
export class CreateOrEditCasePoliceReportSummaryComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;
    
    
    casePoliceReportSummary: CreateOrEditCasePoliceReportSummaryDto = new CreateOrEditCasePoliceReportSummaryDto();



breadcrumbs: BreadcrumbItem[]= [
                        new BreadcrumbItem(this.l("CasePoliceReportSummary"),"/app/main/registration/casePoliceReportSummaries"),
                        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
                    ];


    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,        
        private _casePoliceReportSummariesServiceProxy: CasePoliceReportSummariesServiceProxy,
        private _router: Router,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
        
    }

    show(casePoliceReportSummaryId?: number): void {

        if (!casePoliceReportSummaryId) {
            this.casePoliceReportSummary = new CreateOrEditCasePoliceReportSummaryDto();
            this.casePoliceReportSummary.id = casePoliceReportSummaryId;


            this.active = true;
        } else {
            this._casePoliceReportSummariesServiceProxy.getCasePoliceReportSummaryForEdit(casePoliceReportSummaryId).subscribe(result => {
                this.casePoliceReportSummary = result.casePoliceReportSummary;



                this.active = true;
            });
        }
        
         
    }
    
    save(): void {
        this.saving = true;
        
        
        this._casePoliceReportSummariesServiceProxy.createOrEdit(this.casePoliceReportSummary)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {
                 this.saving = false;               
                 this.notify.info(this.l('SavedSuccessfully'));
                 this._router.navigate( ['/app/main/registration/casePoliceReportSummaries']);
            })
    }
    
    saveAndNew(): void {
        this.saving = true;
        
        
        this._casePoliceReportSummariesServiceProxy.createOrEdit(this.casePoliceReportSummary)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {
                this.saving = false;               
                this.notify.info(this.l('SavedSuccessfully'));
                this.casePoliceReportSummary = new CreateOrEditCasePoliceReportSummaryDto();
                
            });
    }

















}
