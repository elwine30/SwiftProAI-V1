import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { BranchServiceProxy, GetBranchForViewDto, BranchDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-branch.component.html',
    animations: [appModuleAnimation()]
})
export class ViewBranchComponent extends AppComponentBase implements OnInit {

    active = false;
    saving = false;

    item: GetBranchForViewDto;

breadcrumbs: BreadcrumbItem[]= [
                        new BreadcrumbItem(this.l("Branch"),"/app/main/branches/branch"),
                        new BreadcrumbItem(this.l('Branch') + '' + this.l('Details')),
                    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
         private _branchServiceProxy: BranchServiceProxy
    ) {
        super(injector);
        this.item = new GetBranchForViewDto();
        this.item.branch = new BranchDto();        
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(branchId: number): void {
      this._branchServiceProxy.getBranchForView(branchId).subscribe(result => {      
                 this.item = result;
                this.active = true;
            });       
    }
    
    
}
