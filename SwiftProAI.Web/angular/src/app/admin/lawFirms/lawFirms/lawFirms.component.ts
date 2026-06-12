import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { LawFirmsServiceProxy, LawFirmDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditLawFirmModalComponent } from './create-or-edit-lawFirm-modal.component';

import { ViewLawFirmModalComponent } from './view-lawFirm-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

import { HttpClient } from '@angular/common/http';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs';
            

@Component({
    templateUrl: './lawFirms.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class LawFirmsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditLawFirmModal', { static: true }) createOrEditLawFirmModal: CreateOrEditLawFirmModalComponent;
    @ViewChild('viewLawFirmModal', { static: true }) viewLawFirmModal: ViewLawFirmModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
                    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
            

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    shortNameFilter = '';
    addressFilter = '';
    isActiveFilter = -1;







                    uploadUrl: string;
            

    constructor(
        injector: Injector,
        private _lawFirmsServiceProxy: LawFirmsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService,
             private _httpClient: HttpClient
    ) {
        super(injector);
        
                    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/LawFirms/ImportFromExcel';
            
    }

    getLawFirms(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._lawFirmsServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.shortNameFilter,
            this.addressFilter,
            this.isActiveFilter,
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

    createLawFirm(): void {
        this.createOrEditLawFirmModal.show();        
    }


    deleteLawFirm(lawFirm: LawFirmDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._lawFirmsServiceProxy.delete(lawFirm.id)
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
    this.addressFilter = '';

        this.getLawFirms();
    }
}
