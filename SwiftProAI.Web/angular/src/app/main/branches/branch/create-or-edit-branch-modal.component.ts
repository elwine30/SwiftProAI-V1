import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { BranchServiceProxy, CreateOrEditBranchDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditBranchModal',
    templateUrl: './create-or-edit-branch-modal.component.html',
})
export class CreateOrEditBranchModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    branch: CreateOrEditBranchDto = new CreateOrEditBranchDto();

    constructor(
        injector: Injector,
        private _branchServiceProxy: BranchServiceProxy,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
    }

    show(branchId?: number): void {
        if (!branchId) {
            this.branch = new CreateOrEditBranchDto();
            this.branch.id = branchId;

            this.active = true;
            this.modal.show();
        } else {
            this._branchServiceProxy.getBranchForEdit(branchId).subscribe((result) => {
                this.branch = result.branch;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._branchServiceProxy
            .createOrEdit(this.branch)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                console.log('Emitting modalSave event');
                this.modalSave.emit();
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
