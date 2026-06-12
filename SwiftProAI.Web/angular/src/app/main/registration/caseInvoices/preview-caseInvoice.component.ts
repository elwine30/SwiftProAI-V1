import { AppConsts } from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CaseInvoicesServiceProxy, GetCaseInvoiceForPreviewDto, InvoiceItemAmountDto, CreateOrEditCaseInvoiceDto, InvoiceItemsServiceProxy, InvoiceItemDto, MainRegistrationServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { EnumRegistrationStatus } from "@app/shared/common/registration/enum";

@Component({
    templateUrl: './preview-caseInvoice.component.html',
    animations: [appModuleAnimation()]
})
export class PreviewCaseInvoiceComponent extends AppComponentBase implements OnInit {

    active = false;
    saving = false;

    caseInvoice: CreateOrEditCaseInvoiceDto = new CreateOrEditCaseInvoiceDto();
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
    invoiceDate = "";
    sstAmount : number;
    showCompleteInvoiceButton = false;

    thirdPartyItemList : InvoiceItemDto[];
    searchFeeItemList : InvoiceItemDto[];
    airFareItemList : InvoiceItemDto[];
    charteredItemList : InvoiceItemDto[];
    taxiFareItemList : InvoiceItemDto[];
    accommodationItemList : InvoiceItemDto[];
    miscItemList : InvoiceItemDto[];

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseInvoicesServiceProxy: CaseInvoicesServiceProxy,
        private _invoiceItemsServiceProxy: InvoiceItemsServiceProxy,
        private _mainRegistrationService: MainRegistrationServiceProxy,
        private _router: Router,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(caseInvoiceId: number): void {
        this._caseInvoicesServiceProxy.getCaseInvoiceForPreview(caseInvoiceId).subscribe(result => {
            this.caseInvoice = result.caseInvoice;
            if (this.caseInvoice){
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
                this.invoiceDate = this.caseInvoice.invoiceDate.toISODate();
                
                if (result.statusId == EnumRegistrationStatus.PendingInvoices) {
                    this.showCompleteInvoiceButton = true;
                }
                this.sstAmount = parseFloat((this.caseInvoice.amountWithSST - this.caseInvoice.amountExcludeSST).toFixed(2));
            }
                this.thirdPartyItemList = result.invoiceItems.filter(item => item.itemType === 'ThirdParty');
                this.searchFeeItemList = result.invoiceItems.filter(item => item.itemType === 'SearchFee');
                this.airFareItemList = result.invoiceItems.filter(item => item.itemType === 'AirFare');
                this.charteredItemList = result.invoiceItems.filter(item => item.itemType === 'CharteredTransport');
                this.taxiFareItemList = result.invoiceItems.filter(item => item.itemType === 'TaxiFare');
                this.accommodationItemList = result.invoiceItems.filter(item => item.itemType === 'Accommodation');
                this.miscItemList = result.invoiceItems.filter(item => item.itemType === 'Miscellaneous');

            this.active = true;
        });
    }

    print(): void {
        let printContents, popupWin;
        printContents = document.getElementById('invoiceArea').innerHTML;
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

                        /* Style the invoice title */
                        .invoice-title {
                            margin-bottom: 40px;
                        }

                        .client-info, .invoice-info {
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
        this._router.navigate(['/app/main/registration/caseInvoices/createOrEdit'], { queryParams: { id: this._activatedRoute.snapshot.queryParams['id'] } });  
    }

    reloadPage(): void {
        location.reload();    
    }

    completeInvoice(): void {
        this.message.confirm(
            this.l('CompleteInvoiceReminder'),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._mainRegistrationService.updateStatus(this._activatedRoute.snapshot.queryParams['id']).subscribe(result => {
                        this.notify.success(this.l('InvoiceCompleted'));
                        this.show(this._activatedRoute.snapshot.queryParams['id']);
                    });
                    this.showCompleteInvoiceButton = false;
                }
            }
        );

    }


}
