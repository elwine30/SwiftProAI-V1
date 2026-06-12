import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    CaseIncidentDetailsServiceProxy,
    GetCaseIncidentDetailForViewDto,
    CaseIncidentDetailDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
@Component({
    templateUrl: './view-caseIncidentDetail.component.html',
    animations: [appModuleAnimation()],
})
export class ViewCaseIncidentDetailComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;
    registerId = '';
    item: GetCaseIncidentDetailForViewDto;

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseIncidentDetailsServiceProxy: CaseIncidentDetailsServiceProxy,
        public navigationService: NavigationService,
    ) {
        super(injector);
        this.item = new GetCaseIncidentDetailForViewDto();
        this.item.caseIncidentDetail = new CaseIncidentDetailDto();
    }

    ngOnInit(): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        this.show(this._activatedRoute.snapshot.queryParams['id']);

    }

    show(registerId: number): void {
        this.navigationService.registerId = this.registerId;
        this._caseIncidentDetailsServiceProxy.getCaseIncidentDetailForView(registerId).subscribe((result) => {
            this.item = result;
            this.item.caseIncidentDetail = this.item.caseIncidentDetail || new CaseIncidentDetailDto(); // Add null checking to prevent read undefined properties
            this.active = true;
        });
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFileFromFileOrg?id=' + id;
    }
}
