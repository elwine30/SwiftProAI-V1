import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ScopeAssignmentsServiceProxy, CreateOrEditScopeAssignmentDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditScopeAssignmentModal',
    templateUrl: './create-or-edit-scopeAssignment-modal.component.html',
})
export class CreateOrEditScopeAssignmentModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    scopeAssignment: CreateOrEditScopeAssignmentDto = new CreateOrEditScopeAssignmentDto();

    constructor(
        injector: Injector,
        private _scopeAssignmentsServiceProxy: ScopeAssignmentsServiceProxy,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
    }

    show(scopeAssignmentId?: number): void {
        if (!scopeAssignmentId) {
            this.scopeAssignment = new CreateOrEditScopeAssignmentDto();
            this.scopeAssignment.id = scopeAssignmentId;

            this.active = true;
            this.modal.show();
        } else {
            this._scopeAssignmentsServiceProxy.getScopeAssignmentForEdit(scopeAssignmentId).subscribe((result) => {
                this.scopeAssignment = result.scopeAssignment;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._scopeAssignmentsServiceProxy
            .createOrEdit(this.scopeAssignment)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
