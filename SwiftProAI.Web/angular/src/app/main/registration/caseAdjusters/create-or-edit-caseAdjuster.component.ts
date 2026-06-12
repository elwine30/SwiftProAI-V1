import { Component, ViewChild, Injector, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    CaseAdjustersServiceProxy,
    CreateOrEditCaseAdjusterDto,
    CaseAdjusterScopeAssignmentLookupTableDto,
    CaseAdjusterLookupTableDto,
    CaseAdjusterLocationLookupTableDto,
    CaseAdjusterUserLookupTableDto,
    CommonDropdownServiceProxy,
    CommonDropdownDto,
    CommonAdjusterDropdownDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { formatDate } from '@angular/common';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import { EnumRegistrationStatus } from '@app/shared/common/registration/enum';
import { CreateRemarkModalComponent } from '../create-remark-modal.component';
import { NgForm } from '@angular/forms';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import Swal from 'sweetalert2';
import { AppConsts } from '@shared/AppConsts';
import { DateTime } from 'luxon';

@Component({
    selector: 'caseAdjusterForm',
    templateUrl: './create-or-edit-caseAdjuster.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditCaseAdjusterComponent extends AppComponentBase implements OnInit, DirtyFormGuard {
    @ViewChild('caseAdjusterForm') caseAdjusterForm: NgForm;

    active = false;
    saving = false;
    isHidden = AppConsts.isComponentDisabled;
    @ViewChild('createRemarkModal', { static: true }) createRemarkModal: CreateRemarkModalComponent;

    caseAdjuster: CreateOrEditCaseAdjusterDto = new CreateOrEditCaseAdjusterDto();
    scopeAssignmentDescription = '';
    caseTypeId = -1;
    branchId = -1;
    adjusterMemberId = -1;
    adjusterContact = '';
    assignmentTime: string;
    completionTime: string;
    allCaseTypes: CaseAdjusterLookupTableDto[];
    allBranches: CommonDropdownDto[];
    allAdjusters: CommonAdjusterDropdownDto[];
    statuses: any[];

    allScopeAssignments: CaseAdjusterScopeAssignmentLookupTableDto[];
    allStateLocations: CaseAdjusterLocationLookupTableDto[];
    allEditorUsers: CaseAdjusterUserLookupTableDto[];
    registerId = '';
    cancelId = 0;

    scopeAssignment_othersId = 0;
    minExtendDate;
    extendedCompletionDateString;

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseAdjustersServiceProxy: CaseAdjustersServiceProxy,
        private _CommonDropdownService: CommonDropdownServiceProxy,
        private _router: Router,
        public navigationService: NavigationService,
    ) {
        super(injector);
    }

    ngAfterViewInit(): void {
        if (AppConsts.isComponentDisabled) {
            setTimeout(() => {
                if (this.caseAdjusterForm && this.caseAdjusterForm.controls) {
                    this.disableForm();
                }
            });
        }
    }

    disableForm(): void {
        if (this.caseAdjusterForm && this.caseAdjusterForm.controls) {
            Object.keys(this.caseAdjusterForm.controls).forEach((controlName) => {
                const control = this.caseAdjusterForm.controls[controlName];
                control.disable();
            });
        } else {
            console.log('Form or controls not available');
        }
    }

    ngOnInit(): void {
        this.statuses = Object.keys(EnumRegistrationStatus)
            .filter((key) => !isNaN(Number(EnumRegistrationStatus[key])))
            .map((key) => ({
                value: EnumRegistrationStatus[key],
                display: this.convertToPascalSnakeCase(key), // Use the conversion method
            }));

        this.show(this._activatedRoute.snapshot.queryParams['id']);
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];

        this.navigationService.registerId = this.registerId;
        this.cancelId = EnumRegistrationStatus.Cancelled;
    }

    show(registerId?: number): void {
        // if (!caseAdjusterId) {
        //     this.caseAdjuster = new CreateOrEditCaseAdjusterDto();
        //     this.caseAdjuster.id = caseAdjusterId;
        //     this.scopeAssignmentDescription = '';
        //     this.registrationCaseTypeId = '';

        //     this.active = true;
        // } else {
        this._caseAdjustersServiceProxy.getCaseAdjusterForEdit(registerId).subscribe((result) => {
            this.caseAdjuster = result.caseAdjuster;
            if (!this.caseAdjuster) {
                this.caseAdjuster = new CreateOrEditCaseAdjusterDto();
                this.caseAdjuster.registerId = registerId;
                this.scopeAssignmentDescription = '';
                this.caseAdjuster.extendedCompletionDate = null;
            }

            this.caseAdjuster.registerId = registerId;
            this.scopeAssignmentDescription = result.scopeAssignmentDescription;
            this.caseTypeId = result.caseTypeId;
            this.branchId = result.branchId;
            this.adjusterMemberId = result.adjusterMemberId;
            this.adjusterContact = result.adjusterContact;
            this.assignmentTime = formatDate(result.assignmentTime.toString(), 'dd/MM/yyyy', 'en-US');
            this.completionTime = formatDate(result.completionTime.toString(), 'dd/MM/yyyy', 'en-US');
            this.minExtendDate = result.completionTime.toJSDate();
            this.minExtendDate.setDate(this.minExtendDate.getDate() + 1);
            this.caseAdjuster.extendedCompletionDate = result.caseAdjuster.extendedCompletionDate || null;
            this.extendedCompletionDateString = this.caseAdjuster.extendedCompletionDate ? this.caseAdjuster.extendedCompletionDate.toJSDate() : null;
            this.caseAdjuster.extendCompletionRemark = result.caseAdjuster ? result.caseAdjuster.extendCompletionRemark : null;
            this._CommonDropdownService.getAllAdjusterByBranchForTableDropdown(this.branchId).subscribe((result) => {
                this.allAdjusters = result;
            });
            this.active = true;
        });
        // }
        this._caseAdjustersServiceProxy.getAllScopeAssignmentForTableDropdown().subscribe((result) => {
            this.allScopeAssignments = result;
            this.checkOthersValueScopeId(this.allScopeAssignments);
        });

        this._caseAdjustersServiceProxy.getAllCaseTypeForTableDropdown().subscribe((result) => {
            this.allCaseTypes = result;
        });

        this._CommonDropdownService.getAllBranchForTableDropdown().subscribe((result) => {
            this.allBranches = result;
        });

        this._caseAdjustersServiceProxy.getAllStateLocationForTableDropdown(1).subscribe((result) => {
            this.allStateLocations = result;
        });
        this._caseAdjustersServiceProxy.getAllEditorUserForTableDropdown().subscribe((result) => {
            this.allEditorUsers = result;
        });
    }

    checkOthersValueScopeId(scopeAssignments: CaseAdjusterScopeAssignmentLookupTableDto[]): void {
        this.scopeAssignment_othersId = scopeAssignments.find((scope) => scope.displayName === 'Others').id;
        console.log('this.scopeAssignment_othersId ', this.scopeAssignment_othersId);
    }

    save(): void {
        this.saving = true;
        if (this.caseAdjuster.scopeAssignmentId != this.scopeAssignment_othersId) {
            this.caseAdjuster.scopeAssignmentRemarks = '';
        }

        this.caseAdjuster.extendedCompletionDate = this.extendedCompletionDateString != null ? DateTime.fromJSDate(new Date(this.extendedCompletionDateString)) : null;

        this._caseAdjustersServiceProxy
            .createOrEdit(this.caseAdjuster)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.markFormAsPristine(this.caseAdjusterForm);
            });
    }

    onStatusChange(selectedStatusId: number) {
        if (selectedStatusId == this.cancelId) {
            this.cancelCase();
        }
    }

    cancelCase() {
        // Implement your cancel case logic here
        console.log('Case has been cancelled');
        this.createNewRemark();
    }

    createNewRemark(): void {
        this.createRemarkModal.show(Number(this.registerId));
    }

    getStatusesFromEnum(): any[] {
        return Object.keys(EnumRegistrationStatus)
            .filter((key) => !isNaN(Number(EnumRegistrationStatus[key])))
            .map((key) => ({
                value: EnumRegistrationStatus[key],
                display: this.convertToPascalSnakeCase(key),
            }));
    }

    convertToPascalSnakeCase(str: string): string {
        return str
            .replace(/([A-Z])/g, ' $1') // insert a space before all caps
            .trim() // remove the leading space
            .replace(/ /g, ' '); // replace spaces with underscores
    }

    canDeactivate(): Promise<boolean> | boolean {
        this.hideMainSpinner();

        if (this.caseAdjusterForm.dirty) {
            var returnVal = this.swalAlert.fire({
                title: 'Are you sure you want to leave?',
                text: 'Your changes will be lost.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, proceed!',
            });

            return returnVal.then((result) => result.isConfirmed);
        } else {
            return true;
        }
    }

    // saveAndNew(): void {
    //     this.saving = true;

    //     this._caseAdjustersServiceProxy.createOrEdit(this.caseAdjuster)
    //         .pipe(finalize(() => {
    //             this.saving = false;
    //         }))
    //         .subscribe(x => {
    //             this.saving = false;
    //             this.notify.info(this.l('SavedSuccessfully'));
    //             this.caseAdjuster = new CreateOrEditCaseAdjusterDto();
    //         });
    // }
}
