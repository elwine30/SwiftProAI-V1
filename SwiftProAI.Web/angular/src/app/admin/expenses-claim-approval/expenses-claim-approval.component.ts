import { DatePipe } from "@angular/common";
import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { Router } from "@angular/router";
import { CreateAndViewExpensesModalComponent } from "@app/shared/common/modal/expensess/createOrEdit-expensess-modal.component";
import { ExpensessClaimType, ExpensessStatusGroupType } from "@app/shared/common/registration/enum";
import { DateTimeService } from "@app/shared/common/timing/date-time.service";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ExpensesClaimsApprovalDto, ExpensesClaimsApprovalServiceProxy, LookupsServiceProxy, StaffLookupTableDto, StaffsServiceProxy } from "@shared/service-proxies/service-proxies";
import { DateTime } from "luxon";
import { BsModalRef, BsModalService, ModalOptions } from "ngx-bootstrap/modal";
import { LazyLoadEvent } from "primeng/api";
import { Paginator } from "primeng/paginator";
import { Table } from "primeng/table";
import { finalize } from "rxjs";

@Component({
    templateUrl: './expenses-claim-approval.component.html',
    styleUrls: ['./expenses-claim-approval.component.less'],
    encapsulation:  ViewEncapsulation.None,
    animations: [appModuleAnimation()],
    providers: [DatePipe]
})
export class ExpensesClaimApprovalComponent extends AppComponentBase implements OnInit {
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    saving = false;

    test: BsModalRef;

    filters: {
        dateFromFilter: string,
        dateToFilter: string,
        selectedStatus: string,
        selectedType: string,
        selectedGroup: string,
        selectedAdjuster: string,
    } = <any>{};

    cols = [
        { field: 'caseNo', header: 'Case No' },
        { field: 'vehicleNo', header: 'Vehicle Number' },
        { field: 'adjuster', header: 'Adjuster' },
        { field: 'amount', header: 'Amount' },
        { field: 'remark', header: 'Remark' },
        { field: 'status', header: 'Status' },
        { field: 'type', header: 'Type' }
    ];

    selectedStatus: any;

    statusList: any;
    typeList: any;
    groupList: any;
    adjusterList: any;
    actionList = [
        { id: '0', displayName: 'Select One' },
        { id: '1', displayName: 'Approve' },
        { id: '2', displayName: 'Reject' }
    ];

    disableAdjuster: boolean = true;

    expensesClaimsList: any;
    hideSaveButton: boolean = false;

    dateRangeFilter: DateTime[] = [this._dateTimeService.getStartOfDay(), this._dateTimeService.getEndOfDay()];

    constructor(
        injector: Injector,
        private _lookupService: LookupsServiceProxy,
        private _staffLookupService: StaffsServiceProxy,
        private _expensesClaimsApprovalService: ExpensesClaimsApprovalServiceProxy,
        private _dateTimeService: DateTimeService,
        private router: Router,
        private modalService: BsModalService,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._lookupService.getByGroup("ExpensesStatus")
        .subscribe(result => {
            this.statusList = result;
        });

        this._lookupService.getByGroup("ExpClaimApproval")
        .subscribe(result => {
            this.typeList = result;
        });

        this._staffLookupService.getAllGroupForTableDropdown()
        .subscribe(result => {
            this.groupList = result;
        });

        
        let today = this._dateTimeService.getStartOfDay();
        let firstDayOfMonth = today.startOf('month');
        this.dateRangeFilter = [firstDayOfMonth, today];

        this.selectedStatus = ExpensessStatusGroupType.PendingForApproval;
        this.filters.selectedType = ExpensessClaimType.Expenses;
        this.filters.selectedGroup = '0';
        this.filters.selectedAdjuster = '0';
    }

    getTableData(event?: LazyLoadEvent) {
        this.filters.selectedStatus = this.selectedStatus;

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);

            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        if(this.filters.selectedType == ExpensessClaimType.Expenses) {
            this._expensesClaimsApprovalService
                .getAllExpensesApproval(
                    this.dateRangeFilter[0],
                    this.dateRangeFilter[1].endOf('day'),
                    this.filters.selectedStatus,
                    this.filters.selectedType,
                    Number(this.filters.selectedGroup),
                    Number(this.filters.selectedAdjuster),  
                    this.primengTableHelper.getSorting(this.dataTable),
                    this.primengTableHelper.getSkipCount(this.paginator, event),
                    this.primengTableHelper.getMaxResultCount(this.paginator, event)
                )
                .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
                .subscribe((result) => {
                    this.primengTableHelper.totalRecordsCount = result.totalCount;
                    this.primengTableHelper.records = result.items;
                    this.expensesClaimsList = result.items;
                    this.primengTableHelper.hideLoadingIndicator();
                });
        } else if(this.filters.selectedType == ExpensessClaimType.Claims) {
            this._expensesClaimsApprovalService
                .getAllClaimsForApproval(
                    this.dateRangeFilter[0],
                    this.dateRangeFilter[1].endOf('day'),
                    this.filters.selectedStatus,
                    this.filters.selectedType,
                    Number(this.filters.selectedGroup),
                    Number(this.filters.selectedAdjuster),  
                    this.primengTableHelper.getSorting(this.dataTable),
                    this.primengTableHelper.getSkipCount(this.paginator, event),
                    this.primengTableHelper.getMaxResultCount(this.paginator, event)
                ).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
                .subscribe((result) => {
                    this.primengTableHelper.totalRecordsCount = result.totalCount;
                    this.primengTableHelper.records = result.items;
                    this.expensesClaimsList = result.items;
                    this.primengTableHelper.hideLoadingIndicator();
                });
        }

    }
    
    actionOnChange(event: any, id: number): void {
        const selectedValue = event.target.value;
        if(selectedValue == 1) {
            //Approval
            const item = this.expensesClaimsList.find((option: { id: number }) => option.id == id);
            if(item != null) {
                if(item.rejected == true) {
                    item.rejected = false;
                }
                item.approved = true;
            }
        } else if(selectedValue == 2) {
            //Rejected
            const item = this.expensesClaimsList.find((option: { id: number }) => option.id == id);
            if(item != null) {
                if(item.approved == true) {
                    item.approved == false;
                }
                item.rejected = true;

            }
        } else {
            const item = this.expensesClaimsList.find((option: { id: number }) => option.id == id);
            item.approved = false;
            item.rejected = false;
        }
        
    }

    save(): void {
        
        const itemDto: ExpensesClaimsApprovalDto[] = this.expensesClaimsList
        .filter(item => item.approved || item.rejected)
        .map(item => ({
            id: item.id ? item.id : item.id,
            approved: item.approved,
            rejected: item.rejected
        }));

        const type = this.typeList.find((option: { code: string }) => option.code == this.filters.selectedType);

        if(type != null && type.code == ExpensessClaimType.Expenses) {
            this._expensesClaimsApprovalService.updateExpenses(itemDto)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('Update Successfully'));

                this.getTableData();
            });
        } else if (type != null && type.code == ExpensessClaimType.Claims) {
            this._expensesClaimsApprovalService.updateClaims(itemDto)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('Update Successfully'));

                this.getTableData();
            });
        }
    }

    onGroupChange(value: string) {
        if(value != "") {
            this.disableAdjuster = false;
            this._staffLookupService.getAllStaffByGroupIdForTableDropdown(Number(value))
            .subscribe((result) => {
                this.adjusterList = result;
            });
            
        } else {
            this.disableAdjuster = true;
        }
    }

    goToEdit(record: any): void {
        if(this.filters.selectedType == ExpensessClaimType.Claims) {
            this.router.navigate(['/app/main/registration/caseClaims/createOrEdit'], { queryParams: { id: record.id } });
        }

        if(this.filters.selectedType == ExpensessClaimType.Expenses) {
            const modalConfig: ModalOptions = {
                backdrop: 'static',
                keyboard: false,
                animated: true,
                ignoreBackdropClick: false,
                class: 'modal-lg',
                initialState: {
                    expenseRecord: record,
                    showTable: false,
                }
            };

            this.test = this.modalService.show(CreateAndViewExpensesModalComponent,  modalConfig);
        }
    }

    isShouldHide(statusCode: string): boolean {
        
        return statusCode == ExpensessStatusGroupType.Rejected || 
            statusCode == ExpensessStatusGroupType.PaymentDone ||
            statusCode == ExpensessStatusGroupType.SubmitWithoutPayment ||
            statusCode == ExpensessStatusGroupType.Submitted
    }

    colspanLength(statusCode: string) {
        if(statusCode == ExpensessStatusGroupType.Rejected || statusCode == ExpensessStatusGroupType.PaymentDone || statusCode == ExpensessStatusGroupType.SubmitWithoutPayment) {
            return '6'
        } else {
            return '8'
        }
    }

    // isStatusDisabled(statusCode: string): boolean {
    //     return statusCode === ExpensessStatusGroupType.Submitted ||
    //            statusCode === ExpensessStatusGroupType.SubmitWithoutPayment;
    // }
}

function getFirstDateOfMonth(): Date {
    const now = new Date();
    return new Date(now.getFullYear(), now.getMonth(), 1);
}