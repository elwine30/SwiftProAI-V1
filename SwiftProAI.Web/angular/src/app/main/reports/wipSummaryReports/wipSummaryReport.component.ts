import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CommonDropdownServiceProxy, WIPSummaryReportDto, WIPSummaryReportsServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { result } from 'lodash-es';

@Component({
    selector: 'app-wipSummaryReport',
    templateUrl: './wipSummaryReport.component.html',
})
export class WipSummaryReportComponent extends AppComponentBase implements OnInit {
    caseTypeList: any[] = [];
    userList: any[] = [];
    insuranceCompanyList: any[] = [];

    selectedCaseType: number | undefined;
    selectedUser: number | undefined;
    selectedCompany: number | undefined;

    columnHeaders: string[] = [];
    rowHeaders: string[] = [];
    pivotData: any = {};

    private _wipServiceProxy: WIPSummaryReportsServiceProxy;
    private _CommonDropdownService: CommonDropdownServiceProxy;
    private _fileDownloadService: FileDownloadService;

    constructor(injector: Injector) {
        super(injector);
        this._CommonDropdownService = injector.get(CommonDropdownServiceProxy);
        this._wipServiceProxy = injector.get(WIPSummaryReportsServiceProxy);
        this._fileDownloadService = injector.get(FileDownloadService);
    }

    ngOnInit() {
        this._CommonDropdownService.getAllCaseTypeForTableDropdown().subscribe(result=>{
            this.caseTypeList = result;
        })
        this._CommonDropdownService.getAllAdjusterForTableDropdown(undefined).subscribe(result=>{
            this.userList = result;
        })
        this._CommonDropdownService.getAllCompanyForTableDropdown().subscribe(result=>{
            this.insuranceCompanyList = result;
        })
    }

    download() {
        if (!this.selectedCaseType && !this.selectedUser && !this.selectedCompany) {
            return;
        }
        this._wipServiceProxy
            .getWIPSummaryReportsToExcel(this.selectedCaseType, this.selectedUser, this.selectedCompany, undefined)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    search() {
        if (!this.selectedCaseType && !this.selectedUser && !this.selectedCompany) {
            return;
        }

        this._wipServiceProxy
            .getAllWipSummaryData(this.selectedCaseType, this.selectedUser, this.selectedCompany, undefined)
            .subscribe((result) => {
                this.processPivotData(result);
            });
    }

    processPivotData(data: any) {
        this.columnHeaders = data.columnHeaders;
        this.rowHeaders = data.rowHeaders;
        this.pivotData = data.data;
    }

    resetFilters(): void {
        this.selectedCaseType = undefined;
        this.selectedCompany = undefined;
        this.selectedUser = undefined;
    }

    getCellData(column: string, row: string): number {
        return this.pivotData[column] && this.pivotData[column][row] ? this.pivotData[column][row] : 0;
    }
}
