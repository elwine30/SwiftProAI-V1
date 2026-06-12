import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import {
    DebitNoteItemsServiceProxy, CaseDebitNotesServiceProxy, 
    CreateOrEditCaseDebitNoteDto
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { Location } from '@angular/common';
import { NumberToWordsPipe } from '@shared/common/pipes/number-to-words.pipe';
import { CreateOrEditDebitNoteItemModalComponent } from '../debitNoteItems/create-or-edit-debitNoteItem-modal.component';
import { DebitNoteItemsComponent } from '../debitNoteItems/debitNoteItems.component';
import { EnumRegistrationStatus } from '@app/shared/common/registration/enum';
import { NgForm } from '@angular/forms';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';


@Component({
    templateUrl: './create-or-edit-caseDebitNote.component.html',
    animations: [appModuleAnimation()]
})
export class CreateOrEditCaseDebitNoteComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    @ViewChild('createOrEditDebitNoteItemModal', { static: true }) createOrEditDebitNoteItemModal: CreateOrEditDebitNoteItemModalComponent;
    @ViewChild('debitNoteItemsTable', { static: true }) debitNoteItemsTable: DebitNoteItemsComponent;
    caseDebitNote: CreateOrEditCaseDebitNoteDto = new CreateOrEditCaseDebitNoteDto();

    companyName = '';
    claimExecutiveUserName = '';
    adjusterUserName = '';
    caseTypeShortName = '';

    mainRegistrationVehicleNo = '';
    tenantCompanyName = '';
    sstRate: number;
    sstAmount: number;
    totalAmount: number;
    caseStatusId : number;


    totalInTextForm = '';

    sstCheckboxStates: { [key: string]: boolean } = {};
    includeSST: boolean = true;

    itemNames: { amount: string, sst: string }[] = [
        { amount: 'serviceAmount', sst: 'serviceSST' },
        { amount: 'mileageAmount', sst: 'mileageSST' },
        { amount: 'photographTotalPrice', sst: 'photographSST' },
        { amount: 'tollAmount', sst: 'tollSST' },
        { amount: 'bridgeTollAmount', sst: 'bridgeTollSST' },
        { amount: 'policeAmount', sst: 'policeSST' },
        { amount: 'statutoryDeclarationAmount', sst: 'statutoryDeclarationSST' },
        { amount: 'surveillanceAmount', sst: 'surveillanceSST' },
        { amount: 'telcoAmount', sst: 'telcoSST' },
        { amount: 'thirdPartyAmount', sst: 'thirdPartySST' },
        { amount: 'courtAttendanceAmount', sst: 'courtAttendanceSST' },
        { amount: 'searchFeeAmount', sst: 'searchFeeSST' },
        { amount: 'airFareAmount', sst: 'airFareSST' },
        { amount: 'charteredAmount', sst: 'charteredSST' },
        { amount: 'taxiFareAmount', sst: 'taxiFareSST' },
        { amount: 'accommodationAmount', sst: 'accommodationSST' },
        { amount: 'miscAmount', sst: 'miscSST' },
    ];


    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseDebitNotesServiceProxy: CaseDebitNotesServiceProxy,
        private _debitNoteItemsServiceProxy: DebitNoteItemsServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService,
        private _location: Location,
        private numberToWordsPipe: NumberToWordsPipe

    ) {
        super(injector);
    }

    initializeNewCaseDebitNote(): void {
        this.caseDebitNote.mileageUnitPrice = 0;
        this.caseDebitNote.serviceAmount = 0.00;
        this.caseDebitNote.serviceSST = 0.00;
        this.caseDebitNote.mileageKM = 0.00;
        this.caseDebitNote.mileageAmount = 0.00;
        this.caseDebitNote.mileageSST = 0.00;
        this.caseDebitNote.photographQty = 0.00;
        this.caseDebitNote.photographTotalPrice = 0.00;
        this.caseDebitNote.photographSST = 0.00;
        this.caseDebitNote.tollAmount = 0.00;
        this.caseDebitNote.tollSST = 0.00;
        this.caseDebitNote.bridgeTollAmount = 0.00;
        this.caseDebitNote.bridgeTollSST = 0.00;
        this.caseDebitNote.policeAmount = 0.00;
        this.caseDebitNote.policeSST = 0.00;
        this.caseDebitNote.statutoryDeclarationAmount = 0.00;
        this.caseDebitNote.statutoryDeclarationSST = 0.00;
        this.caseDebitNote.surveillanceAmount = 0.00;
        this.caseDebitNote.surveillanceSST = 0.00;
        this.caseDebitNote.telcoAmount = 0.00;
        this.caseDebitNote.telcoSST = 0.00;
        this.caseDebitNote.thirdPartyAmount = 0.00;
        this.caseDebitNote.thirdPartySST = 0.00;
        this.caseDebitNote.courtAttendanceAmount = 0.00;
        this.caseDebitNote.courtAttendanceSST = 0.00;
        this.caseDebitNote.searchFeeAmount = 0.00;
        this.caseDebitNote.searchFeeSST = 0.00;
        this.caseDebitNote.airFareAmount = 0.00;
        this.caseDebitNote.airFareSST = 0.00;
        this.caseDebitNote.charteredAmount = 0.00;
        this.caseDebitNote.charteredSST = 0.00;
        this.caseDebitNote.taxiFareAmount = 0.00;
        this.caseDebitNote.taxiFareSST = 0.00;
        this.caseDebitNote.accommodationAmount = 0.00;
        this.caseDebitNote.accommodationSST = 0.00;
        this.caseDebitNote.miscAmount = 0.00;
        this.caseDebitNote.miscSST = 0.00;
        this.caseDebitNote.amountExcludeSST = 0.00;
        this.caseDebitNote.amountWithSST = 0.00;
        this.caseDebitNote.totalAmount = 0.00;
        this.caseDebitNote.includeSST = this.includeSST;
        this.sstAmount = 0.00;
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(registerId?: number): void {
        this._caseDebitNotesServiceProxy.getCaseDebitNoteForEdit(registerId).subscribe(result => {
            this.caseDebitNote = result.caseDebitNote || new CreateOrEditCaseDebitNoteDto();

            if (!result.caseDebitNote) {
                this.initializeNewCaseDebitNote();
            }

            this.caseDebitNote.photographCharge = result.photographCharge;
            this.sstRate = result.sstRate;
            this.tenantCompanyName = result.tenantCompanyName;
            this.mainRegistrationVehicleNo = result.mainRegistrationVehicleNo;
            this.caseDebitNote.debitDate = this._dateTimeService.getStartOfDay();
            this.caseDebitNote.registerId = registerId;

            this.itemNames.forEach(item => {
                if (this.caseDebitNote[item.sst] === 0 && this.caseDebitNote[item.amount] === 0) {
                    this.sstCheckboxStates[item.sst] = true;
                } else {
                    this.sstCheckboxStates[item.sst] = this.caseDebitNote[item.sst] !== 0;
                }
                this.caseDebitNote[item.amount] = this.formatNumber(this.caseDebitNote[item.amount]);
                this.caseDebitNote[item.sst] = this.formatNumber(this.caseDebitNote[item.sst]);
            });

            this.updateDebitNoteItemTotal();
            this.updateTotals(this.caseDebitNote.includeSST);

            this.active = true;
        });
    }

    updateDebitNoteItemTotal() {
        this._debitNoteItemsServiceProxy.getDebitNoteItemAmountsByRegisterId(this._activatedRoute.snapshot.queryParams['id'])
            .subscribe(result => {
                const totals = result.reduce((acc, item) => {
                    acc[item.itemType] = (acc[item.itemType] || 0) + item.amount;
                    return acc;
                }, {});

                this.caseDebitNote.thirdPartyAmount = totals['ThirdParty'] || 0;
                this.caseDebitNote.searchFeeAmount = totals['SearchFee'] || 0;
                this.caseDebitNote.airFareAmount = totals['AirFare'] || 0;
                this.caseDebitNote.charteredAmount = totals['CharteredTransport'] || 0;
                this.caseDebitNote.taxiFareAmount = totals['TaxiFare'] || 0;
                this.caseDebitNote.accommodationAmount = totals['Accommodation'] || 0;
                this.caseDebitNote.miscAmount = totals['Miscellaneous'] || 0;

                const debitNoteItems = [
                    { amount: 'thirdPartyAmount', sst: 'thirdPartySST' },
                    { amount: 'searchFeeAmount', sst: 'searchFeeSST' },
                    { amount: 'airFareAmount', sst: 'airFareSST' },
                    { amount: 'charteredAmount', sst: 'charteredSST' },
                    { amount: 'taxiFareAmount', sst: 'taxiFareSST' },
                    { amount: 'accommodationAmount', sst: 'accommodationSST' },
                    { amount: 'miscAmount', sst: 'miscSST' }
                ];

                debitNoteItems.forEach(debitNoteItem => this.onDebitNoteItemAmountChange(debitNoteItem));
                this.updateTotals(this.caseDebitNote.includeSST);
            });

    }

    onDebitNoteItemAmountChange(item: { amount: string, sst: string }) {
        if (!this.sstCheckboxStates[item.sst]) {
            this.caseDebitNote[item.sst] = 0;
        } else {
            this.calculateItemSST(item);
        }
    }

    onItemAmountChange(item: { amount: string, sst: string }) {
        if (!this.sstCheckboxStates[item.sst]) {
            this.caseDebitNote[item.sst] = 0;
        } else {
            this.calculateItemSST(item);
        }
        this.updateTotals(this.caseDebitNote.includeSST);
    }



    toggleSST(e): void {
        var isIncludeSST = e.target.checked;
        this.caseDebitNote.includeSST = isIncludeSST;
        this.updateTotals(isIncludeSST);
        this.totalAmount = this.caseDebitNote.includeSST ? this.caseDebitNote.amountWithSST : this.caseDebitNote.amountExcludeSST;
        this.caseDebitNote.totalAmount = this.formatNumberDecimalPlaces(this.totalAmount,2)
        this.caseDebitNote.totalInTextForm = this.numberToWordsPipe.transform(this.caseDebitNote.totalAmount);
    }

    createDebitNoteItem(modal: any, itemType: string): void {
        modal.show(itemType, null);
    }

    private calculateItemSST(item: { amount: string, sst: string }) {
        let amount = this.caseDebitNote[item.amount];
        if (amount !== undefined) {
            this.caseDebitNote[item.amount] = this.formatNumber(amount);
            this.caseDebitNote[item.sst] =this.formatNumber(amount * this.sstRate); 
        }
    }

    private updateTotals(isIncludeSST: boolean = true) {
        let totalSST = 0;
        let totalAmountExcludingSST = 0.00;

        this.itemNames.forEach(item => {
            let amount = parseFloat(this.caseDebitNote[item.amount]) || 0;
            let sst = parseFloat(this.caseDebitNote[item.sst]) || 0;
            totalSST += sst;
            totalAmountExcludingSST += amount;
        });

        totalAmountExcludingSST =this.formatNumberDecimalPlaces(totalAmountExcludingSST,2);
        this.sstAmount = isIncludeSST ? this.formatNumberDecimalPlaces(totalSST,2) : 0.00; 
        this.caseDebitNote.amountExcludeSST = totalAmountExcludingSST;
        this.caseDebitNote.amountWithSST = totalAmountExcludingSST + this.sstAmount;

        if (this.caseDebitNote.includeSST) {
            this.caseDebitNote.totalAmount = this.caseDebitNote.amountWithSST;
        } else {
            this.caseDebitNote.totalAmount = this.caseDebitNote.amountExcludeSST;
        }
        this.caseDebitNote.totalInTextForm = this.numberToWordsPipe.transform(this.caseDebitNote.totalAmount);
    }

    onMileageKMChange() {
        this.caseDebitNote.mileageAmount = this.formatNumberDecimalPlaces(this.caseDebitNote.mileageKM * this.caseDebitNote.mileageUnitPrice,2);
        this.onItemAmountChange({ amount: 'mileageAmount', sst: 'mileageSST' });
    }

    onPhotographQtyChange() {
        this.caseDebitNote.photographTotalPrice =this.formatNumberDecimalPlaces(this.caseDebitNote.photographQty * this.caseDebitNote.photographCharge,2);
        this.onItemAmountChange({ amount: 'photographTotalPrice', sst: 'photographSST' });
    }


    save(): void {
        this.saving = true;
        this._caseDebitNotesServiceProxy.createOrEdit(this.caseDebitNote)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.caseDebitNote = new CreateOrEditCaseDebitNoteDto();
                this.show(this._activatedRoute.snapshot.queryParams['id']);
            })

    }

    preview(): void {
        this.saving = true;
        this._caseDebitNotesServiceProxy.createOrEdit(this.caseDebitNote)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.caseDebitNote = new CreateOrEditCaseDebitNoteDto();
                this.show(this._activatedRoute.snapshot.queryParams['id']);
                this._router.navigate(['/app/main/registration/caseDebitNotes/preview'], { queryParams: { id: this._activatedRoute.snapshot.queryParams['id'] } });  
            })
    }


    goBack(): void {
        this._router.navigate(['/app/main/dashboard'], { queryParams: { status: EnumRegistrationStatus[this.caseStatusId] } } );
    }

    formatNumberDecimalPlaces(number, n){
        if(number!== null && number !== undefined){
            return parseFloat(number.toFixed(n)); // return as Number data type to prevent error upon saving
        }
        return 0.00;
    }

    formatNumber(value: any): string {
        const parsedValue = parseFloat(value);
        if (isNaN(parsedValue)) {
          return '0.00';
        }
        return parsedValue.toFixed(2);
    }

}
