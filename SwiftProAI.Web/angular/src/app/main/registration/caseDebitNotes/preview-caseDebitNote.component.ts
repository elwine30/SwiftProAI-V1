import { AppConsts } from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CaseDebitNotesServiceProxy, GetCaseDebitNoteForPreviewDto, DebitNoteItemAmountDto, CreateOrEditCaseDebitNoteDto, DebitNoteItemsServiceProxy, DebitNoteItemDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
    templateUrl: './preview-caseDebitNote.component.html',
    animations: [appModuleAnimation()]
})
export class PreviewCaseDebitNoteComponent extends AppComponentBase implements OnInit {

    active = false;
    saving = false;

    caseDebitNote: CreateOrEditCaseDebitNoteDto = new CreateOrEditCaseDebitNoteDto();
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
    debitNoteDate = "";
    sstAmount : number;


    thirdPartyItemList : DebitNoteItemDto[];
    searchFeeItemList : DebitNoteItemDto[];
    airFareItemList : DebitNoteItemDto[];
    charteredItemList : DebitNoteItemDto[];
    taxiFareItemList : DebitNoteItemDto[];
    accommodationItemList : DebitNoteItemDto[];
    miscItemList : DebitNoteItemDto[];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseDebitNotesServiceProxy: CaseDebitNotesServiceProxy,
        private _debitNoteItemsServiceProxy: DebitNoteItemsServiceProxy,
        private _router: Router,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(caseDebitNoteId: number): void {
        this._caseDebitNotesServiceProxy.getCaseDebitNoteForPreview(caseDebitNoteId).subscribe(result => {
            this.caseDebitNote = result.caseDebitNote;
            if (this.caseDebitNote){
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

                this.mainRegistrationVehicleNo = result.mainRegistrationVehicleNo;
                this.fileRefNo = result.fileRefNo;
                this.debitNoteDate = this.caseDebitNote.debitDate.toISODate();

                this.sstAmount = parseFloat((this.caseDebitNote.amountWithSST - this.caseDebitNote.amountExcludeSST).toFixed(2));
            }
            this.thirdPartyItemList = result.debitNoteItems.filter(item => item.itemType === 'ThirdParty');
            this.searchFeeItemList = result.debitNoteItems.filter(item => item.itemType === 'SearchFee');
            this.airFareItemList = result.debitNoteItems.filter(item => item.itemType === 'AirFare');
            this.charteredItemList = result.debitNoteItems.filter(item => item.itemType === 'CharteredTransport');
            this.taxiFareItemList = result.debitNoteItems.filter(item => item.itemType === 'TaxiFare');
            this.accommodationItemList = result.debitNoteItems.filter(item => item.itemType === 'Accommodation');
            this.miscItemList = result.debitNoteItems.filter(item => item.itemType === 'Miscellaneous');

            this.active = true;
        });
    }

    print(): void {
        let printContents, popupWin;
        printContents = document.getElementById('debitNoteArea').innerHTML;
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

                        /* Style the debitNote title */
                        .debitNote-title {
                            margin-bottom: 40px;
                        }

                        .client-info, .debitNote-info {
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
        this._router.navigate(['/app/main/registration/caseDebitNotes/createOrEdit'], { queryParams: { id: this._activatedRoute.snapshot.queryParams['id'] } });  
    }


}
