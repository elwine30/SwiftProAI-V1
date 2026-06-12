import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetInvoiceReportForViewDto, InvoiceReportDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewInvoiceReportModal',
    templateUrl: './view-invoiceReport-modal.component.html'
})
export class ViewInvoiceReportModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetInvoiceReportForViewDto;
    minDate: Date = new Date(0); // or new Date('0001-01-01T00:00:00Z') for DateTime.MinValue equivalent

  invoiceFields: any[] = [
    { label: 'ReportDate', key: 'reportDate', type: 'date' },
    { label: 'InsuranceCompany', key: 'insuranceCompany' },
    { label: 'CaseReference', key: 'caseReference' },
    { label: 'InsurerRef', key: 'insurerRef' },
    { label: 'VehicleNo', key: 'vehicleNo' },
    { label: 'CaseType', key: 'caseType' },
    { label: 'Invoice ID', key: 'caseInvoiceId' },
    { label: 'Adjuster Name', key: 'adjusterName' },
    { label: 'Cheque Number', key: 'invChequeNo' },
    { label: 'Invoice Reference', key: 'invoiceRef' },
    { label: 'Date Paid', key: 'invDatePaid', type: 'date' },
    { label: 'Date Sent', key: 'invDateSent', type: 'date' },
    { label: 'Amount Paid', key: 'invAmountPaid', type: 'currency' },
    { label: 'Total Amount', key: 'invTotal', type: 'currency' },
    { label: 'Credit Note', key: 'invCreditNote', type: 'currency' },
    { label: 'Debit Note', key: 'invDebitNote', type: 'currency' },
  ];

  breakdownFields: any[] = [
    { label: 'Service', key: 'invService', type: 'currency' },
    { label: 'Mileage', key: 'invMileage', type: 'currency' },
    { label: 'Photo', key: 'invPhoto', type: 'currency' },
    { label: 'Toll', key: 'invToll', type: 'currency' },
    { label: 'Bridge', key: 'invBridge', type: 'currency' },
    { label: 'Police', key: 'invPolice', type: 'currency' },
    { label: 'Statutory', key: 'invStatutory', type: 'currency' },
    { label: 'Surveillance', key: 'invSurveillance', type: 'currency' },
    { label: 'Telco', key: 'invTelco', type: 'currency' },
    { label: 'Third Party', key: 'invThirdParty', type: 'currency' },
    { label: 'Air Fare', key: 'invAir', type: 'currency' },
    { label: 'Search Fee', key: 'invSearch', type: 'currency' },
    { label: 'Amount Excluding SST', key: 'invNetAmount', type: 'currency' },
    { label: 'SST Amount', key: 'invGST', type: 'currency' },
    { label: 'Total Amount', key: 'invTotal', type: 'currency' },
  ];

  isValidDate(date: Date): boolean {
    return date && date > this.minDate;
  }
    


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetInvoiceReportForViewDto();
        this.item.invoiceReport = new InvoiceReportDto();
    }

    show(item: GetInvoiceReportForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
