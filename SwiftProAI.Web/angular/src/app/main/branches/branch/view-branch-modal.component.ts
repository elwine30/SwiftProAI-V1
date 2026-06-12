import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetBranchForViewDto, BranchDto, BranchServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBranchModal',
    templateUrl: './view-branch-modal.component.html',
})
export class ViewBranchModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBranchForViewDto;

    constructor(
        injector: Injector,
        private _branchServiceProxy: BranchServiceProxy,
    ) {
        super(injector);
        this.item = new GetBranchForViewDto();
        this.item.branch = new BranchDto();
    }

    show(branchId: number): void {
        this._branchServiceProxy.getBranchForView(branchId).subscribe((result) => {
            this.item = result;
            this.active = true;
        });
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
