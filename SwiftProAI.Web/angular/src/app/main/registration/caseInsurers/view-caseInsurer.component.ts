import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    CaseInsurersServiceProxy, CreateOrEditCaseInsurerDto,
    CommonDropdownServiceProxy,
    CommonDropdownDto,
    CaseInsurerDto
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { NgForm } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';




@Component({
    selector: 'viewCaseInsurer',
    templateUrl: './view-caseInsurer.component.html',
    animations: [appModuleAnimation()]
})
export class ViewCaseInsurerComponent extends AppComponentBase implements OnInit {


    item: CaseInsurerDto = new CaseInsurerDto();

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



    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);

    }

    show(registerId?: number): void {

        this._caseInsurersServiceProxy.getCaseInsurerForView(registerId).subscribe(result => {

            this.item = result.caseInsurer;
            if (!this.item) {
                this.item = new CaseInsurerDto();
                this.companyName = '';
            }

            this.item.registerId = registerId;
            this.companyName = result.companyName;

        });

        this._CommonDropdownService.getAllCompanyForTableDropdown().subscribe(result => {
            this.allCompanys = result;
        });


    }























}
