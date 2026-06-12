import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    CaseExpensesServiceProxy,
    CreateExpenseInput,
    GetLookupByGroupDto,
    LookupsServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs';

@Component({
    selector: 'createAndViewExpensesModal',
    templateUrl: './create-and-view-caseExpenses.component.html',
})
export class CreateAndViewExpensesModal extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) public modal: ModalDirective;
    @ViewChild('caseExpensessForm') caseExpensessForm: NgForm;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    expense: CreateExpenseInput = new CreateExpenseInput();
    registerId: number = 0;
    active: boolean = false;
    saving: boolean = false;
    expenseTypeList: GetLookupByGroupDto[];
    caseExpensesList: any[] = [];

    private _caseExpenseServiceProxy: CaseExpensesServiceProxy;

    constructor(
        injector: Injector,
        private _lookupServiceProxy: LookupsServiceProxy,
    ) {
        super(injector);
        this._caseExpenseServiceProxy = injector.get(CaseExpensesServiceProxy);
    }

    show(registerId: number): void {
        this._caseExpenseServiceProxy.getCaseExpenseAdjusterViewDto(registerId).subscribe((result) => {
            this.caseExpensesList = result;
            this.registerId = registerId;
        });
        this.getExpenseType();
        if (this.modal) {
            this.modal.show();
        }
    }

    getExpenseType(): void {
        this._lookupServiceProxy.getByGroup('ExpensesType').subscribe((result) => {
            this.expenseTypeList = result;
            this.markFormAsPristine(this.caseExpensessForm);
        });
    }

    saveCaseExpense(): void {
        if (this.caseExpensessForm.valid) {
            this.saving = true;
            this.expense.mainRegistrationId = this.registerId;
            this.expense.subTypeId = 0;
            this._caseExpenseServiceProxy
                .createExpenses(this.expense)
                .pipe(
                    finalize(() => {
                        this.saving = false;
                    }),
                )
                .subscribe({
                    next: (result) => {
                        this.notify.info(this.l('SavedSuccessfully'));
                        this.caseExpensesList.push(result);
                        if (result.id) {
                            this.expense = new CreateExpenseInput();
                            this.caseExpensessForm.resetForm();
                        }
                        this.modalSave.emit(null);
                    },
                    error: (error) => {
                        this.notify.error(this.l('Failed'));
                    },
                });
        }
    }

    close(): void {
        if (this.caseExpensessForm.dirty) {
            confirm('Are you sure you want to leave? Your changes will be lost.') ? this.modal.hide() : '';
            this.caseExpensessForm.resetForm();
        } else {
            this.active = false;
            this.modal.hide();
        }
    }

    ngOnInit(): void {}

    ngAfterViewInit(): void {
        console.log(this.modal);
    }
}
