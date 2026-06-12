import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MainRegistrationServiceProxy, CreateMainRegistrationInput, RegistrationCaseTypeLookupTableDto, CommonDropdownDto, CommonDropdownServiceProxy, CommonAdjusterDropdownDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { Router } from '@angular/router';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';
import { NgForm } from '@angular/forms';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
@Component({
    selector: 'registrationForm',
    templateUrl: './registration.component.html',
    animations: [appModuleAnimation()],
})
export class RegistrationComponent extends AppComponentBase implements DirtyFormGuard {
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('registrationForm') registrationForm: NgForm;
    @Output() formSave: EventEmitter<any> = new EventEmitter<any>();

    registrationDetails: CreateMainRegistrationInput = new CreateMainRegistrationInput();
    registerId: number;
    showButton: boolean = true;
    today = new Date();
    adjusterMemberId: number;

    modesOfAssignment = [
        { value: "Merimen" },
        { value: "Email" },
        { value: "Fax" },
        { value: "By Hand" },
        { value: "Portal" }
    ];

    allAdjusters: CommonAdjusterDropdownDto[];
    allCompanies: CommonDropdownDto[];
    allCaseTypes: RegistrationCaseTypeLookupTableDto[];
    allBranches: CommonDropdownDto[];
    tenants = [{ value: 1, display: 'Tenant 1' }];

    active: boolean = true;
    saving: boolean = false;
    remarkInputActive: boolean = false;
    filters: {
        filterText: string;
        assignmentDateRangeActive: boolean;
        selectedStatusId: number;
        selectedCompanyId: number;
        selectedAdjusterId: number;
        selectedEditorId: number;
    } = <any>{};
    assignmentDateRange: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];
    cols = [
        { field: 'id', header: 'ID No' },
        { field: 'assignTime', header: 'Assignment Date' },
        { field: 'caseTypeShortName', header: 'Case Type' },
        { field: 'vehicleNo', header: 'Vehicle Number' },
        { field: 'companyShortName', header: 'Company' },
        { field: 'branchShortName', header: 'Branch' },
        { field: 'modeOfAssignment', header: 'Mode of Assignment' },
        { field: 'adjusterMemberId', header: 'Adjuster Member ID' },
    ];

    stringAssignDate: string;

    constructor(
        injector: Injector,
        private _MainRegistrationService: MainRegistrationServiceProxy,
        private _CommonDropdownService: CommonDropdownServiceProxy,
        private _dateTimeService: DateTimeService,
        private _router: Router,
    ) {
        super(injector);

        this._CommonDropdownService.getAllCompanyForTableDropdown().subscribe((result) => {
            this.allCompanies = result;
        });

        this._MainRegistrationService.getAllCaseTypeForTableDropdown().subscribe((result) => {
            this.allCaseTypes = result;
        });

        this._CommonDropdownService.getAllBranchForTableDropdown().subscribe((result) => {
            this.allBranches = result;
        });
    }

    updateAdjusterslist(branchId: number): void {
        this.registrationDetails.adjusterMemberId = null;
        this._CommonDropdownService.getAllAdjusterByBranchForTableDropdown(branchId).subscribe(result => {
            this.allAdjusters = result;
        });
    }

    save(): void {

        this.registrationDetails.assignTime = DateTime.fromJSDate(new Date(this.stringAssignDate));
        // Set to zero as default value if the form not fill up
        this.registrationDetails.caseTypeId = this.registrationDetails.caseTypeId == undefined ? 0 : this.registrationDetails.caseTypeId ;
        this.registrationDetails.companyId = this.registrationDetails.companyId == undefined ? 0 : this.registrationDetails.companyId ;
        this.registrationDetails.branchId = this.registrationDetails.branchId == undefined ? 0 : this.registrationDetails.branchId ;
        this.registrationDetails.adjusterMemberId = this.adjusterMemberId == undefined ? 0 : this.adjusterMemberId ;
        
        this.saving = true;
        if (!this.remarkInputActive) {
            this.registrationDetails.remarkDescription = '';
        }
        this._MainRegistrationService
            .createMainRegistration(this.registrationDetails)
            .pipe(finalize(() => (this.saving = false)))
            .subscribe((result) => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.formSave.emit(this.registrationDetails);
                this.registerId = result;
                this.showButton = false;

                this.markFormAsPristine(this.registrationForm);
            });
    }

    backToDashboard(): void {
        this._router.navigate(['/app/main/dashboard']);
    }

    proceedToRegistrationDetails(): void {
        this._router.navigate(['/app/main/registration/caseAdjusters/createOrEdit'], {
            queryParams: { id: this.registerId, stepId: 1 },
        });
    }

    getDuplicateRegistration(): void {
        if (this.registrationDetails.vehicleNo && this.registrationDetails.vehicleNo.trim() !== '') {
            this.getMainRegistrationDetails();
        } else {
            // Reset the table to show no data
            this.primengTableHelper.totalRecordsCount = 0;
            this.primengTableHelper.records = [];
        }
    }

    getMainRegistrationDetails(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);

            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._MainRegistrationService
            .getMainRegistrationDetails(
                (this.filters.filterText = this.registrationDetails.vehicleNo),
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
                this.primengTableHelper.getSkipCount(this.paginator, event),
            )
            .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    canDeactivate(): Promise<boolean> | boolean {
        this.hideMainSpinner();

        if (this.registrationForm.dirty) {
            var returnVal = this.swalAlert.fire({
                title: "Are you sure you want to leave?",
                text: "Your changes will be lost.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Yes, proceed!"
            });

            return returnVal.then(result => result.isConfirmed);
        } else {
            return true;
        }
    }
}
