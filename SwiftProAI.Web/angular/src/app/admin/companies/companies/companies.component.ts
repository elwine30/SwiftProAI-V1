import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { CompanyServiceProxy, CompanyDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditCompanyModalComponent } from './create-or-edit-company-modal.component';

import { ViewCompanyModalComponent } from './view-company-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    templateUrl: './companies.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CompaniesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditCompanyModal', { static: true }) createOrEditCompanyModal: CreateOrEditCompanyModalComponent;
    @ViewChild('viewCompanyModal', { static: true }) viewCompanyModal: ViewCompanyModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    shortNameFilter = '';
    maxClaimRateFilter : number;
    maxClaimRateFilterEmpty : number;
    minClaimRateFilter : number;
    minClaimRateFilterEmpty : number;
    addressFilter = '';
    gstRegNoFilter = '';
    isActiveFilter = -1;
    maxPhotographChargeFilter : number;
    maxPhotographChargeFilterEmpty : number;
    minPhotographChargeFilter : number;
    minPhotographChargeFilterEmpty : number;
    caseTypeDescriptionFilter = '';


    constructor(
        injector: Injector,
        private _companiesServiceProxy: CompanyServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        
    }

    getCompanies(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._companiesServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.shortNameFilter,
            this.maxClaimRateFilter == null ? this.maxClaimRateFilterEmpty: this.maxClaimRateFilter,
            this.minClaimRateFilter == null ? this.minClaimRateFilterEmpty: this.minClaimRateFilter,
            this.addressFilter,
            this.gstRegNoFilter,
            this.isActiveFilter,
            this.maxPhotographChargeFilter == null ? this.maxPhotographChargeFilterEmpty: this.maxPhotographChargeFilter,
            this.minPhotographChargeFilter == null ? this.minPhotographChargeFilterEmpty: this.minPhotographChargeFilter,
            this.caseTypeDescriptionFilter,
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

    createCompany(): void {
        this.createOrEditCompanyModal.show();        
    }


    deleteCompany(company: CompanyDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._companiesServiceProxy.delete(company.id)
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
        this.nameFilter = '';
        this.shortNameFilter = '';
        this.maxClaimRateFilter = this.maxClaimRateFilterEmpty;
		this.minClaimRateFilter = this.maxClaimRateFilterEmpty;
        this.addressFilter = '';
        this.gstRegNoFilter = '';
        this.isActiveFilter = -1;
        this.maxPhotographChargeFilter = this.maxPhotographChargeFilterEmpty;
		this.minPhotographChargeFilter = this.maxPhotographChargeFilterEmpty;
		this.caseTypeDescriptionFilter = '';
					
        this.getCompanies();
    }
}
