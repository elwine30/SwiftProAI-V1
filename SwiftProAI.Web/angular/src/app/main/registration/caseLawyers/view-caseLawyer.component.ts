import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    CaseLawyersServiceProxy,
    CaseLawyerLawFirmLookupTableDto,
    LookupsServiceProxy,
    GetLookupByGroupDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { LazyLoadEvent } from 'primeng/api';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { DateTime } from 'luxon';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import { ViewcaseLawyerModalComponent } from './view-caseLawyer.modal.component';

@Component({
    selector: 'viewCaseLawyer',
    templateUrl: './view-caseLawyer.component.html',
    animations: [appModuleAnimation()],
})
export class ViewCaseLawyerComponent extends AppComponentBase implements OnInit {
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('viewcaseLawyerModal', { static: true }) viewcaseLawyerModal: ViewcaseLawyerModalComponent;

    lawFirmName = '';
    advancedFiltersAreShown = false;
    filterText = '';
    maxHearingDateFilter: DateTime;
    minHearingDateFilter: DateTime;
    referenceNoFilter = '';
    contactNoFilter = '';
    contactNameFilter = '';
    emailFilter = '';
    typeFilter = '';
    mainRegistrationVehicleNoFilter = '';
    lawFirmNameFilter = '';
    registerIdFilter = '';
    caseLawyerIdNumber: number;

    allLawFirms: CaseLawyerLawFirmLookupTableDto[];
    types: GetLookupByGroupDto[];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseLawyersServiceProxy: CaseLawyersServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService,
        public navigationService: NavigationService,
        private _lookupService: LookupsServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.registerIdFilter = this._activatedRoute.snapshot.queryParams['id'].toString();
        this.navigationService.registerId = this.registerIdFilter;
    }

    // Add the methods from caseLawyers.component.ts here
    getCaseLawyers(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._caseLawyersServiceProxy
            .getAllForView(
                this.filterText,
                this.maxHearingDateFilter === undefined
                    ? this.maxHearingDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxHearingDateFilter),
                this.minHearingDateFilter === undefined
                    ? this.minHearingDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minHearingDateFilter),
                this.referenceNoFilter,
                this.contactNoFilter,
                this.contactNameFilter,
                this.emailFilter,
                this.typeFilter,
                this.registerIdFilter,
                this.lawFirmNameFilter,
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
