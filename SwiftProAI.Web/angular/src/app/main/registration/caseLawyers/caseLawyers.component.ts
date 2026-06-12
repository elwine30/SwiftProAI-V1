import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { CaseLawyersServiceProxy, CaseLawyerDto  } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { Input } from '@angular/core';
import { OnChanges, SimpleChanges } from '@angular/core';

@Component({
    selector: 'caseLawyersComponent',
    templateUrl: './caseLawyers.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class CaseLawyersComponent extends AppComponentBase implements OnChanges {
    @Input() registerId;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
    advancedFiltersAreShown = false;
    filterText = '';
    maxHearingDateFilter : DateTime;
	minHearingDateFilter : DateTime;
    referenceNoFilter = '';
    contactNoFilter = '';
    contactNameFilter = '';
    emailFilter = '';
    typeFilter = '';
    mainRegistrationVehicleNoFilter = '';
    lawFirmNameFilter = '';
    registerIdFilter = "";

    constructor(
        injector: Injector,
        private _caseLawyersServiceProxy: CaseLawyersServiceProxy,
		private _router: Router,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        
    }

    getCaseLawyers(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._caseLawyersServiceProxy.getAll(
            this.filterText,
            this.maxHearingDateFilter === undefined ? this.maxHearingDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxHearingDateFilter),
            this.minHearingDateFilter === undefined ? this.minHearingDateFilter : this._dateTimeService.getStartOfDayForDate(this.minHearingDateFilter),
            this.referenceNoFilter,
            this.contactNoFilter,
            this.contactNameFilter,
            this.emailFilter,
            this.typeFilter,
            this.registerIdFilter,
            this.lawFirmNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            50
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    ngOnChanges(changes: SimpleChanges) {
        if (changes['registerId']) {
            this.registerIdFilter = this.registerId.toString();
            this.getCaseLawyers();
        }
        if (changes['caseLawyerId']) {
            this.registerIdFilter = this.registerId.toString();
            this.getCaseLawyers();
        }
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createCaseLawyer(): void {
        this._router.navigate(['/app/main/registration/caseLawyers/createOrEdit']);        
    }


    deleteCaseLawyer(caseLawyer: CaseLawyerDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._caseLawyersServiceProxy.delete(caseLawyer.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
    
    resetFilters(): void {
        this.filterText = '';
        this.maxHearingDateFilter = undefined;
	    this.minHearingDateFilter = undefined;
        this.referenceNoFilter = '';
        this.contactNoFilter = '';
        this.contactNameFilter = '';
        this.emailFilter = '';
        this.typeFilter = '';
		this.mainRegistrationVehicleNoFilter = '';
		this.lawFirmNameFilter = '';
        this.getCaseLawyers();
    }

}
