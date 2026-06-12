import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DashboardCustomizationConst } from '@app/shared/common/customizable-dashboard/DashboardCustomizationConsts';
import { MainRegistrationServiceProxy, CommonDropdownServiceProxy, CommonDropdownDto, CommonAdjusterDropdownDto } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';
import { Enum3rdPartyRegistrationStatus } from '@app/shared/common/registration/enum';
import { formatDate } from '@angular/common';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';

@Component({
    templateUrl: './dashboard3rdParty.component.html',
    styleUrls: ['./dashboard3rdParty.component.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class Dashboard3rdPartyComponent extends AppComponentBase implements OnInit {
    dashboardName = DashboardCustomizationConst.dashboardNames.defaultTenantDashboard;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    _entityTypeFullName = 'ThinknInsurTech.MultiTenancy.Tenant';
    entityHistoryEnabled = false;
    assignmentDateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

    enumRegistrationStatus = Enum3rdPartyRegistrationStatus;
    underInvestigationCount = 0;
    adjusterCount = 0;
    pendingInvoiceCount = 0;
    completedInvoiceCount = 0;
    cancelledCount = 0;
    totalCount = 0;
    allAdjusters: CommonAdjusterDropdownDto[];
    allCompanies: CommonDropdownDto[];
    adjusterCompanies: CommonDropdownDto[];

    cols = [
        { field: 'caseNo', header: 'Case No' },
        { field: 'assignTime', header: 'Assignment Date' },
        { field: 'caseTypeShortName', header: 'Case Type' },
        { field: 'vehicleNo', header: 'Vehicle Number' },
        { field: 'companyShortName', header: 'Insurance Company' },
        { field: 'branchShortName', header: 'Branch' },
        { field: 'modeOfAssignment', header: 'Mode of Assignment' },
        { field: 'adjusterUserName', header: 'Adjuster' }
    ];

    filters: {
        filterText: string;
        assignmentDateRangeActive: boolean;
        selectedStatusId: number;
        selectedCompanyId: number;
        selectedAdjusterId: number;
        selectedEditorId: number;
        selectedAdjusterCompanyOUId: number;
    } = <any>{};
    selectedStatusId: any;

    toChildID: number;

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _MainRegistrationService: MainRegistrationServiceProxy,
        private _CommonDropdownService: CommonDropdownServiceProxy,
        private _dateTimeService: DateTimeService,
        private _router: Router,
        private _navigationService: NavigationService
    ) {
        super(injector);
        this.setFiltersFromRoute();
    }

    statuses: any[];


    setFiltersFromRoute(): void {
        if (this._activatedRoute.snapshot.queryParams['assignmentDateStart'] != null) {
            this.filters.assignmentDateRangeActive = true;
            this.assignmentDateRange[0] = this._dateTimeService.fromISODateString(
                this._activatedRoute.snapshot.queryParams['assignmentDateStart']
            );
        } else {
            this.assignmentDateRange[0] = this._dateTimeService.getEndOfDayMinusDays(30);
        }

        if (this._activatedRoute.snapshot.queryParams['assignmentDateEnd'] != null) {
            this.filters.assignmentDateRangeActive = true;
            this.assignmentDateRange[1] = this._dateTimeService.fromISODateString(
                this._activatedRoute.snapshot.queryParams['assignmentDateEnd']
            );
        } else {
            this.assignmentDateRange[1] = this._dateTimeService.getStartOfDay();
        }

        if (this._activatedRoute.snapshot.queryParams['statusId'] != null) {
            this.filters.selectedStatusId = parseInt(this._activatedRoute.snapshot.queryParams['statusId']);
        }

        if (this._activatedRoute.snapshot.queryParams['companyId'] != null) {
            this.filters.selectedCompanyId = parseInt(this._activatedRoute.snapshot.queryParams['companyId']);
        }

        if (this._activatedRoute.snapshot.queryParams['adjusterId'] != null) {
            this.filters.selectedAdjusterId = parseInt(this._activatedRoute.snapshot.queryParams['adjusterId']);
        }

        if (this._activatedRoute.snapshot.queryParams['editorId'] != null) {
            this.filters.selectedEditorId = parseInt(this._activatedRoute.snapshot.queryParams['editorId']);
        }
    }

    ngOnInit(): void {
        this.statuses = Object.keys(Enum3rdPartyRegistrationStatus)
            .filter(key => !isNaN(Number(Enum3rdPartyRegistrationStatus[key])))
            .map(key => ({
                value: Enum3rdPartyRegistrationStatus[key],
                display: this.convertToPascalSnakeCase(key) // Use the conversion method
            }));

        this.filters.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
        this.getDashboardSummary();

        this._CommonDropdownService.getAllAdjusterForTableDropdown(undefined).subscribe(result => {
            this.allAdjusters = result;
        });

        this._CommonDropdownService.getAllCompanyForTableDropdown().subscribe(result => {
            this.allCompanies = result;
        });

        this._CommonDropdownService.getAdjusterCompanyForTableDropdown().subscribe(result => {
            this.adjusterCompanies = result;
        });

        if (this._activatedRoute.snapshot.queryParams['status'] != null) {
            this.selectedStatusId = Enum3rdPartyRegistrationStatus[this._activatedRoute.snapshot.queryParams['status']];
        }
    }

    getMainRegistrationDetails(event?: LazyLoadEvent) {
        //Set the declaration here so that this only calls when Search button is clicked. 
        //If this.filters.selectedStatusId is binded to the model, it will angular will update the property and trigger search even if there is no onChange() added.
        this.filters.selectedStatusId = this.selectedStatusId
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);

            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();
        this._MainRegistrationService
            .getMainRegistrationDetails(
                this.filters.filterText,
                this.filters.assignmentDateRangeActive ? this.assignmentDateRange[0] : undefined,
                this.filters.assignmentDateRangeActive ? this.assignmentDateRange[1].endOf('day') : undefined,
                this.filters.selectedStatusId,
                this.filters.selectedStatusId !== undefined && this.filters.selectedStatusId + '' !== '-1',
                this.filters.selectedCompanyId,
                this.filters.selectedCompanyId !== undefined && this.filters.selectedCompanyId + '' !== '-1',
                this.filters.selectedAdjusterId,
                this.filters.selectedAdjusterId !== undefined && this.filters.selectedAdjusterId + '' !== '-1',
                this.filters.selectedEditorId,
                this.filters.selectedEditorId !== undefined && this.filters.selectedEditorId + '' !== '-1',
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getMaxResultCount(this.paginator, event),
                this.primengTableHelper.getSkipCount(this.paginator, event)
            )
            .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = this.formatDates(result.items);
                console.log(this.primengTableHelper);
                this.primengTableHelper.hideLoadingIndicator();
            });

    }
    formatDates(records: any[]): any[] {
        return records.map(record => {
            return {
                ...record,
                assignTime: record.assignTime ? formatDate(new Date(record.assignTime), 'dd/MM/yyyy', 'en') : '-'
            };
        });
    }

    getStatusesFromEnum(): any[] {
        return Object.keys(Enum3rdPartyRegistrationStatus)
            .filter(key => !isNaN(Number(Enum3rdPartyRegistrationStatus[key])))
            .map(key => ({
                value: Enum3rdPartyRegistrationStatus[key],
                display: this.convertToPascalSnakeCase(key)
            }));
    }

    convertToPascalSnakeCase(str: string): string {
        return str.replace(/([A-Z])/g, ' $1') // insert a space before all caps
            .trim() // remove the leading space
            .replace(/ /g, ' '); // replace spaces with underscores
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    getDashboardSummary(): void {
        this._MainRegistrationService.getMainRegistrationDashboardSummary()
            .subscribe((result) => {
                Object.entries(result).forEach(([key, value]) => {
                    switch (parseInt(key)) {
                        case Enum3rdPartyRegistrationStatus.UnderInvestigation:
                            this.underInvestigationCount = value;
                            break;
                        case Enum3rdPartyRegistrationStatus.Completed:
                            this.completedInvoiceCount = value;
                            break;
                        case Enum3rdPartyRegistrationStatus.Cancelled:
                            this.cancelledCount = value;
                            break;
                        default:
                            break;
                    }
                    this.totalCount = this.underInvestigationCount + this.completedInvoiceCount + this.cancelledCount
                });
            });
    }

    isInvoiceStatus(statusId: number): boolean {
        return statusId == this.enumRegistrationStatus.Completed
    }

    onClick(statusId) {
        this.selectedStatusId = statusId;
        this._router.navigate(['/app/main/dashboard3rdParty'], { queryParams: { status: Enum3rdPartyRegistrationStatus[statusId] } });
        this.getMainRegistrationDetails()
    }

    exportTable(): void {
        if (this.dataTable) {
            console.log(this.dataTable.filter);
            this.dataTable.exportCSV();
        } else {
            console.error('dataTable is not yet initialized');
        }
    }


    onRowClicked(id: number, isView: boolean) {
        this._navigationService.viewOnly = isView;
        this._navigationService.registerId = id.toString();
        // console.log(isView, this._navigationService.step1Url);
        this._router.navigate([this._navigationService.step1Url], { queryParams: { id: id, stepId: 1 } });
    }

    filterAdjuster(){
        this._CommonDropdownService.getAllAdjusterForTableDropdown(this.filters.selectedAdjusterCompanyOUId).subscribe(result => {
            this.allAdjusters = result;
        });
    }
}
