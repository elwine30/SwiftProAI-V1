import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { DateTimeService } from "@app/shared/common/timing/date-time.service";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AppComponentBase } from "@shared/common/app-component-base";
import { CommonDropdownDto, CommonDropdownServiceProxy, ViewThirdPartyCaseRequestsServiceProxy } from "@shared/service-proxies/service-proxies";
import { DateTime } from "luxon";
import { TabDirective } from "ngx-bootstrap/tabs";
import { LazyLoadEvent } from "primeng/api";
import { Paginator } from "primeng/paginator";
import { Table } from "primeng/table";
import { finalize } from "rxjs";
import { NotOnboardDataTableComponent } from "./not-onboard-data-table/not-onboard-data-table.component";
import { OnBoardDataTableComponent } from "./onboard-data-table/onboard-data-table.component";

@Component({
    templateUrl: './thirdPartyViewApproval.component.html',
    styleUrls: ['./thirdPartyViewApproval.component.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ThirdPartyViewComponent extends AppComponentBase implements AfterViewInit {

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('notOnboardDataTable', { static: true }) notOnboardDataTable: NotOnboardDataTableComponent;
    @ViewChild('onBoardDataTable', { static: true }) onBoardDataTable: OnBoardDataTableComponent;

    currentTab: string = 'OnBoard';

    cols: any[] = [
        { field: 'adjusterCompanyName', header: 'Adjuster Company Name' },
        { field: 'thirdPartyCompanyName', header: 'ThirdParty Company Name' },
        { field: 'companyType', header: 'Company Type' },
        { field: 'businessRegistrationNo', header: 'Busness Registration No.' },
        { field: 'requestedBy', header: 'Requested By' },
        { field: 'requestedDate', header: 'Requested Date' }
    ]; // Column configuration

    statusList: CommonDropdownDto[];
    selectedStatus = 'Pending Approval';

    creationDate: DateTime;
    creationDateString: any;

    constructor(
        injector: Injector,
        private _commonDropdownService: CommonDropdownServiceProxy,
    ) {
        super(injector);
    }

    ngAfterViewInit(): void {
        this._commonDropdownService.getAllThirdPartyCaseViewRequestStatus()
            .subscribe(result => {
                this.statusList = result;
            });

        
        this.notOnboardDataTable.selectedStatus = this.selectedStatus;
        this.onBoardDataTable.selectedStatus = this.selectedStatus;

        this.notOnboardDataTable.getTableData();
        this.onBoardDataTable.getTableData();
    }

    getDataTable() {
        //get data for not onboard datatable
        this.notOnboardDataTable.selectedStatus = this.selectedStatus;
        this.notOnboardDataTable.creationDateString = this.creationDateString;
        this.notOnboardDataTable.getTableData();

        //get data for onboard datatable
        this.onBoardDataTable.selectedStatus = this.selectedStatus;
        this.onBoardDataTable.creationDateString = this.creationDateString;
        this.onBoardDataTable.getTableData();
    }

    getDataCountNotOnboarded() {
        return this.notOnboardDataTable.dataCount;
    }

    getDataCountOnboard() {
        return this.onBoardDataTable.dataCount;
    }

    onChangeStatus(event: number) {
        const selectedStatusName = this.statusList.find(x => x.id == event);
        this.selectedStatus = selectedStatusName.displayName;
    }

    onTabChange(tab: string) {
        this.currentTab = tab.toString();
    }
}