import { Component, ViewChild } from "@angular/core";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ViewCaseLawyerComponent } from "../caseLawyers/view-caseLawyer.component";
import { StepNavComponent } from "@app/shared/common/registration/step-nav.component";
import { ActionsButtonComponent } from "@app/shared/common/registration/actions-button.component";
import { ViewCaseInsurerComponent } from "../caseInsurers/view-caseInsurer.component";
import { ViewCaseWorkshopComponent } from "../caseWorkshops/view-caseWorkshop.component";


@Component({
    templateUrl: './view-caseStakeHolder.component.html',
    animations: [appModuleAnimation()]
})


export class ViewCaseStakeholderComponent extends AppComponentBase{
    @ViewChild(ActionsButtonComponent) actionsButton: ActionsButtonComponent;
    @ViewChild(StepNavComponent) stepNav: StepNavComponent;
    @ViewChild(ViewCaseLawyerComponent) viewCaseLawyer: ViewCaseLawyerComponent ;
    @ViewChild(ViewCaseInsurerComponent) viewCaseInsurer:ViewCaseInsurerComponent;
    @ViewChild(ViewCaseWorkshopComponent) viewCaseWorkshop:ViewCaseWorkshopComponent;
    
}