import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { BranchServiceProxy, CreateOrEditBranchDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {Observable} from "@node_modules/rxjs";
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';




@Component({
    templateUrl: './create-or-edit-branch.component.html',
    animations: [appModuleAnimation()]
})
export class CreateOrEditBranchComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;
    
    
    branch: CreateOrEditBranchDto = new CreateOrEditBranchDto();



breadcrumbs: BreadcrumbItem[]= [
                        new BreadcrumbItem(this.l("Branch"),"/app/main/branches/branch"),
                        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
                    ];


    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,        
        private _branchServiceProxy: BranchServiceProxy,
        private _router: Router,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
        
    }

    show(branchId?: number): void {

        if (!branchId) {
            this.branch = new CreateOrEditBranchDto();
            this.branch.id = branchId;


            this.active = true;
        } else {
            this._branchServiceProxy.getBranchForEdit(branchId).subscribe(result => {
                this.branch = result.branch;



                this.active = true;
            });
        }
        
         
    }
    
    save(): void {
        this.saving = true;
        
        this._branchServiceProxy.createOrEdit(this.branch)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {
                 this.saving = false;               
                 this.notify.info(this.l('SavedSuccessfully'));
                 this._router.navigate( ['/app/main/branches/branch']);
            })
    }
    
    saveAndNew(): void {
        this.saving = true;
        
        this._branchServiceProxy.createOrEdit(this.branch)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {
                this.saving = false;               
                this.notify.info(this.l('SavedSuccessfully'));
                this.branch = new CreateOrEditBranchDto();
            });
    }

















}
