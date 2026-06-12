import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    CaseThirdPartyInfosServiceProxy,
    GetCaseThirdPartyInfoForViewDto,
    CaseThirdPartyInfoDto,
    CreateOrEditInsuredPersonDto,
    CreateOrEditCaseThirdPartyInfoDto,
    InsuredPersonDto,
    CommonDropdownServiceProxy,
    CommonDropdownDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
import { LazyLoadEvent } from 'primeng/api';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
@Component({
    templateUrl: './view-caseThirdPartyInfo.component.html',
    animations: [appModuleAnimation()],
})
export class ViewCaseThirdPartyInfoComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;
    registerId = 0;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    isHidden = AppConsts.isComponentDisabled;
    @ViewChild('viewModal', { static: true }) viewModal: ModalDirective;

    caseInsuredPerson: InsuredPersonDto = new InsuredPersonDto();
    caseThirdPartyInfo: CaseThirdPartyInfoDto = new CaseThirdPartyInfoDto();

    mainRegistrationVehicleNo = '';
    hospitalName = '';
    hospitalAddress = '';
    hospitalName2 = '';
    hospitalAddress2 = '';
    hospitalName3 = '';
    hospitalAddress3 = '';
    allCountryLocations: CommonDropdownDto[];
    allStateLocations: CommonDropdownDto[];

    countryName = '';
    stateName = '';

    item: GetCaseThirdPartyInfoForViewDto;

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseThirdPartyInfosServiceProxy: CaseThirdPartyInfosServiceProxy,
        public navigationService: NavigationService,
        private _CommonDropdownService: CommonDropdownServiceProxy,
    ) {
        super(injector);
        this.item = new GetCaseThirdPartyInfoForViewDto();
        this.item.caseThirdPartyInfo = new CaseThirdPartyInfoDto();
    }

    ngOnInit(): void {
        // this.show(this._activatedRoute.snapshot.queryParams['id']);
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        this.navigationService.registerId = this.registerId.toString();
    }

    show(caseThirdPartyInfoId: number): void {
        this._caseThirdPartyInfosServiceProxy.getThirdPartyInfoForView(caseThirdPartyInfoId).subscribe((result) => {
            this.caseThirdPartyInfo = result.caseThirdPartyInfo;
            this.caseInsuredPerson = result.insuredPerson;
            this.mainRegistrationVehicleNo = result.mainRegistrationVehicleNo;
            this.item = result;
            this.hospitalName = result.hospitalName;
            this.hospitalAddress = result.hospitalAddress;
            this.hospitalName2 = result.hospitalName2;
            this.hospitalAddress2 = result.hospitalAddress2;
            this.hospitalName3 = result.hospitalName3;
            this.hospitalAddress3 = result.hospitalAddress3;
            this.active = true;
        });
        this.showModal();

        this._CommonDropdownService.getAllLocationByCountryForTableDropdown(0).subscribe((result) => {
            this.allCountryLocations = result;
        });
        this._CommonDropdownService.getAllLocationByCountryForTableDropdown(1).subscribe((result) => {
            this.allStateLocations = result;
        });

        this.countryName = this.allCountryLocations.find(
            (x) => x.id == this.caseInsuredPerson.countryLocationId,
        ).displayName;
        this.stateName = this.allStateLocations.find((x) => x.id == this.caseInsuredPerson.stateLocationId).displayName;
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFile?id=' + id;
    }

    showModal() {
        this.viewModal.show();
    }
    close() {
        this.viewModal.hide();
    }

    getCaseThirdPartyInfos(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._caseThirdPartyInfosServiceProxy
            .getAllForView(
                this.registerId,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event),
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
                console.log(this.primengTableHelper.records);
            });
    }
}
