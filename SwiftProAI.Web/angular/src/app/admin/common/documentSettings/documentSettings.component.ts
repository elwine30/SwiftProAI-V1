import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { DocumentSettingsServiceProxy, DocumentSettingDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    templateUrl: './documentSettings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class DocumentSettingsComponent extends AppComponentBase {
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    

    advancedFiltersAreShown = false;
    filterText = '';
    businessRegistrationNoFilter = '';
    companyLegalNameFilter = '';
    addressFilter = '';
    taxNoFilter = '';
    telNoFilter = '';
    invoiceRefNoPrefixFilter = '';
    maxinvoiceRefNoLengthFilter : number;
    maxinvoiceRefNoLengthFilterEmpty : number;
    mininvoiceRefNoLengthFilter : number;
    mininvoiceRefNoLengthFilterEmpty : number;
    debitRefNoPrefixFilter = '';
    maxdebitRefNoLengthFilter : number;
    maxdebitRefNoLengthFilterEmpty : number;
    mindebitRefNoLengthFilter : number;
    mindebitRefNoLengthFilterEmpty : number;
    creditRefNoPrefixFilter = '';
    maxcreditRefNoLengthFilter : number;
    maxcreditRefNoLengthFilterEmpty : number;
    mincreditRefNoLengthFilter : number;
    mincreditRefNoLengthFilterEmpty : number;
    caseRefNoPrefixFilter = '';
    maxcaseRefNoLengthFilter : number;
    maxcaseRefNoLengthFilterEmpty : number;
    mincaseRefNoLengthFilter : number;
    mincaseRefNoLengthFilterEmpty : number;



    constructor(
        injector: Injector,
        private _documentSettingsServiceProxy: DocumentSettingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
			private _router: Router,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
        
    }

    getDocumentSettings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._documentSettingsServiceProxy.getAll(
            this.filterText,
            this.businessRegistrationNoFilter,
            this.companyLegalNameFilter,
            this.addressFilter,
            this.taxNoFilter,
            this.telNoFilter,
            this.invoiceRefNoPrefixFilter,
            this.maxinvoiceRefNoLengthFilter == null ? this.maxinvoiceRefNoLengthFilterEmpty: this.maxinvoiceRefNoLengthFilter,
            this.mininvoiceRefNoLengthFilter == null ? this.mininvoiceRefNoLengthFilterEmpty: this.mininvoiceRefNoLengthFilter,
            this.debitRefNoPrefixFilter,
            this.maxdebitRefNoLengthFilter == null ? this.maxdebitRefNoLengthFilterEmpty: this.maxdebitRefNoLengthFilter,
            this.mindebitRefNoLengthFilter == null ? this.mindebitRefNoLengthFilterEmpty: this.mindebitRefNoLengthFilter,
            this.creditRefNoPrefixFilter,
            this.maxcreditRefNoLengthFilter == null ? this.maxcreditRefNoLengthFilterEmpty: this.maxcreditRefNoLengthFilter,
            this.mincreditRefNoLengthFilter == null ? this.mincreditRefNoLengthFilterEmpty: this.mincreditRefNoLengthFilter,
            this.caseRefNoPrefixFilter,
            this.maxcaseRefNoLengthFilter == null ? this.maxcaseRefNoLengthFilterEmpty: this.maxcaseRefNoLengthFilter,
            this.mincaseRefNoLengthFilter == null ? this.mincaseRefNoLengthFilterEmpty: this.mincaseRefNoLengthFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createDocumentSetting(): void {
        this._router.navigate(['/app/admin/common/documentSettings/createOrEdit']);        
    }


    deleteDocumentSetting(documentSetting: DocumentSettingDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._documentSettingsServiceProxy.delete(documentSetting.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
    
    
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
        this.businessRegistrationNoFilter = '';
        this.companyLegalNameFilter = '';
        this.addressFilter = '';
        this.taxNoFilter = '';
        this.telNoFilter = '';
        this.invoiceRefNoPrefixFilter = '';
        this.maxinvoiceRefNoLengthFilter = this.maxinvoiceRefNoLengthFilterEmpty;
        this.mininvoiceRefNoLengthFilter = this.maxinvoiceRefNoLengthFilterEmpty;
        this.debitRefNoPrefixFilter = '';
        this.maxdebitRefNoLengthFilter = this.maxdebitRefNoLengthFilterEmpty;
        this.mindebitRefNoLengthFilter = this.maxdebitRefNoLengthFilterEmpty;
        this.creditRefNoPrefixFilter = '';
        this.maxcreditRefNoLengthFilter = this.maxcreditRefNoLengthFilterEmpty;
        this.mincreditRefNoLengthFilter = this.maxcreditRefNoLengthFilterEmpty;
        this.caseRefNoPrefixFilter = '';
        this.maxcaseRefNoLengthFilter = this.maxcaseRefNoLengthFilterEmpty;
        this.mincaseRefNoLengthFilter = this.maxcaseRefNoLengthFilterEmpty;
        this.getDocumentSettings();
    }
}
