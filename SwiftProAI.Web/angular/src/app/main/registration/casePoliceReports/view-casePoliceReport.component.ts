import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    CasePoliceReportsServiceProxy,
    GetCasePoliceReportForViewDto,
    CasePoliceReportDto,
    LookupsServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { LazyLoadEvent } from 'primeng/api';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import { ViewCasePoliceReportModalComponent } from './view-casePoliceReport-modal.component';
@Component({
    templateUrl: './view-casePoliceReport.component.html',
    animations: [appModuleAnimation()],
})
export class ViewCasePoliceReportComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;

    registerId = '';
    @ViewChild('viewCasePoliceReportModal', { static: true })
    viewCasePoliceReportModal: ViewCasePoliceReportModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _casePoliceReportsServiceProxy: CasePoliceReportsServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService,
        public navigationService: NavigationService,
        private _lookupService: LookupsServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'].toString();
        this.navigationService.registerId = this.registerId;
        // this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    getCasePoliceReports(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._casePoliceReportsServiceProxy
            .getAllForView(
                this.registerId,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                50,
            )
            .subscribe((result) => {
                console.log(result);
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFile?id=' + id;
    }
}
