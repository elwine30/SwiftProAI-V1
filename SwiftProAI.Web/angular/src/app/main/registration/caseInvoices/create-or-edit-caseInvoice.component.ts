import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    CreateOrEditCaseClaimDto, InvoiceItemsServiceProxy, ThemeSettingsDto
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import {
    CaseInvoicesServiceProxy, CreateOrEditCaseInvoiceDto, CaseInvoiceMainRegistrationLookupTableDto
    , CaseInvoiceUserLookupTableDto
    , CaseInvoiceCaseTypeLookupTableDto
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from "@node_modules/rxjs";
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { Location } from '@angular/common';
import { NumberToWordsPipe } from '@shared/common/pipes/number-to-words.pipe';
import { InvoiceItemsComponent } from '../invoiceItems/invoiceItems.component';
import { CreateOrEditInvoiceItemModalComponent } from '../invoiceItems/create-or-edit-invoiceItem-modal.component';
import { EnumRegistrationStatus } from '@app/shared/common/registration/enum';
import { NgForm } from '@angular/forms';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';


@Component({
    templateUrl: './create-or-edit-caseInvoice.component.html',
    animations: [appModuleAnimation()]
})
export class CreateOrEditCaseInvoiceComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    @ViewChild('createOrEditInvoiceItemModal', { static: true }) createOrEditInvoiceItemModal: CreateOrEditInvoiceItemModalComponent;
    @ViewChild('invoiceItemsTable', { static: true }) invoiceItemsTable: InvoiceItemsComponent;
    caseInvoice: CreateOrEditCaseInvoiceDto = new CreateOrEditCaseInvoiceDto();
    caseClaim: CreateOrEditCaseClaimDto = new CreateOrEditCaseClaimDto();

    companyName = '';
    claimExecutiveUserName = '';
    adjusterUserName = '';
    caseTypeShortName = '';

    mainRegistrationVehicleNo = '';
    tenantCompanyName = '';
    sstRate: number;
    sstAmount: number;
    totalAmount: number;
    caseStatusId: number;

    totalInTextForm = '';

    acknowledgeDependency: boolean = false;
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
        private _router: Router,
        private _dateTimeService: DateTimeService,
        private _location: Location,
        private numberToWordsPipe: NumberToWordsPipe

    ) {
        super(injector);

    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(registerId?: number): void {
        this._caseInvoicesServiceProxy.getCaseInvoiceForEdit(registerId).subscribe(result => {
            this.caseClaim = result.caseClaim;
            this.caseInvoice = result.caseInvoice;

            if (!this.caseInvoice) {
                this.caseInvoice = new CreateOrEditCaseInvoiceDto();
                this.caseInvoice.mileageUnitPrice = result.mileageUnitPrice;
                this.caseInvoice.serviceAmount = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.serviceSST = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.mileageKM = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.mileageAmount = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.mileageSST = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.photographQty = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.photographTotalPrice = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.photographSST = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.tollAmount = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.tollSST = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.bridgeTollAmount = 0.00;
                this.caseInvoice.bridgeTollSST = 0.00;
                this.caseInvoice.policeAmount =  this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.policeSST = 0.00;
                this.caseInvoice.statutoryDeclarationAmount = 0.00;
                this.caseInvoice.statutoryDeclarationSST = 0.00;
                this.caseInvoice.surveillanceAmount = 0.00;
                this.caseInvoice.surveillanceSST = 0.00;
                this.caseInvoice.telcoAmount = 0.00;
                this.caseInvoice.telcoSST = 0.00;
                this.caseInvoice.thirdPartyAmount = 0.00;   // From table
                this.caseInvoice.thirdPartySST = 0.00;
                this.caseInvoice.courtAttendanceAmount = 0.00;
                this.caseInvoice.courtAttendanceSST = 0.00;
                this.caseInvoice.searchFeeAmount = 0.00;    // From table
                this.caseInvoice.searchFeeSST = 0.00;
                this.caseInvoice.airFareAmount = 0.00;      // From table
                this.caseInvoice.airFareSST = 0.00;
                this.caseInvoice.charteredAmount = 0.00;    // From table
                this.caseInvoice.charteredSST = 0.00;
                this.caseInvoice.taxiFareAmount = 0.00;     // From table
                this.caseInvoice.taxiFareSST = 0.00;
                this.caseInvoice.accommodationAmount = 0.00;    // From table
                this.caseInvoice.accommodationSST = 0.00;
                this.caseInvoice.miscAmount = 0.00;         // From table
                this.caseInvoice.miscSST = 0.00;

                this.caseInvoice.amountExcludeSST = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.amountWithSST = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.totalAmount = this.formatNumberDecimalPlaces(0.00,2);
                this.caseInvoice.includeSST = this.includeSST;
                this.sstAmount = this.formatNumberDecimalPlaces(0.00,2);

            } else {

            }

            if (!this.caseClaim) {
                this.caseClaim = new CreateOrEditCaseClaimDto();
                // Print warning where claim hasnt been made.
            }

            this.caseStatusId = result.caseStatusId;
            this.sstRate = result.sstRate;
            this.tenantCompanyName = result.tenantCompanyName;
            this.mainRegistrationVehicleNo = result.mainRegistrationVehicleNo;
            this.caseInvoice.invoiceDate = this._dateTimeService.getStartOfDay();
            this.caseInvoice.registerId = registerId;
            this.caseInvoice.photographCharge = result.photographCharge;
            this.active = true;

            this.itemNames.forEach(item => {
                if (this.caseInvoice[item.sst] == 0) {
                    // If the sst is 0, check the amount. If the amount is also 0, set the checkbox state to true.
                    if (this.caseInvoice[item.amount] == 0) {
                        this.sstCheckboxStates[item.sst] = true;
                    } else {
                        this.sstCheckboxStates[item.sst] = false;
                    }
                } else {
                    // If the sst is not 0, then set the checkbox state to true.
                    this.sstCheckboxStates[item.sst] = true;
                }
            });
            this.updateInvoiceItemTotal();
            this.updateTotals(this.caseInvoice.includeSST);
        });

    }

    updateInvoiceItemTotal() {
        this._invoiceItemsServiceProxy.getInvoiceItemAmountsByRegisterId(this._activatedRoute.snapshot.queryParams['id'])
            .subscribe(result => {
                const totals = result.reduce((acc, item) => {
                    acc[item.itemType] = (acc[item.itemType] || 0.00) + item.amount;
                    return acc;
                }, {});

                this.caseInvoice.thirdPartyAmount = totals['ThirdParty'] || 0.00;
                this.caseInvoice.searchFeeAmount = totals['SearchFee'] || 0.00;
                this.caseInvoice.airFareAmount = totals['AirFare'] || 0.00;
                this.caseInvoice.charteredAmount = totals['CharteredTransport'] || 0.00;
                this.caseInvoice.taxiFareAmount = totals['TaxiFare'] || 0.00;
                this.caseInvoice.accommodationAmount = totals['Accommodation'] || 0.00;
                this.caseInvoice.miscAmount = totals['Miscellaneous'] || 0.00;

                const invoiceItems = [
                    { amount: 'thirdPartyAmount', sst: 'thirdPartySST' },
                    { amount: 'searchFeeAmount', sst: 'searchFeeSST' },
                    { amount: 'airFareAmount', sst: 'airFareSST' },
                    { amount: 'charteredAmount', sst: 'charteredSST' },
                    { amount: 'taxiFareAmount', sst: 'taxiFareSST' },
                    { amount: 'accommodationAmount', sst: 'accommodationSST' },
                    { amount: 'miscAmount', sst: 'miscSST' }
                ];

                invoiceItems.forEach(invoiceItem => this.onInvoiceItemAmountChange(invoiceItem));
                this.updateTotals(this.caseInvoice.includeSST);
            });

    }

    onInvoiceItemAmountChange(item: { amount: string, sst: string }) {        
        if (!this.sstCheckboxStates[item.sst]) {
            this.caseInvoice[item.sst] = 0.00;
        } else {
            this.calculateItemSST(item);
        }
    }

    onItemAmountChange(item: { amount: string, sst: string }) {
        if (!this.sstCheckboxStates[item.sst]) {
            this.caseInvoice[item.sst] = 0.00;
        } else {
            this.calculateItemSST(item);
        }
        this.updateTotals(this.caseInvoice.includeSST);
    }



    toggleSST(e): void {
        var isIncludeSST = e.target.checked;
        this.caseInvoice.includeSST = isIncludeSST;
        this.updateTotals(isIncludeSST);
        this.totalAmount = isIncludeSST ? this.caseInvoice.amountWithSST : this.caseInvoice.amountExcludeSST;
        this.caseInvoice.totalAmount = this.formatNumberDecimalPlaces(this.totalAmount,2);
        this.caseInvoice.totalInTextForm = this.numberToWordsPipe.transform(this.caseInvoice.totalAmount);
    }

    createInvoiceItem(modal: any, itemType: string): void {
        modal.show(itemType, null);
    }

    private calculateItemSST(item: { amount: string, sst: string }) {
        let amount = this.caseInvoice[item.amount];
        if (amount !== undefined) {
            this.caseInvoice[item.amount] =this.formatNumber(amount); // round off to two decimals for item amount
            this.caseInvoice[item.sst] =this.formatNumber(amount * this.sstRate); 
        }
    }

    private updateTotals(isIncludeSST: boolean = true) {
        let totalSST = 0.00;
        let totalAmountExcludingSST = 0.00;

        this.itemNames.forEach(item => {
            let amount = parseFloat(this.caseInvoice[item.amount]) || 0.00;
            let sst = parseFloat(this.caseInvoice[item.sst]) || 0.00;
            totalSST += sst;
            totalAmountExcludingSST += amount;
        });

        totalAmountExcludingSST =this.formatNumberDecimalPlaces(totalAmountExcludingSST,2);
        this.sstAmount = isIncludeSST ? this.formatNumberDecimalPlaces(totalSST,2) : 0.00; 
        
        this.caseInvoice.amountExcludeSST = totalAmountExcludingSST;
        this.caseInvoice.amountWithSST = totalAmountExcludingSST + this.sstAmount;

        if (this.caseInvoice.includeSST) {
            this.totalAmount = this.caseInvoice.amountWithSST;
        } else {
            this.totalAmount = this.caseInvoice.amountExcludeSST;
        }
        this.caseInvoice.totalAmount =this.formatNumberDecimalPlaces(this.totalAmount,2); 
        this.caseInvoice.totalInTextForm = this.numberToWordsPipe.transform(this.caseInvoice.totalAmount);
    }

    onMileageKMChange() {
        this.caseInvoice.mileageAmount =this.formatNumberDecimalPlaces(this.caseInvoice.mileageKM * this.caseInvoice.mileageUnitPrice,2);
        this.onItemAmountChange({ amount: 'mileageAmount', sst: 'mileageSST' });
    }

    onPhotographQtyChange() {
        this.caseInvoice.photographTotalPrice = this.formatNumberDecimalPlaces(this.caseInvoice.photographQty * this.caseInvoice.photographCharge,2);
        this.onItemAmountChange({ amount: 'photographTotalPrice', sst: 'photographSST' });
    }


    save(): void {
        this.saving = true;
        this._caseInvoicesServiceProxy.createOrEdit(this.caseInvoice)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {
                 this.saving = false;               
                 this.notify.info(this.l('SavedSuccessfully'));
                 this.caseClaim = new CreateOrEditCaseClaimDto();
                 this.caseInvoice = new CreateOrEditCaseInvoiceDto();
                 this.show(this._activatedRoute.snapshot.queryParams['id']);
            })

    }

    preview(): void {
        this.saving = true;
        this._caseInvoicesServiceProxy.createOrEdit(this.caseInvoice)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.caseClaim = new CreateOrEditCaseClaimDto();
                this.caseInvoice = new CreateOrEditCaseInvoiceDto();
                this.show(this._activatedRoute.snapshot.queryParams['id']);
                this._router.navigate(['/app/main/registration/caseInvoices/preview'], { queryParams: { id: this._activatedRoute.snapshot.queryParams['id'] } });
            })
    }


    goBack(): void {
        this._router.navigate(['/app/main/dashboard'], { queryParams: { status: EnumRegistrationStatus[this.caseStatusId] } } );
      }

      formatNumberDecimalPlaces(number: number, n: number): number {
        if (number !== null && number !== undefined) {
            const formattedValue = parseFloat(number.toFixed(n));
            return formattedValue; // return as Number data type to prevent error upon saving
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
