import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    OnInit,
    ElementRef,
    ChangeDetectorRef,
} from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    CaseLawyersServiceProxy,
    CreateOrEditCaseLawyerDto,
    CaseLawyerLawFirmLookupTableDto,
    CaseLawyerDto,
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
import { NgForm } from '@angular/forms';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { AppConsts } from '@shared/AppConsts';
import { formatDate } from '@angular/common';

@Component({
    selector: 'createCaseLawyer',
    templateUrl: './create-or-edit-caseLawyer.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditCaseLawyerComponent extends AppComponentBase implements OnInit, DirtyFormGuard {
    @ViewChild('caseLawyerForm') caseLawyerForm: NgForm;
    isHidden = AppConsts.isComponentDisabled;
    // active = false;
    saving = false;

    caseLawyer: CreateOrEditCaseLawyerDto = new CreateOrEditCaseLawyerDto();
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

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
    savedHearingDate;

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
        private cdr: ChangeDetectorRef,
    ) {
        super(injector);
    }
    ngAfterViewInit(): void {
        if (AppConsts.isComponentDisabled) {
            setTimeout(() => {
                if (this.caseLawyerForm && this.caseLawyerForm.controls) {
                    this.disableForm();
                }
            });
        }
    }

    disableForm(): void {
        if (this.caseLawyerForm && this.caseLawyerForm.controls) {
            Object.keys(this.caseLawyerForm.controls).forEach((controlName) => {
                const control = this.caseLawyerForm.controls[controlName];
                control.disable();
            });
        } else {
            console.log('Form or controls not available');
        }
    }

    ngOnInit(): void {
        this.show(); //insert the case
        this.registerIdFilter = this._activatedRoute.snapshot.queryParams['id'].toString();
        this.navigationService.registerId = this.registerIdFilter;
        this._caseLawyersServiceProxy.getAllLawFirmForTableDropdown().subscribe((result) => {
            this.allLawFirms = result;
        });
        this._lookupService.getByGroup('InvolvedPartyType').subscribe((result) => {
            this.types = result;
        });
    }

    show(caseLawyerId?: number): void {
        this._caseLawyersServiceProxy.getCaseLawyerForEdit(caseLawyerId).subscribe((result) => {
            this.caseLawyer = result.caseLawyer;
            if (!this.caseLawyer) {
                console.log(result.caseLawyer);
                this.caseLawyer = new CreateOrEditCaseLawyerDto();
                this.lawFirmName = '';
                this.caseLawyer.hearingDate = this._dateTimeService.getDate();
                this.caseLawyer.contactNo = '';
            }
            this.savedHearingDate = formatDate(this.caseLawyer.hearingDate.toString(), 'dd/MM/yyyy', 'en-US');
            this.lawFirmName = result.lawFirmName;
            this.caseLawyer.registerId = this._activatedRoute.snapshot.queryParams['id'];
            this.cdr.detectChanges();

            // this.active = true;
        });
    }

    save(): void {
        this.saving = true;
        const hearFormat = DateTime.fromJSDate(this.savedHearingDate);
        this.caseLawyer.hearingDate = hearFormat;

        console.log('Converted Date:', this.caseLawyer.hearingDate);
        this._caseLawyersServiceProxy
            .createOrEdit(this.caseLawyer)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe(() => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.caseLawyer = new CreateOrEditCaseLawyerDto();
                this.getCaseLawyers(); // Refresh the table

                this.markFormAsPristine(this.caseLawyerForm);
            });
    }

    saveAndNew(): void {
        this.saving = true;
        const hearFormat = DateTime.fromJSDate(this.savedHearingDate);
        this.caseLawyer.hearingDate = hearFormat;

        console.log('Converted Date:', this.caseLawyer.hearingDate);

        this._caseLawyersServiceProxy
            .createOrEdit(this.caseLawyer)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe(() => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.caseLawyer = new CreateOrEditCaseLawyerDto();
                this.show();
                this.getCaseLawyers(); // Refresh the table

                this.markFormAsPristine(this.caseLawyerForm);
            });
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
            .getAll(
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

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    deleteCaseLawyer(caseLawyer: CaseLawyerDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._caseLawyersServiceProxy.delete(caseLawyer.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
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

    editCaseLawyer(record: CaseLawyerDto): void {
        // this.active = true;
        this.show(record.id);
    }

    canDeactivate(): boolean {
        this.hideMainSpinner();
        return !this.caseLawyerForm.dirty || confirm('Are you sure you want to leave? Your changes will be lost.');
    }
}
