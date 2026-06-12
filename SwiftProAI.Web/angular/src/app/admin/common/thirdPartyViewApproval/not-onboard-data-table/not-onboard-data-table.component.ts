import { Component, Injector, Input, OnInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { DataService } from "@app/shared/common/data-service/data-service";
import { ThirdPartyCaseViewApprovalStatus } from "@app/shared/common/registration/enum";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AppComponentBase } from "@shared/common/app-component-base";
import { CreateOrEditViewThirdPartyCaseRequestDto, ViewThirdPartyCaseRequestsServiceProxy } from "@shared/service-proxies/service-proxies";
import { DateTime } from "luxon";
import { LazyLoadEvent } from "primeng/api";
import { Paginator } from "primeng/paginator";
import { Table } from "primeng/table";
import { finalize } from "rxjs";

@Component({
    selector: 'notonboardDataTable',
    templateUrl: './not-onboard-data-table.component.html',
    animations: [appModuleAnimation()]
})
export class NotOnboardDataTableComponent extends AppComponentBase {

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    @Input() selectedStatus: string;
    @Input() creationDateString: any;

    dataCount: number = 0;

    updateData = new CreateOrEditViewThirdPartyCaseRequestDto();

    cols: any[] = [
        { field: 'adjusterCompanyName', header: 'Adjuster Company Name' },
        { field: 'thirdPartyCompanyName', header: 'ThirdParty Company Name' },
        { field: 'companyType', header: 'Company Type' },
        { field: 'businessRegistrationNo', header: 'Busness Registration No.' },
        { field: 'requestedBy', header: 'Requested By' },
        { field: 'requestedDate', header: 'Requested Date' }
    ]; // Column configuration

    constructor(
        injector: Injector,
        private _viewThirdPartyCaseApprovalService: ViewThirdPartyCaseRequestsServiceProxy,
        private router: Router,
        private dataService: DataService,
    ) {
        super(injector);
    }

    getTableData(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);

            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._viewThirdPartyCaseApprovalService.getAllNotOnboarded(
            '',
            this.selectedStatus,
            this.creationDateString != null ? DateTime.fromJSDate(new Date(this.creationDateString)) : this.creationDateString,
            'CreationTime asc',
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        )
            .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();

                this.dataCount = result.totalCount;
            });
    }

    toOnboardPage(record: any) {
        
        this.dataService.setNotOnboardData(record);
        this.router.navigate(['/app/admin/registration/onboardingOU']);
    }
}