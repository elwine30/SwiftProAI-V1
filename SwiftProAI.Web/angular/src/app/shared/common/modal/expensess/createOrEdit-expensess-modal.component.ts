import { Component, Injector, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { CaseExpensesServiceProxy, CreateExpenseInput, GetLookupByGroupDto, LookupsServiceProxy, UpdateCaseExpensesDTO } from "@shared/service-proxies/service-proxies";
import { error } from "console";
import { subscribe } from "diagnostics_channel";
import { BsModalRef } from "ngx-bootstrap/modal";
import { finalize } from "rxjs";

@Component({
    selector: 'add-expensess-modal',
    templateUrl: './createOrEdit-expensess-modal.component.html',
})
export class CreateAndViewExpensesModalComponent extends AppComponentBase implements OnInit {

    saving: boolean = false;
    showTable: boolean = true;

    expense: UpdateCaseExpensesDTO = new UpdateCaseExpensesDTO();
    caseExpensesList: any[] = [];
    expenseTypeList: GetLookupByGroupDto[];

    expenseRecord: any;
    expensessForm: FormGroup;
    expensessTypeList: any;

    constructor(
        injector: Injector,
        public bsModalRef: BsModalRef,
        private fb: FormBuilder,
        public _caseExpensesServiceProxy: CaseExpensesServiceProxy,
        private _lookupServiceProxy: LookupsServiceProxy,
    ) {
        super(injector);
    }


    ngOnInit(): void {

        //Register Form Group
        this.expensessForm = this.fb.group({
            remark: ['', [Validators.required]],
            amount: ['', [Validators.required]],
            typeId: ['', [Validators.required]]
        });

        this._lookupServiceProxy.getByGroup('ExpensesType')
            .subscribe((result) => {
                this.expenseTypeList = result;
            });
        this._caseExpensesServiceProxy.getCaseExpenseForView(this.expenseRecord.id)
            .subscribe((result) => {
                this.expensessForm.patchValue({
                    remark: result.caseExpense.remark,
                    amount: result.caseExpense.amount,
                    typeId: result.caseExpense.typeId
                });
            });
    }

    close() {
        this.bsModalRef.hide();
    }

    get remark() {
        return this.expensessForm.get('remark');
    }

    saveCaseExpense() {
        if(this.expensessForm.valid) {

            this.expense = this.expensessForm.value
            this.expense.id = this.expenseRecord.id;
            this.expense.registerId = this.expenseRecord.registerId
            this.expense.subTypeId = 0;

            this._caseExpensesServiceProxy.updateCaseExpenses(this.expense)
                .pipe(finalize(() => {
                    this.saving = false;
                    this.bsModalRef.hide();
                }))
                .subscribe({
                    next: (result) => {
                        this.notify.info(this.l('SavedSuccesfully'));
                    },
                    error: (error) => {
                        this.notify.error(this.l('Failed'));
                    }
                });
        }
    }
}