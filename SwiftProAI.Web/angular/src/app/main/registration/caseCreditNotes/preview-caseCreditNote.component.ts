import { AppConsts } from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CaseCreditNotesServiceProxy, GetCaseCreditNoteForPreviewDto, CreditNoteItemAmountDto, CreateOrEditCaseCreditNoteDto, CreditNoteItemsServiceProxy, CreditNoteItemDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
    templateUrl: './preview-caseCreditNote.component.html',
    animations: [appModuleAnimation()]
})
export class PreviewCaseCreditNoteComponent extends AppComponentBase implements OnInit {

    active = false;
    saving = false;

    caseCreditNote: CreateOrEditCaseCreditNoteDto = new CreateOrEditCaseCreditNoteDto();
    tenantCompanyName = "";
    tenantBusinessRegistrationNo = "";
    tenantCompanyTaxVatNo = "";
    tenantCompanyAddress = "";
    tenantCompanyTelNo = "";

    companyAddress = "";
    companyName = "";
    companySstRegNo = "";

    insuredPersonName = "";
    insuredPersonPolicyNo = "";

    mainRegistrationVehicleNo = "";
    fileRefNo = "";
    creditNoteDate = "";
    sstAmount : number;


    thirdPartyItemList : CreditNoteItemDto[];
    searchFeeItemList : CreditNoteItemDto[];
    airFareItemList : CreditNoteItemDto[];
    charteredItemList : CreditNoteItemDto[];
    taxiFareItemList : CreditNoteItemDto[];
    accommodationItemList : CreditNoteItemDto[];
    miscItemList : CreditNoteItemDto[];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseCreditNotesServiceProxy: CaseCreditNotesServiceProxy,
        private _creditNoteItemsServiceProxy: CreditNoteItemsServiceProxy,
        private _router: Router,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(caseCreditNoteId: number): void {
        this._caseCreditNotesServiceProxy.getCaseCreditNoteForPreview(caseCreditNoteId).subscribe(result => {
            this.caseCreditNote = result.caseCreditNote;
            if (this.caseCreditNote){
                this.tenantCompanyName = result.tenantCompanyName;
                this.tenantBusinessRegistrationNo = result.tenantBusinessRegistrationNo;
                this.tenantCompanyTaxVatNo = result.tenantCompanyTaxVatNo;
                this.tenantCompanyAddress = result.tenantCompanyAddress;
                this.tenantCompanyTelNo = result.tenantCompanyTelNo;

                this.companyAddress = result.companyAddress;
                this.companyName = result.companyName; 
                this.companySstRegNo = result.companySstRegNo;

                this.insuredPersonName = result.insuredPersonName;
                this.insuredPersonPolicyNo = result.insuredPersonPolicyNo;
                this.fileRefNo = result.fileRefNo;
                this.mainRegistrationVehicleNo = result.mainRegistrationVehicleNo;
                this.creditNoteDate = this.caseCreditNote.debitDate.toISODate();

                this.sstAmount = parseFloat((this.caseCreditNote.amountWithSST - this.caseCreditNote.amountExcludeSST).toFixed(2));
            }
            
            this.searchFeeItemList = result.creditNoteItems.filter(item => item.itemType === 'SearchFee');
            this.airFareItemList = result.creditNoteItems.filter(item => item.itemType === 'AirFare');
            this.charteredItemList = result.creditNoteItems.filter(item => item.itemType === 'CharteredTransport');
            this.taxiFareItemList = result.creditNoteItems.filter(item => item.itemType === 'TaxiFare');
            this.accommodationItemList = result.creditNoteItems.filter(item => item.itemType === 'Accommodation');
            this.miscItemList = result.creditNoteItems.filter(item => item.itemType === 'Miscellaneous');

            this.active = true;
        });
    }

    print(): void {
        let printContents, popupWin;
        printContents = document.getElementById('creditNoteArea').innerHTML;
        popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
        popupWin.document.open();
        popupWin.document.write(`
            <html>
                <head>
                    <title>Print</title>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            margin-top: 30px;
                        }
                        .card-body {
                            width: 800px;
                            margin: auto;
                        }
                        .text-center {
                            text-align: center;
                        }
                        .text-right {
                            text-align: right;
                        }
                        table {
                            width: 100%;
                            border-collapse: collapse;
                        }
                            
                        .table-bordered td {
                            border: 1px solid grey;
                            padding: 10px;
                            text-align: left;
                        }
                        .mb-20 {
                            margin-bottom: 20px;
                        }
                        .mb-10 {
                            margin-bottom: 10px;
                        }
                        .mb-5 {
                            margin-bottom: 5px;
                        }
                        h1 {
                            font-size: 30px;
                            text-decoration: underline;
                            margin-bottom: 80px;
                        }
                        h2 {
                            margin-bottom: 0px;
                        }
                        h3, h5 {
                            margin: 0;
                        }
                        h1 {
                            margin: 0;
                        }
                        .company-telno-info {
                            margin-bottom: 50px;
                        }

                        /* Style the creditNote title */
                        .creditNote-title {
                            margin-bottom: 40px;
                        }

                        .client-info, .creditNote-info {
                            font-size: 12px;
                            margin-bottom: 30px;
                        }

                        .reference-info {
                            margin-bottom: 40px;
                            margin-left: 40px;
                        }

                        .total-row {
                            font-weight: bold;
                        }

                        .note {
                            font-size: 10px;
                            text-align: center;
                            margin-top: 50px;
                        }

                    </style>
                </head>
                <body onload="window.print();window.close()">${printContents}</body>
            </html>`
        );
        popupWin.document.close();
    }
    
    edit(): void {
        this._router.navigate(['/app/main/registration/caseCreditNotes/createOrEdit'], { queryParams: { id: this._activatedRoute.snapshot.queryParams['id'] } });  
    }


}
