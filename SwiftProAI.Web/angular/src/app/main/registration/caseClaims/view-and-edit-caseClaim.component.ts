import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    CaseClaimsServiceProxy,
    CreateOrEditCaseClaimDto,
    MainRegistrationServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs';
import { CreateOrEditCaseSearchFeeModalComponent } from '../caseSearchFees/create-or-edit-caseSearchFee-modal.component';
import { CaseSearchFeesComponent } from '../caseSearchFees/caseSearchFees.component';
import { ActivatedRoute, Router } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './view-and-edit-caseClaim.component.html',
    animations: [appModuleAnimation()],
})
export class ViewAndEditCaseClaimComponent extends AppComponentBase implements OnInit {
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild(CreateOrEditCaseSearchFeeModalComponent)
    createOrEditCaseSearchFeeModal: CreateOrEditCaseSearchFeeModalComponent;

    @ViewChild(CaseSearchFeesComponent)
    caseSearchFeesTable: CaseSearchFeesComponent;

    searchFeeTotal: number;
    mileageTotal: number;
    total: number;
    caseClaims: any[] = [];
    caseClaim: CreateOrEditCaseClaimDto = new CreateOrEditCaseClaimDto();
    saving: boolean = false;
    bsConfigMonth: Partial<BsDatepickerConfig>;
    bsConfigYear: Partial<BsDatepickerConfig>;
    selectedCaseClaim: CreateOrEditCaseClaimDto | null = null;
    filters: {
        vehicleNumber: string;
        month: DateTime;
        year: DateTime;
    } = <any>{};

    ngOnInit(): void {
        this.bsConfigMonth = {
            dateInputFormat: 'MMMM YYYY', // Format to display
            minMode: 'month', // Only show months and years
            adaptivePosition: true // Adjust position to fit within the viewport
          };
          this.bsConfigYear = {
            dateInputFormat: 'YYYY', // Format to display
            minMode: 'year', // Only show years
            adaptivePosition: true // Adjust position to fit within the viewport
          };
        this.getCaseClaimsList;
        const id = this._activatedRoute.snapshot.queryParams['id'];
        console.log('ID from query params:', id);
    }

    ngAfterViewInit(): void {
        // Access and initialize your child components here
        if (this.createOrEditCaseSearchFeeModal && this.caseSearchFeesTable) {
            console.log('Modal component initialized:', this.createOrEditCaseSearchFeeModal);
            console.log('Table component initialized:', this.caseSearchFeesTable);

            // Example: Subscribe to modalSave event
            this.createOrEditCaseSearchFeeModal.modalSave.subscribe((event) => {
                console.log('Modal saved event received:', event);
                // Example: Call a method in CaseSearchFeesComponent after modal is saved
                this.caseSearchFeesTable.getCaseSearchFees();
            });
        }
    }
    resetFilters(): void {
        this.filters.month = undefined;
        this.filters.year = undefined;
        this.filters.vehicleNumber = undefined;
    }

    updateTotal() {
        this.selectedCaseClaim.total =
            this.selectedCaseClaim.fraudAmount +
            this.selectedCaseClaim.fileCharges +
            this.selectedCaseClaim.mileageTotal +
            this.selectedCaseClaim.police +
            this.selectedCaseClaim.airFare +
            this.selectedCaseClaim.charteredTransport +
            this.selectedCaseClaim.toll +
            this.selectedCaseClaim.hotel +
            this.selectedCaseClaim.sd +
            this.selectedCaseClaim.airFare;
    }

    onMonthFilterChange() {
        //Set the year filter to be equal as monthFilter because backend will handle getting the month and year
        this.filters.year = this.filters.month;
    }

    onYearFilterChange() {
        //Reset the monthFilter so you won't have conflicts like monthFilter: June 2023 and year filter 2025
        this.filters.month = undefined;
    }

    getCaseClaimsList(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);

            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._caseClaimsServiceProxy
            .getAll(
                this.filters.month === undefined
                    ? this.filters.month
                    : this._dateTimeService.getEndOfDayForDate(this.filters.month),
                this.filters.year === undefined
                    ? this.filters.year
                    : this._dateTimeService.getStartOfDayForDate(this.filters.year),
                this.filters.vehicleNumber,
                undefined,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getMaxResultCount(this.paginator, event),
                this.primengTableHelper.getSkipCount(this.paginator, event),
            )
            .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    updateCaseClaim(): void {
        this.saving = true;
        this._caseClaimsServiceProxy
            .createOrEdit(this.selectedCaseClaim)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
            });
    }

    createCaseSearchFee(): void {
        this.createOrEditCaseSearchFeeModal.show();
    }

    selectCaseClaim(record: any): void {
        this.selectedCaseClaim = record;
        console.log('Selected Case Claim:', this.selectedCaseClaim);
        this.router.navigate([], {
            relativeTo: this._activatedRoute,
            queryParams: { id: this.selectedCaseClaim.registerId },
            queryParamsHandling: 'merge', // merge existing query params
        });
        console.log('QueryParams Id: ' + this._activatedRoute.snapshot.queryParams['id']);
        console.log('iD for case claim : ' + this.selectedCaseClaim.id);
    }

    onModalSave(event: any) {
        try {
            if (this.caseSearchFeesTable) {
                this.caseSearchFeesTable.getCaseSearchFees();
                this.selectedCaseClaim.searchFee = this.caseSearchFeesTable.getTotalSearchFee();
            }
        } catch (error) {
            console.log(error);
        }
    }

    constructor(
        injector: Injector,
        private _caseClaimsServiceProxy: CaseClaimsServiceProxy,
        private _mainRegistrationServiceProxy: MainRegistrationServiceProxy,
        private _dateTimeService: DateTimeService,
        private _activatedRoute: ActivatedRoute,
        private router: Router,
    ) {
        super(injector);
    }
}
