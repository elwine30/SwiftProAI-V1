import { NgModule } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    {
                        path: 'common/documentSettings',
                        loadChildren: () =>
                            import('./common/documentSettings/documentSetting.module').then(
                                (m) => m.DocumentSettingModule,
                            ),
                        data: { permission: 'Pages.Administration.DocumentSettings' },
                    },

                    {
                        path: 'common/thirdPartyViewApproval',
                        loadChildren: () =>
                            import('./common/thirdPartyViewApproval/thirdPartyViewApproval.module').then(
                                (m) => m.ThirdPartyViewApprovalModule,
                            ),
                        // data: { permissions: '' },
                    },

                    {
                        path: 'common/documentSettings',
                        loadChildren: () =>
                            import('./common/documentSettings/documentSetting.module').then(
                                (m) => m.DocumentSettingModule,
                            ),
                        data: { permission: 'Pages.Administration.DocumentSettings' },
                    },

                    {
                        path: 'vehicles/vehicles',
                        loadChildren: () => import('./vehicles/vehicles/vehicle.module').then((m) => m.VehicleModule),
                        data: { permission: 'Pages.Administration.Vehicles' },
                    },

                    {
                        path: 'integration/openAIIntegrationLogs',
                        loadChildren: () =>
                            import('./integration/openAIIntegrationLogs/openAIIntegrationLog.module').then(
                                (m) => m.OpenAIIntegrationLogModule,
                            ),
                        data: { permission: 'Pages.Administration.OpenAIIntegrationLogs' },
                    },

                    {
                        path: 'approval/expensesClaimApproval',
                        loadChildren: () =>
                            import('./expenses-claim-approval/expenses-claim-approval.module').then(
                                (m) => m.ExpensesClaimApprovalModule,
                            ),
                        data: { permission: 'Pages.Administration.ExpensesClaimApproval' }
                    },

                    {
                        path: 'ocr/prompts',
                        loadChildren: () => import('./ocr/prompts/prompt.module').then((m) => m.PromptModule),
                        data: { permission: 'Pages.Administration.Prompts' },
                    },

                    {
                        path: 'audit/auditTrails',
                        loadChildren: () =>
                            import('./audit/auditTrails/auditTrail.module').then((m) => m.AuditTrailModule),
                        data: { permission: 'Pages.Administration.AuditTrails' },
                    },

                    {
                        path: 'companies/companies',
                        loadChildren: () => import('./companies/companies/company.module').then((m) => m.CompanyModule),
                        data: { permission: 'Pages.Administration.Companies' },
                    },

                    {
                        path: 'common/staffs',
                        loadChildren: () => import('./common/staffs/staff.module').then((m) => m.StaffModule),
                        data: { permission: 'Pages.Administration.Staffs' },
                    },

                    {
                        path: 'common/groups',
                        loadChildren: () => import('./common/groups/group.module').then((m) => m.GroupModule),
                        data: { permission: 'Pages.Administration.Groups' },
                    },

                    {
                        path: 'workshops/workshops',
                        loadChildren: () =>
                            import('./workshops/workshops/workshop.module').then((m) => m.WorkshopModule),
                        data: { permission: 'Pages.Administration.Workshops' },
                    },

                    {
                        path: 'casetype/casetype',
                        loadChildren: () => import('./casetype/casetype.module').then((m) => m.CasetypeModule),
                        data: { permission: 'Pages.Administration.CaseType' },
                    },

                    {
                        path: 'lawFirms/lawFirms',
                        loadChildren: () => import('./lawFirms/lawFirms/lawFirm.module').then((m) => m.LawFirmModule),
                        data: { permission: 'Pages.Administration.LawFirms' },
                    },

                    {
                        path: 'registration/declarationQuestions',
                        loadChildren: () =>
                            import('./registration/declarationQuestions/declarationQuestion.module').then(
                                (m) => m.DeclarationQuestionModule,
                            ),
                        data: { permission: 'Pages.Administration.DeclarationQuestions' },
                    },

                    {
                        path: 'registration/declarationQuestions',
                        loadChildren: () =>
                            import('./registration/declarationQuestions/declarationQuestion.module').then(
                                (m) => m.DeclarationQuestionModule,
                            ),
                        data: { permission: 'Pages.Administration.DeclarationQuestions' },
                    },
                    {
                        path: 'registration/onboardingOU',
                        loadChildren: () =>
                            import('./registration/onboardingOU/onboardingOU.module').then((m) => m.OnboardingOUModule),
                        data: { permission: 'Pages.Administration.OrganizationUnits' },
                    },

                    {
                        path: 'registration/scopeAssignments',
                        loadChildren: () =>
                            import('./registration/scopeAssignments/scopeAssignment.module').then(
                                (m) => m.ScopeAssignmentModule,
                            ),
                        data: { permission: 'Pages.Administration.ScopeAssignments' },
                    },
                    {
                        path: 'users',
                        loadChildren: () => import('./users/users.module').then((m) => m.UsersModule),
                        data: { permission: 'Pages.Administration.Users' },
                    },
                    {
                        path: 'roles',
                        loadChildren: () => import('./roles/roles.module').then((m) => m.RolesModule),
                        data: { permission: 'Pages.Administration.Roles' },
                    },
                    {
                        path: 'auditLogs',
                        loadChildren: () => import('./audit-logs/audit-logs.module').then((m) => m.AuditLogsModule),
                        data: { permission: 'Pages.Administration.AuditLogs' },
                    },
                    {
                        path: 'maintenance',
                        loadChildren: () => import('./maintenance/maintenance.module').then((m) => m.MaintenanceModule),
                        data: { permission: 'Pages.Administration.Host.Maintenance' },
                    },
                    {
                        path: 'hostSettings',
                        loadChildren: () => import('./settings/host-settings.module').then((m) => m.HostSettingsModule),
                        data: { permission: 'Pages.Administration.Host.Settings' },
                    },
                    {
                        path: 'editions',
                        loadChildren: () => import('./editions/editions.module').then((m) => m.EditionsModule),
                        data: { permission: 'Pages.Editions' },
                    },
                    {
                        path: 'languages',
                        loadChildren: () => import('./languages/languages.module').then((m) => m.LanguagesModule),
                        data: { permission: 'Pages.Administration.Languages' },
                    },
                    {
                        path: 'languages/:name/texts',
                        loadChildren: () =>
                            import('./languages/language-texts.module').then((m) => m.LanguageTextsModule),
                        data: { permission: 'Pages.Administration.Languages.ChangeTexts' },
                    },
                    {
                        path: 'tenants',
                        loadChildren: () => import('./tenants/tenants.module').then((m) => m.TenantsModule),
                        data: { permission: 'Pages.Tenants' },
                    },
                    {
                        path: 'organization-units',
                        loadChildren: () =>
                            import('./organization-units/organization-units.module').then(
                                (m) => m.OrganizationUnitsModule,
                            ),
                        data: { permission: 'Pages.Administration.OrganizationUnits' },
                    },
                    {
                        path: 'subscription-management',
                        loadChildren: () =>
                            import('./subscription-management/subscription-management.module').then(
                                (m) => m.SubscriptionManagementModule,
                            ),
                        data: { permission: 'Pages.Administration.Tenant.SubscriptionManagement' },
                    },
                    {
                        path: 'invoice/:paymentId',
                        loadChildren: () =>
                            import('./subscription-management/invoice/invoice.module').then((m) => m.InvoiceModule),
                        data: { permission: 'Pages.Administration.Tenant.SubscriptionManagement' },
                    },
                    {
                        path: 'tenantSettings',
                        loadChildren: () =>
                            import('./settings/tenant-settings.module').then((m) => m.TenantSettingsModule),
                        data: { permission: 'Pages.Administration.Tenant.Settings' },
                    },
                    {
                        path: 'hostDashboard',
                        loadChildren: () =>
                            import('./dashboard/host-dashboard.module').then((m) => m.HostDashboardModule),
                        data: { permission: 'Pages.Administration.Host.Dashboard' },
                    },
                    {
                        path: 'demo-ui-components',
                        loadChildren: () =>
                            import('./demo-ui-components/demo-ui-components.module').then(
                                (m) => m.DemoUIComponentsModule,
                            ),
                        data: { permission: 'Pages.DemoUiComponents' },
                    },
                    {
                        path: 'install',
                        loadChildren: () => import('./install/install.module').then((m) => m.InstallModule),
                    },
                    {
                        path: 'ui-customization',
                        loadChildren: () =>
                            import('./ui-customization/ui-customization.module').then((m) => m.UICustomizationModule),
                    },
                    {
                        path: 'webhook-subscriptions',
                        loadChildren: () =>
                            import('./webhook-subscription/webhook-subscription.module').then(
                                (m) => m.WebhookSubscriptionModule,
                            ),
                        data: { permission: 'Pages.Administration.WebhookSubscription' },
                    },
                    {
                        path: 'webhook-subscriptions-detail',
                        loadChildren: () =>
                            import('./webhook-subscription/webhook-subscription-detail.module').then(
                                (m) => m.WebhookSubscriptionDetailModule,
                            ),
                        data: { permission: 'Pages.Administration.WebhookSubscription.Detail' },
                    },
                    {
                        path: 'webhook-event-detail',
                        loadChildren: () =>
                            import('./webhook-subscription/webhook-event-detail.module').then(
                                (m) => m.WebhookEventDetailModule,
                            ),
                        data: { permission: 'Pages.Administration.WebhookSubscription.Detail' },
                    },
                    {
                        path: 'dynamic-property',
                        loadChildren: () =>
                            import('./dynamic-properties/dynamic-properties.module').then(
                                (m) => m.DynamicPropertiesModule,
                            ),
                        data: { permission: 'Pages.Administration.DynamicEntityProperties' },
                    },
                    {
                        path: 'dynamic-entity-property/:entityFullName',
                        loadChildren: () =>
                            import(
                                './dynamic-properties/dynamic-entity-properties/dynamic-entity-properties.module'
                            ).then((m) => m.DynamicEntityPropertiesModule),
                        data: { permission: 'Pages.Administration.DynamicEntityProperties' },
                    },
                    {
                        path: 'dynamic-entity-property-value/manage-all/:entityFullName/:rowId',
                        loadChildren: () =>
                            import(
                                './dynamic-properties/dynamic-entity-properties/value/dynamic-entity-property-value.module'
                            ).then((m) => m.DynamicEntityPropertyValueModule),
                        data: { permission: 'Pages.Administration.DynamicEntityProperties' },
                    },
                    {
                        path: 'login-attempts',
                        loadChildren: () =>
                            import('./login-attempts/login-attempts.module').then((m) => m.LoginAttemptsModule),
                        data: {},
                    },
                    {
                        path: 'mass-notifications',
                        loadChildren: () =>
                            import('./mass-notifications/mass-notifications.module').then(
                                (m) => m.MassNotificationsModule,
                            ),
                        data: { permission: 'Pages.Administration.MassNotification' },
                    },
                    { path: '', redirectTo: 'hostDashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'hostDashboard' },
                ],
            },
        ]),
    ],
    exports: [RouterModule],
})
export class AdminRoutingModule {
    constructor(private router: Router) {
        router.events.subscribe((event) => {
            if (event instanceof NavigationEnd) {
                window.scroll(0, 0);
            }
        });
    }
}
