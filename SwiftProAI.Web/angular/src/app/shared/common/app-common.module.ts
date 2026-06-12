import { CommonModule } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppLocalizationService } from '@app/shared/common/localization/app-localization.service';
import { AppNavigationService } from '@app/shared/layout/nav/app-navigation.service';
import { ThinknInsurTechCommonModule } from '@shared/common/common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import {
    BsDatepickerModule,
    BsDatepickerConfig,
    BsDaterangepickerConfig,
    BsLocaleService,
} from 'ngx-bootstrap/datepicker';
import { PaginatorModule } from 'primeng/paginator';
import { TableModule } from 'primeng/table';
import { AppAuthService } from './auth/app-auth.service';
import { AppRouteGuard } from './auth/auth-route-guard';
import { CommonLookupModalComponent } from './lookup/common-lookup-modal.component';
import { EntityTypeHistoryModalComponent } from './entityHistory/entity-type-history-modal.component';
import { EntityChangeDetailModalComponent } from './entityHistory/entity-change-detail-modal.component';
import { DateRangePickerInitialValueSetterDirective } from './timing/date-range-picker-initial-value.directive';
import { DatePickerInitialValueSetterDirective } from './timing/date-picker-initial-value.directive';
import { DateTimeService } from './timing/date-time.service';
import { TimeZoneComboComponent } from './timing/timezone-combo.component';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { PerfectScrollbarModule } from '@craftsjs/perfect-scrollbar';
import { AppBsModalModule } from '@shared/common/appBsModal/app-bs-modal.module';
import { SingleLineStringInputTypeComponent } from './input-types/single-line-string-input-type/single-line-string-input-type.component';
import { ComboboxInputTypeComponent } from './input-types/combobox-input-type/combobox-input-type.component';
import { CheckboxInputTypeComponent } from './input-types/checkbox-input-type/checkbox-input-type.component';
import { MultipleSelectComboboxInputTypeComponent } from './input-types/multiple-select-combobox-input-type/multiple-select-combobox-input-type.component';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { PasswordInputWithShowButtonComponent } from './password-input-with-show-button/password-input-with-show-button.component';
import { KeyValueListManagerComponent } from './key-value-list-manager/key-value-list-manager.component';
import { Angular2CountoModule } from '@awaismirza/angular2-counto';
import { StepNavComponent } from './registration/step-nav.component';
import { ActionsButtonComponent } from './registration/actions-button.component';
import { CreateOrEditCaseDeclarationAnswerModalComponent } from '@app/main/registration/caseDeclarationAnswers/create-or-edit-caseDeclarationAnswer-modal.component';
import { CreateAndViewExpensesModal } from '@app/main/registration/caseExpenses/create-and-view-caseExpenses.component';
import { CreateRemarkModalComponent } from '../../main/registration/create-remark-modal.component';
import { AddRemarkButtonComponent } from './registration/add-remark-button.component';
import { AppRoutingModule } from '@app/app-routing.module';
import { OnlyNumberDirective } from './input-types/only-number-input-type/only-number.directive';
import { AmountInputDirective } from './input-types/amount-input-type/amount-input.directive';
import { CreateAndViewExpensesModalComponent } from './modal/expensess/createOrEdit-expensess-modal.component';
import { DecimalFormatDirective } from './input-types/decimalFormat-input-type/decimalFormat-input.directive';
import { ViewCaseDeclarationAnswerModalComponent } from '@app/main/registration/caseDeclarationAnswers/view-caseDeclarationAnswer-modal.component';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ModalModule.forRoot(),
        UtilsModule,
        ThinknInsurTechCommonModule,
        TableModule,
        AppRoutingModule,
        PaginatorModule,
        TabsModule.forRoot(),
        BsDropdownModule.forRoot(),
        BsDatepickerModule.forRoot(),
        PerfectScrollbarModule,
        Angular2CountoModule,
        AppBsModalModule,
        AutoCompleteModule,
    ],
    declarations: [
        CreateAndViewExpensesModal,
        CreateOrEditCaseDeclarationAnswerModalComponent,
        ViewCaseDeclarationAnswerModalComponent,
        TimeZoneComboComponent,
        CommonLookupModalComponent,
        StepNavComponent,
        ActionsButtonComponent,
        EntityTypeHistoryModalComponent,
        EntityChangeDetailModalComponent,
        DateRangePickerInitialValueSetterDirective,
        DatePickerInitialValueSetterDirective,
        SingleLineStringInputTypeComponent,
        ComboboxInputTypeComponent,
        CheckboxInputTypeComponent,
        MultipleSelectComboboxInputTypeComponent,
        PasswordInputWithShowButtonComponent,
        KeyValueListManagerComponent,
        CreateRemarkModalComponent,
        AddRemarkButtonComponent,
        OnlyNumberDirective,
        AmountInputDirective,
        CreateAndViewExpensesModalComponent,
        DecimalFormatDirective,
    ],
    exports: [
        TimeZoneComboComponent,
        CommonLookupModalComponent,
        StepNavComponent,
        ActionsButtonComponent,
        EntityTypeHistoryModalComponent,
        EntityChangeDetailModalComponent,
        DateRangePickerInitialValueSetterDirective,
        DatePickerInitialValueSetterDirective,
        PasswordInputWithShowButtonComponent,
        KeyValueListManagerComponent,
        CreateRemarkModalComponent,
        AddRemarkButtonComponent,
        OnlyNumberDirective,
        AmountInputDirective,
        CreateAndViewExpensesModalComponent,
        DecimalFormatDirective,
    ],
    providers: [
        DateTimeService,
        AppLocalizationService,
        AppNavigationService,
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
    ]
})
export class AppCommonModule {
    static forRoot(): ModuleWithProviders<AppCommonModule> {
        return {
            ngModule: AppCommonModule,
            providers: [AppAuthService, AppRouteGuard],
        };
    }
}
