import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrUpdateRoleInput, RoleEditDto, RoleServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { PermissionTreeComponent } from '../shared/permission-tree.component';
import { finalize } from 'rxjs/operators';
import { NgForm } from '@angular/forms';
import Swal from 'sweetalert2';

@Component({
    selector: 'createOrEditRoleModal',
    templateUrl: './create-or-edit-role-modal.component.html',
})
export class CreateOrEditRoleModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('permissionTree') permissionTree: PermissionTreeComponent;
    @ViewChild('roleForm') roleForm: NgForm;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    role: RoleEditDto = new RoleEditDto();
    constructor(injector: Injector, private _roleService: RoleServiceProxy) {
        super(injector);
    }

    show(roleId?: number): void {
        const self = this;
        self.active = true;

        self._roleService.getRoleForEdit(roleId).subscribe((result) => {
            self.role = result.role;
            this.permissionTree.editData = result;

            self.modal.show();
        });
    }

    onShown(): void {
        document.getElementById('RoleDisplayName').focus();
        this.markFormAsPristine(this.roleForm);
    }

    save(): void {
        const self = this;

        const input = new CreateOrUpdateRoleInput();
        input.role = self.role;
        input.grantedPermissionNames = self.permissionTree.getGrantedPermissionNames();

        this.saving = true;
        this._roleService
            .createOrUpdateRole(input)
            .pipe(finalize(() => (this.saving = false)))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.markFormAsPristine(this.roleForm);
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {

        if (this.roleForm.dirty) {
            this.swalAlert.fire({
                title: "Are you sure you want to leave?",
                text: "Your changes will be lost.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Yes, proceed!"
            }).then(result => {
                if(result.isConfirmed) {
                    this.modal.hide()
                }
            });

        } else {
            this.modal.hide();
        }
    }
}
