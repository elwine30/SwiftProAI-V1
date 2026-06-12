import { PermissionCheckerService } from 'abp-ng2-module';
import { AppSessionService } from '@shared/common/session/app-session.service';

import { Injectable } from '@angular/core';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';

@Injectable()
export class AppNavigationService {
    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService,
    ) {}

    getMenu(): AppMenu {
        return new AppMenu('MainMenu', 'MainMenu', [
            new AppMenuItem(
                'Dashboard',
                'Pages.Administration.Host.Dashboard',
                'flaticon-line-graph',
                '/app/admin/hostDashboard',
            ),
            new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard', 'flaticon-line-graph', '/app/main/dashboard'),
            new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard3rdParty', 'flaticon-line-graph', '/app/main/dashboard3rdParty'),
            new AppMenuItem('Registration Form', 'Pages.Tenant.Dashboard', 'flaticon-list-2', '/app/main/registration'),
            // new AppMenuItem('OCR', null, 'flaticon-list-2', '/app/main/ocr'),
            new AppMenuItem('Tenants', 'Pages.Tenants', 'flaticon-list-3', '/app/admin/tenants'),
            new AppMenuItem('Editions', 'Pages.Editions', 'flaticon-app', '/app/admin/editions'),
            new AppMenuItem(
                'Administration',
                '',
                'flaticon-interface-8',
                '',
                [],
                [
                    new AppMenuItem(
                        'OrganizationUnits',
                        'Pages.Administration.OrganizationUnits',
                        'flaticon-map',
                        '/app/admin/organization-units',
                    ),
                    new AppMenuItem(
                        'OU Onboarding',
                        'Pages.Administration.OrganizationUnits',
                        'flaticon-map',
                        '/app/admin/registration/onboardingOU',
                    ),
                    new AppMenuItem('Roles', 'Pages.Administration.Roles', 'flaticon-suitcase', '/app/admin/roles'),
                    new AppMenuItem('Users', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/users'),
                    new AppMenuItem(
                        'companySettings',
                        'Pages.Administration.DocumentSettings',
                        'flaticon-settings-1',
                        '/app/admin/common/documentSettings',
                    ),
                    new AppMenuItem(
                        'thirdPartyViewApproval',
                        'Pages.Administration.ThirdPartyViewApproval',
                        'flaticon-settings-1',
                        '/app/admin/common/thirdPartyViewApproval',
                    ),
                    // new AppMenuItem(
                    //     'Vehicles',
                    //     'Pages.Administration.Vehicles',
                    //     'flaticon-truck',
                    //     '/app/admin/vehicles/vehicles',
                    // ),

                    new AppMenuItem(
                        'OpenAIIntegrationLogs',
                        'Pages.Administration.OpenAIIntegrationLogs',
                        'flaticon2-file-1',
                        '/app/admin/integration/openAIIntegrationLogs',
                    ),
                    new AppMenuItem(
                        'Prompts',
                        'Pages.Administration.Prompts',
                        'flaticon-speech-bubble',
                        '/app/admin/ocr/prompts',
                    ),

                    new AppMenuItem('Branches', 'Pages.Branch', 'flaticon-suitcase', '/app/main/branches/branch'),

                    new AppMenuItem(
                        'Groups',
                        'Pages.Administration.Groups',
                        'flaticon-network',
                        '/app/admin/common/groups',
                    ),
                    new AppMenuItem(
                        'Hospitals',
                        'Pages.Hospitals',
                        'flaticon2-hospital', //'fa-regular fa-hospital'
                        '/app/main/registration/hospitals',
                    ),
                    new AppMenuItem(
                        'Scope Of Work',
                        'Pages.Administration.ScopeAssignments',
                        'flaticon-list-2',
                        '/app/admin/registration/scopeAssignments',
                    ),
                    new AppMenuItem(
                        'CaseType',
                        'Pages.Administration.CaseType',
                        'flaticon-squares-3',
                        '/app/admin/casetype/casetype',
                    ),
                    new AppMenuItem(
                        'Companies',
                        'Pages.Administration.Companies',
                        'flaticon-support',
                        '/app/admin/companies/companies',
                    ),
                    new AppMenuItem(
                        'Workshops',
                        'Pages.Administration.Workshops',
                        'flaticon-tool',
                        '/app/admin/workshops/workshops',
                    ),
                    new AppMenuItem(
                        'LawFirms',
                        'Pages.Administration.LawFirms',
                        'flaticon-suitcase',
                        '/app/admin/lawFirms/lawFirms',
                    ),
                    new AppMenuItem(
                        'AuditTrails',
                        'Pages.Administration.AuditTrails',
                        'flaticon-file-1',
                        '/app/admin/audit/auditTrails',
                    ),
                    new AppMenuItem(
                        'DeclarationQuestions',
                        'Pages.Administration.DeclarationQuestions',
                        'flaticon-list',
                        '/app/admin/registration/declarationQuestions',
                    ),
                    // new AppMenuItem(
                    //     'Languages',
                    //     'Pages.Administration.Languages',
                    //     'flaticon-tabs',
                    //     '/app/admin/languages',
                    //     ['/app/admin/languages/{name}/texts'],
                    // ),
                    new AppMenuItem(
                        'AuditLogs',
                        'Pages.Administration.AuditLogs',
                        'flaticon-folder-1',
                        '/app/admin/auditLogs',
                    ),
                    new AppMenuItem(
                        'Maintenance',
                        'Pages.Administration.Host.Maintenance',
                        'flaticon-lock',
                        '/app/admin/maintenance',
                    ),
                    // new AppMenuItem(
                    //     'Subscription',
                    //     'Pages.Administration.Tenant.SubscriptionManagement',
                    //     'flaticon-refresh',
                    //     '/app/admin/subscription-management',
                    // ),
                    // new AppMenuItem(
                    //     'VisualSettings',
                    //     'Pages.Administration.UiCustomization',
                    //     'flaticon-medical',
                    //     '/app/admin/ui-customization',
                    // ),
                    // new AppMenuItem(
                    //     'WebhookSubscriptions',
                    //     'Pages.Administration.WebhookSubscription',
                    //     'flaticon2-world',
                    //     '/app/admin/webhook-subscriptions',
                    // ),
                    // new AppMenuItem(
                    //     'DynamicProperties',
                    //     'Pages.Administration.DynamicProperties',
                    //     'flaticon-interface-8',
                    //     '/app/admin/dynamic-property',
                    // ),
                    // new AppMenuItem(
                    //     'Notifications',
                    //     '',
                    //     'flaticon-alarm',
                    //     '',
                    //     [],
                    //     [
                    //         new AppMenuItem('Inbox', '', 'flaticon-mail-1', '/app/notifications'),
                    //         new AppMenuItem(
                    //             'MassNotifications',
                    //             'Pages.Administration.MassNotification',
                    //             'flaticon-paper-plane',
                    //             '/app/admin/mass-notifications',
                    //         ),
                    //     ],
                    // ),
                    new AppMenuItem(
                        'Settings',
                        'Pages.Administration.Host.Settings',
                        'flaticon-settings',
                        '/app/admin/hostSettings',
                    ),
                    new AppMenuItem(
                        'Settings',
                        'Pages.Administration.Tenant.Settings',
                        'flaticon-settings',
                        '/app/admin/tenantSettings',
                    ),
                ],
            ),
            new AppMenuItem(
                'Reports',
                '',
                'flaticon-graph',
                '',
                [],
                [
                    new AppMenuItem('WIP Report', 'Pages.WIPReports', 'flaticon-graph', '/app/main/reports/wipReports'),
                    new AppMenuItem(
                        'WIPSummaryReports',
                        'Pages.WIPSummaryReports',
                        'flaticon-graph',
                        '/app/main/reports/wipSummaryReports',
                    ),
                    new AppMenuItem(
                        'Case Report',
                        'Pages.CaseReports',
                        'flaticon-graph',
                        '/app/main/reports/caseReports',
                    ),
                    new AppMenuItem(
                        'AdjusterReport',
                        'Pages.AdjusterReports',
                        'flaticon-graph',
                        '/app/main/reports/adjusterReports',
                    ),
                    new AppMenuItem(
                        'Invoice Report',
                        'Pages.InvoiceReports',
                        'flaticon-graph',
                        '/app/main/reports/invoiceReports',
                    ),
                ],
            ),
            new AppMenuItem(
                'Claims',
                '',
                'flaticon-piggy-bank',
                '',
                [],
                [
                    new AppMenuItem(
                        'Manage Claims Info',
                        'Pages.Manage.CaseClaims',
                        'flaticon-piggy-bank',
                        '/app/main/registration/caseClaims/viewAndEdit',
                    ),
                    new AppMenuItem(
                        'Expenses/Claims Approval',
                        'Pages.Administration.ExpensesClaimApproval',
                        'flaticon-piggy-bank',
                        '/app/admin/approval/expensesClaimApproval'
                    ),
                ],
            ),
            // new AppMenuItem(
            //     'DemoUiComponents',
            //     'Pages.DemoUiComponents',
            //     'flaticon-shapes',
            //     '/app/admin/demo-ui-components',
            // ),
        ]);
    }

    checkChildMenuItemPermission(menuItem): boolean {
        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName === '' || subMenuItem.permissionName === null) {
                if (subMenuItem.route) {
                    return true;
                }
            } else if (this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                if (!subMenuItem.hasFeatureDependency()) {
                    return true;
                }

                if (subMenuItem.featureDependencySatisfied()) {
                    return true;
                }
            }

            if (subMenuItem.items && subMenuItem.items.length) {
                let isAnyChildItemActive = this.checkChildMenuItemPermission(subMenuItem);
                if (isAnyChildItemActive) {
                    return true;
                }
            }
        }

        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (
            menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' &&
            this._appSessionService.tenant &&
            !this._appSessionService.tenant.edition
        ) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }

    /**
     * Returns all menu items recursively
     */
    getAllMenuItems(): AppMenuItem[] {
        let menu = this.getMenu();
        let allMenuItems: AppMenuItem[] = [];
        menu.items.forEach((menuItem) => {
            allMenuItems = allMenuItems.concat(this.getAllMenuItemsRecursive(menuItem));
        });

        return allMenuItems;
    }

    private getAllMenuItemsRecursive(menuItem: AppMenuItem): AppMenuItem[] {
        if (!menuItem.items) {
            return [menuItem];
        }

        let menuItems = [menuItem];
        menuItem.items.forEach((subMenu) => {
            menuItems = menuItems.concat(this.getAllMenuItemsRecursive(subMenu));
        });

        return menuItems;
    }
}
