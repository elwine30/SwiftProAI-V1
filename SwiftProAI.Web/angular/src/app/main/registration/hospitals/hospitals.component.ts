import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { HospitalsServiceProxy, HospitalDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditHospitalModalComponent } from './create-or-edit-hospital-modal.component';

import { ViewHospitalModalComponent } from './view-hospital-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    templateUrl: './hospitals.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class HospitalsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditHospitalModal', { static: true }) createOrEditHospitalModal: CreateOrEditHospitalModalComponent;
    @ViewChild('viewHospitalModal', { static: true }) viewHospitalModal: ViewHospitalModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    addressFilter = '';
        locationNameFilter = '';
        locationName2Filter = '';







    constructor(
        injector: Injector,
        private _hospitalsServiceProxy: HospitalsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
        
    }

    getHospitals(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._hospitalsServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.addressFilter,
            this.locationNameFilter,
            this.locationName2Filter,
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

    createHospital(): void {
        this.createOrEditHospitalModal.show();        
    }


    deleteHospital(hospital: HospitalDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._hospitalsServiceProxy.delete(hospital.id)
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
    this.addressFilter = '';
		this.locationNameFilter = '';
							this.locationName2Filter = '';
					
        this.getHospitals();
    }
}
