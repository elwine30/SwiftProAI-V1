import { Component, ViewChild, Injector, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { CaseThirdPartyVehiclesServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import { ViewcaseThirdPartyVehicleModalComponent } from './view-caseThirdPartyVehicle.modal.component';

@Component({
    templateUrl: './view-caseThirdPartyVehicle.component.html',
    animations: [appModuleAnimation()],
})
export class ViewCaseThirdPartyVehicleComponent extends AppComponentBase implements OnInit {
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('viewcaseThirdPartyVehicleModal', { static: true })
    viewcaseThirdPartyVehicleModal: ViewcaseThirdPartyVehicleModalComponent;

    registerIdFilter = '';

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseThirdPartyVehiclesServiceProxy: CaseThirdPartyVehiclesServiceProxy,
        public navigationService: NavigationService,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.registerIdFilter = this._activatedRoute.snapshot.queryParams['id'].toString();
        this.navigationService.registerId = this.registerIdFilter;
    }

    getCaseThirdPartyVehicles(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._caseThirdPartyVehiclesServiceProxy
            .getAllForView(
                this.registerIdFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                50,
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }
}
