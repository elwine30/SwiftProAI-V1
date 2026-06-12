import { Component, ViewChild, Injector, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    CaseClaimsServiceProxy,
    CaseSearchFeesServiceProxy,
    CreateOrEditCaseClaimDto,
    LookupsServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';

import { Paginator } from 'primeng/paginator';
import { StepNavComponent } from '@app/shared/common/registration/step-nav.component';
import { CreateOrEditCaseSearchFeeModalComponent } from '../caseSearchFees/create-or-edit-caseSearchFee-modal.component';
import { CaseSearchFeesComponent } from '../caseSearchFees/caseSearchFees.component';
import { ExpensessStatusGroupType } from '@app/shared/common/registration/enum';
import { CaseClaimDataService } from './caseClaimDataService';
import { Location } from '@angular/common';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { NgForm } from '@angular/forms';

@Component({
    templateUrl: './create-or-edit-caseClaim.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditCaseClaimComponent extends AppComponentBase implements OnInit, DirtyFormGuard {
    active = false;
    saving = false;

    @ViewChild('stepNav') stepNav: StepNavComponent;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('createOrEditCaseSearchFeeModal', { static: true })
    createOrEditCaseSearchFeeModal: CreateOrEditCaseSearchFeeModalComponent;
    @ViewChild('caseSearchFeesTable', { static: true }) caseSearchFeesTable: CaseSearchFeesComponent;
    @ViewChild('caseClaimForm') caseClaimForm: NgForm;

    caseClaim: CreateOrEditCaseClaimDto = new CreateOrEditCaseClaimDto();
    mileageTotal: number;
    total: number;
    searchFeeTotal: number;
    mileageUnitPrice: number;

    expensessStatusGroupList: any;

    EXP001Id: number;
    EXP002Id: number;

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseClaimsServiceProxy: CaseClaimsServiceProxy,
        private _caseClaimDataService: CaseClaimDataService,
        private _location: Location,
        private _lookupService: LookupsServiceProxy,
        private _caseSearchFeeService: CaseSearchFeesServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
        this._lookupService.getByGroup('ExpensesStatus').subscribe((data) => {
            this.expensessStatusGroupList = data;
            this.expensessStatusGroupList.forEach((item) => {
                if (item.code === 'EXP001') {
                    this.EXP001Id = this.expensessStatusGroupList.id;
                } else if (item.code === 'EXP002') {
                    this.EXP002Id = this.expensessStatusGroupList.id;
                }
            });
            this.markFormAsPristine(this.caseClaimForm);
        });
    }

    onMileageKMChange() {
        this.mileageTotal = this.formatNumberDecimalPlaces(this.caseClaim.mileageKM * this.caseClaim.mileageUnitPrice,2);
        this.updateTotal();
    }

    updateTotal(): void {
        this.total = 
            (Number(this.caseClaim.fraudAmount) || 0) +
            (Number(this.caseClaim.fileCharges) || 0) +
            (Number(this.caseClaim.police) || 0) +
            (Number(this.caseClaim.airFare) || 0) +
            (Number(this.caseClaim.charteredTransport) || 0) +
            (Number(this.caseClaim.toll) || 0) +
            (Number(this.caseClaim.hotel) || 0) +
            (Number(this.caseClaim.sd) || 0) +
            (Number(this.caseClaim.searchFee) || 0) +
            (Number(this.mileageTotal) || 0);
        this.total = this.formatNumberDecimalPlaces(this.total, 2);
    }

    initializeNewCaseClaim(): void {
        this.caseClaim.fraudAmount = 0.0;
        this.caseClaim.fileCharges = 0.0;
        this.caseClaim.mileageKM = 0.0;
        this.caseClaim.police = 0.0;
        this.caseClaim.airFare = 0.0;
        this.caseClaim.charteredTransport = 0.0;
        this.caseClaim.toll = 0.0;
        this.caseClaim.hotel = 0.0;
        this.caseClaim.sd = 0.0;
        this.caseClaim.searchFee = 0.0;
        this.caseClaim.mileageTotal = 0.0;
        this.caseClaim.statusId = null;
        this.mileageTotal = 0.0;
    }

    show(registerId?: number): void {
        this._caseClaimsServiceProxy.getCaseClaimForEdit(registerId).subscribe((result) => {
            this.caseClaim = result.caseClaim || new CreateOrEditCaseClaimDto()

            if (!this.caseClaim) {

                this.caseClaim.mileageUnitPrice = result.mileageUnitPrice;
                this.initializeNewCaseClaim();
            }
            else{ // add null checking to prevent read undefined properties
                this.mileageTotal = this.caseClaim.mileageTotal;
                this.total = this.caseClaim.total;
                this.searchFeeTotal = this.caseClaim.searchFee;
                this.caseClaim.mileageUnitPrice = result.mileageUnitPrice;
                this._caseClaimDataService.changeStatusId(this.caseClaim.statusId);
                this.caseClaim.registerId = registerId;
                this.updateTotal();
                this.active = true;
                this.updateSearchfeeTotal();

            }

        });
    }

    createCaseSearchFee(): void {
        this.createOrEditCaseSearchFeeModal.show();
    }

    reloadPage(): void {
        location.reload();
    }

    save(): void {
        this.saving = true;

        //Reset the value of fraud amount back to 0 if is not fraud
        if (!this.caseClaim.fraud) {
            this.caseClaim.fraudAmount = 0.0;
        } 

        this._caseClaimsServiceProxy
            .createOrEdit(this.caseClaim)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.caseClaim = new CreateOrEditCaseClaimDto();
                this.markFormAsPristine(this.caseClaimForm);
                this.show(this._activatedRoute.snapshot.queryParams['id']);
            });
    }

    submit(): void {
        this.message.confirm(this.l('SubmitClaimsReminder'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                const statusType = this.expensessStatusGroupList.find(
                    (option: { code: string }) => option.code == ExpensessStatusGroupType.PendingForApproval,
                );

                this.caseClaim.statusId = statusType.id;
                this._caseClaimDataService.changeStatusId(this.caseClaim.statusId);
                this.save();
                this.notify.success(this.l('SuccessfullySubmitted'));
                this.reloadPage();
            }
        });
    }

    goBack(): void {
        this._location.back();
    }

    canDeactivate(): boolean {
        this.hideMainSpinner();
        return !this.caseClaimForm.dirty || confirm('Are you sure you want to leave? Your changes will be lost.');
    }

    formatNumberDecimalPlaces(number, n) {
        if (number !== null && number !== undefined) {
            return parseFloat(number.toFixed(n)); // return as Number data type to prevent error upon saving
        }
        return 0.0;
    }

    formatNumber(value: any): string {
        const parsedValue = parseFloat(value);
        if (isNaN(parsedValue)) {
            return '0.00';
        }
        return parsedValue.toFixed(2);
    }

    updateSearchfeeTotal(){
        this._caseSearchFeeService.getCaseSearchFeeAmountByRegisterId(this._activatedRoute.snapshot.queryParams['id'])
        .subscribe(result => {
            this.caseClaim.searchFee = result.reduce((sum, item) => sum + item.amount, 0);
            this.updateTotal();
        })        

    }

}
