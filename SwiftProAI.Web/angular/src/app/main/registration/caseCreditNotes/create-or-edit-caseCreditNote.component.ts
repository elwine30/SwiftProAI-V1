import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import {
    CaseCreditNotesServiceProxy, CaseInvoicesServiceProxy, CreateOrEditCaseCreditNoteDto,
    CreditNoteItemsServiceProxy,
    InvoiceItemsServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { Location } from '@angular/common';
import { NumberToWordsPipe } from '@shared/common/pipes/number-to-words.pipe';
import { CreateOrEditCreditNoteItemModalComponent } from '../creditNoteItems/create-or-edit-creditNoteItem-modal.component';
import { CreditNoteItemsComponent } from '../creditNoteItems/creditNoteItems.component';
import { EnumRegistrationStatus } from '@app/shared/common/registration/enum';
import { NgForm } from '@angular/forms';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';


@Component({
    templateUrl: './create-or-edit-caseCreditNote.component.html',
    animations: [appModuleAnimation()]
})
export class CreateOrEditCaseCreditNoteComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    @ViewChild('creditNoteItemsTableThirdParty', { static: true }) creditNoteItemsTableThirdParty: CreditNoteItemsComponent;
    @ViewChild('creditNoteItemsTableSearchFee', { static: true }) creditNoteItemsTableSearchFee: CreditNoteItemsComponent;
    @ViewChild('creditNoteItemsTableAirFare', { static: true }) creditNoteItemsTableAirFare: CreditNoteItemsComponent;
    @ViewChild('creditNoteItemsTableTaxiFare', { static: true }) creditNoteItemsTableTaxiFare: CreditNoteItemsComponent;
    @ViewChild('creditNoteItemsTableAccommodation', { static: true }) creditNoteItemsTableAccommodation: CreditNoteItemsComponent;
    @ViewChild('creditNoteItemsTableMiscellaneous', { static: true }) creditNoteItemsTableMiscellaneous: CreditNoteItemsComponent;
    @ViewChild('creditNoteItemsTableCharteredTransport', { static: true }) creditNoteItemsTableCharteredTransport: CreditNoteItemsComponent;
    
    @ViewChild('createOrEditCreditNoteItemModal', { static: true }) createOrEditCreditNoteItemModal: CreateOrEditCreditNoteItemModalComponent;
    caseCreditNote: CreateOrEditCaseCreditNoteDto = new CreateOrEditCaseCreditNoteDto();

    companyName = '';
    claimExecutiveUserName = '';
    adjusterUserName = '';
    caseTypeShortName = '';

    mainRegistrationVehicleNo = '';
    tenantCompanyName = '';
    sstRate: number;
    sstAmount: number;
    totalAmount: number;
    loadInvoice: boolean = false;
    caseStatusId: number;

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
        private _caseInvoicesServiceProxy: CaseInvoicesServiceProxy,
        private _invoiceItemsServiceProxy: InvoiceItemsServiceProxy,
        private _caseCreditNotesServiceProxy: CaseCreditNotesServiceProxy,
        private _creditNoteItemsServiceProxy: CreditNoteItemsServiceProxy,
        private numberToWordsPipe: NumberToWordsPipe,
        private _location: Location,
        private _router: Router,
        private _dateTimeService: DateTimeService

    ) {
        super(injector);

    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(registerId?: number): void {
        this._caseCreditNotesServiceProxy.getCaseCreditNoteForEdit(registerId).subscribe(result => {
            this.caseCreditNote = result.caseCreditNote;

            if (!this.caseCreditNote) {

                //copy creditnoteItems from invoiceItems for initialization
                this._creditNoteItemsServiceProxy.bulkCreate(registerId).subscribe(() => {
                    this.creditNoteItemsTableAccommodation.getCreditNoteItems();
                    this.creditNoteItemsTableAirFare.getCreditNoteItems();
                    this.creditNoteItemsTableCharteredTransport.getCreditNoteItems();
                    this.creditNoteItemsTableMiscellaneous.getCreditNoteItems();
                    this.creditNoteItemsTableSearchFee.getCreditNoteItems();
                    this.creditNoteItemsTableTaxiFare.getCreditNoteItems();
                    this.creditNoteItemsTableThirdParty.getCreditNoteItems();
                });

                this.caseCreditNote = new CreateOrEditCaseCreditNoteDto();
                this.caseCreditNote.mileageUnitPrice = result.mileageUnitPrice;
                this.caseCreditNote.serviceAmount = 0.00;
                this.caseCreditNote.serviceSST = 0.00;
                this.caseCreditNote.mileageKM = 0.00;
                this.caseCreditNote.mileageAmount = 0.00;
                this.caseCreditNote.mileageSST = 0.00;
                this.caseCreditNote.photographQty = 0.00;
                this.caseCreditNote.photographTotalPrice = 0.00;
                this.caseCreditNote.photographSST = 0.00;
                this.caseCreditNote.tollAmount = 0.00;
                this.caseCreditNote.tollSST = 0.00;
                this.caseCreditNote.bridgeTollAmount = 0.00;
                this.caseCreditNote.bridgeTollSST = 0.00;
                this.caseCreditNote.policeAmount = 0.00;
                this.caseCreditNote.policeSST = 0.00;
                this.caseCreditNote.statutoryDeclarationAmount = 0.00;
                this.caseCreditNote.statutoryDeclarationSST = 0.00;
                this.caseCreditNote.surveillanceAmount = 0.00;
                this.caseCreditNote.surveillanceSST = 0.00;
                this.caseCreditNote.telcoAmount = 0.00;
                this.caseCreditNote.telcoSST = 0.00;
                this.caseCreditNote.thirdPartyAmount = 0.00;   // From table
                this.caseCreditNote.thirdPartySST = 0.00;
                this.caseCreditNote.courtAttendanceAmount = 0.00;
                this.caseCreditNote.courtAttendanceSST = 0.00;
                this.caseCreditNote.searchFeeAmount = 0.00;    // From table
                this.caseCreditNote.searchFeeSST = 0.00;
                this.caseCreditNote.airFareAmount = 0.00;      // From table
                this.caseCreditNote.airFareSST = 0.00;
                this.caseCreditNote.charteredAmount = 0.00;    // From table
                this.caseCreditNote.charteredSST = 0.00;
                this.caseCreditNote.taxiFareAmount = 0.00;     // From table
                this.caseCreditNote.taxiFareSST = 0.00;
                this.caseCreditNote.accommodationAmount = 0.00;    // From table
                this.caseCreditNote.accommodationSST = 0.00;
                this.caseCreditNote.miscAmount = 0.00;         // From table
                this.caseCreditNote.miscSST = 0.00;

                this.caseCreditNote.amountExcludeSST = 0.00;
                this.caseCreditNote.amountWithSST = 0.00;
                this.caseCreditNote.totalAmount = 0.00;
                this.caseCreditNote.includeSST = this.includeSST;
                this.sstAmount = 0.00;
                
                this.loadInvoice = true;
                this._caseInvoicesServiceProxy.getCaseInvoiceForEdit(registerId).subscribe(result => {
                if (result.caseInvoice) {
                        this.caseCreditNote = new CreateOrEditCaseCreditNoteDto(result.caseInvoice);
                        this.caseCreditNote.id = null;
                    }
                });
            }

            this.caseStatusId = result.caseStatusId;
            this.sstRate = result.sstRate;
            this.tenantCompanyName = result.tenantCompanyName;
            this.mainRegistrationVehicleNo = result.mainRegistrationVehicleNo;
            this.caseCreditNote.debitDate = this._dateTimeService.getStartOfDay();
            this.caseCreditNote.registerId = registerId;
            this.caseCreditNote.photographCharge = result.photographCharge;
            this.active = true;

            this.itemNames.forEach(item => {
                if (this.caseCreditNote[item.sst] === 0 && this.caseCreditNote[item.amount] === 0) {
                    this.sstCheckboxStates[item.sst] = true;
                } else {
                    this.sstCheckboxStates[item.sst] = this.caseCreditNote[item.sst] !== 0;
                }
                this.caseCreditNote[item.amount] = this.formatNumber(this.caseCreditNote[item.amount]);
                this.caseCreditNote[item.sst] = this.formatNumber(this.caseCreditNote[item.sst]);
            });
            
            this.updateCreditNoteItemTotal();            
            this.updateTotals(this.caseCreditNote.includeSST);
        });

    }

    updateInvoiceItemTotal() {
        this._invoiceItemsServiceProxy.getInvoiceItemAmountsByRegisterId(this._activatedRoute.snapshot.queryParams['id'])
            .subscribe(result => {
                const totals = result.reduce((acc, item) => {
                    acc[item.itemType] = (acc[item.itemType] || 0) + item.amount;
                    return acc;
                }, {});

                this.caseCreditNote.thirdPartyAmount = totals['ThirdParty'] || 0;
                this.caseCreditNote.searchFeeAmount = totals['SearchFee'] || 0;
                this.caseCreditNote.airFareAmount = totals['AirFare'] || 0;
                this.caseCreditNote.charteredAmount = totals['CharteredTransport'] || 0;
                this.caseCreditNote.taxiFareAmount = totals['TaxiFare'] || 0;
                this.caseCreditNote.accommodationAmount = totals['Accommodation'] || 0;
                this.caseCreditNote.miscAmount = totals['Miscellaneous'] || 0;

                const creditNoteItems = [
                    { amount: 'thirdPartyAmount', sst: 'thirdPartySST' },
                    { amount: 'searchFeeAmount', sst: 'searchFeeSST' },
                    { amount: 'airFareAmount', sst: 'airFareSST' },
                    { amount: 'charteredAmount', sst: 'charteredSST' },
                    { amount: 'taxiFareAmount', sst: 'taxiFareSST' },
                    { amount: 'accommodationAmount', sst: 'accommodationSST' },
                    { amount: 'miscAmount', sst: 'miscSST' }
                ];

                creditNoteItems.forEach(creditNoteItem => this.onCreditNoteItemAmountChange(creditNoteItem));
                this.updateTotals(this.caseCreditNote.includeSST);
            });

    }
    updateCreditNoteItemTotal() {
        this._creditNoteItemsServiceProxy.getCreditNoteItemAmountsByRegisterId(this._activatedRoute.snapshot.queryParams['id'])
            .subscribe(result => {
                const totals = result.reduce((acc, item) => {
                    acc[item.itemType] = (acc[item.itemType] || 0) + item.amount;
                    return acc;
                }, {});

                this.caseCreditNote.thirdPartyAmount = totals['ThirdParty'] || 0;
                this.caseCreditNote.searchFeeAmount = totals['SearchFee'] || 0;
                this.caseCreditNote.airFareAmount = totals['AirFare'] || 0;
                this.caseCreditNote.charteredAmount = totals['CharteredTransport'] || 0;
                this.caseCreditNote.taxiFareAmount = totals['TaxiFare'] || 0;
                this.caseCreditNote.accommodationAmount = totals['Accommodation'] || 0;
                this.caseCreditNote.miscAmount = totals['Miscellaneous'] || 0;

                const creditNoteItems = [
                    { amount: 'thirdPartyAmount', sst: 'thirdPartySST' },
                    { amount: 'searchFeeAmount', sst: 'searchFeeSST' },
                    { amount: 'airFareAmount', sst: 'airFareSST' },
                    { amount: 'charteredAmount', sst: 'charteredSST' },
                    { amount: 'taxiFareAmount', sst: 'taxiFareSST' },
                    { amount: 'accommodationAmount', sst: 'accommodationSST' },
                    { amount: 'miscAmount', sst: 'miscSST' }
                ];

                creditNoteItems.forEach(creditNoteItem => this.onCreditNoteItemAmountChange(creditNoteItem));
                this.updateTotals(this.caseCreditNote.includeSST);
            });

    }

    private updateTotals(isIncludeSST: boolean = true) {
        let totalSST = 0;
        let totalAmountExcludingSST = 0;

        this.itemNames.forEach(item => {
            
            let amount = parseFloat(this.caseCreditNote[item.amount]) || 0;
            let sst = parseFloat(this.caseCreditNote[item.sst]) || 0;
            totalSST += sst;
            totalAmountExcludingSST += amount;
        });

        totalAmountExcludingSST = this.formatNumberDecimalPlaces(totalAmountExcludingSST,2); 
        this.sstAmount = isIncludeSST ? this.formatNumberDecimalPlaces(totalSST,2) : 0.00;
        this.caseCreditNote.amountExcludeSST = totalAmountExcludingSST;
        this.caseCreditNote.amountWithSST = totalAmountExcludingSST + this.sstAmount;

        if (this.caseCreditNote.includeSST) {
            this.totalAmount = this.caseCreditNote.amountWithSST;
        } else {
            this.totalAmount = this.caseCreditNote.amountExcludeSST;
        }
        this.caseCreditNote.totalAmount = this.formatNumberDecimalPlaces(this.totalAmount,2);
        this.caseCreditNote.totalInTextForm = this.numberToWordsPipe.transform(this.caseCreditNote.totalAmount);
    }

    private calculateItemSST(item: { amount: string, sst: string }) {
        let amount = this.caseCreditNote[item.amount];
        if (amount !== undefined) {
            this.caseCreditNote[item.sst] = this.formatNumberDecimalPlaces(amount * this.sstRate,2);
        }
    }

    onCreditNoteItemAmountChange(item: { amount: string, sst: string }) {
        if (!this.sstCheckboxStates[item.sst]) {
            this.caseCreditNote[item.sst] = 0;
        } else {
            this.calculateItemSST(item);
        }
    }

    onItemAmountChange(item: { amount: string, sst: string }) {
        if (!this.sstCheckboxStates[item.sst]) {
            this.caseCreditNote[item.sst] = 0;
        } else {
            this.calculateItemSST(item);
        }
        this.updateTotals(this.caseCreditNote.includeSST);
    }

    createCreditNoteItem(modal: any, itemType: string): void {
        modal.show(itemType, null);
    }

    toggleSST(e): void {
        var isIncludeSST = e.target.checked;
        this.caseCreditNote.includeSST = isIncludeSST;
        this.updateTotals(isIncludeSST);
        this.totalAmount = this.caseCreditNote.includeSST ? this.caseCreditNote.amountWithSST : this.caseCreditNote.amountExcludeSST;
        this.caseCreditNote.totalAmount = this.formatNumberDecimalPlaces(this.totalAmount,2);
        this.caseCreditNote.totalInTextForm = this.numberToWordsPipe.transform(this.caseCreditNote.totalAmount);
    }

    onMileageKMChange() {
        this.caseCreditNote.mileageAmount = this.formatNumberDecimalPlaces(this.caseCreditNote.mileageKM * this.caseCreditNote.mileageUnitPrice,2);
        this.onItemAmountChange({ amount: 'mileageAmount', sst: 'mileageSST' });
    }

    onPhotographQtyChange() {
        this.caseCreditNote.photographTotalPrice =this.formatNumberDecimalPlaces(this.caseCreditNote.photographQty * this.caseCreditNote.photographCharge,2);
        this.onItemAmountChange({ amount: 'photographTotalPrice', sst: 'photographSST' });
    }

    save(): void {
        this.saving = true;

        this._caseCreditNotesServiceProxy.createOrEdit(this.caseCreditNote)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.caseCreditNote = new CreateOrEditCaseCreditNoteDto();
                this.show(this._activatedRoute.snapshot.queryParams['id']);
            })
    }

    preview(): void {
        this.save();

        this._router.navigate(['/app/main/registration/caseCreditNotes/preview'], { queryParams: { id: this._activatedRoute.snapshot.queryParams['id'] } });  
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
