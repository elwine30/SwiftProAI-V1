import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { StepNavComponent } from '@app/shared/common/registration/step-nav.component';
import { CreateOrEditCaseLawyerComponent } from '../caseLawyers/create-or-edit-caseLawyer.component';
import { CreateOrEditCaseWorkshopComponent } from '../caseWorkshops/create-or-edit-caseWorkshop.component';
import { ActionsButtonComponent } from '@app/shared/common/registration/actions-button.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import Swal from 'sweetalert2';

@Component({
    templateUrl: './create-or-edit-caseStakeholder.component.html',
    animations: [appModuleAnimation()]
})
export class CreateOrEditCaseStakeholderComponent extends AppComponentBase implements DirtyFormGuard {
    @ViewChild(ActionsButtonComponent) actionsButton: ActionsButtonComponent;
    @ViewChild(StepNavComponent) stepNav: StepNavComponent;
    @ViewChild(CreateOrEditCaseLawyerComponent) createCaseLawyer: CreateOrEditCaseLawyerComponent ;
    @ViewChild(CreateOrEditCaseWorkshopComponent) createCaseWorkshop: CreateOrEditCaseWorkshopComponent ;

    swalAlert = Swal;

    canDeactivate(): Promise<boolean> | boolean {
        this.hideMainSpinner();

        if (this.createCaseLawyer.caseLawyerForm.dirty || this.createCaseWorkshop.caseWorkshopForm.dirty || this.createCaseWorkshop.createCaseInsurer.caseInsurerForm.dirty) {
            var returnVal = this.swalAlert.fire({
                title: "Are you sure you want to leave?",
                text: "Your changes will be lost.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Yes, proceed!"
            });

            return returnVal.then(result => result.isConfirmed);
        } else {
            return true;
        }
    }
}
