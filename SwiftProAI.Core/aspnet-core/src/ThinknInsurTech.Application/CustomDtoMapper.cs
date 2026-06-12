using ThinknInsurTech.Approval.Dtos;
using ThinknInsurTech.Approval;
using ThinknInsurTech.Organizations.Dtos;
using ThinknInsurTech.Organizations;
using ThinknInsurTech.Vehicles.Dtos;
using ThinknInsurTech.Vehicles;
using ThinknInsurTech.Integration.Dtos;
using ThinknInsurTech.Integration;
using ThinknInsurTech.OCR.Dtos;
using ThinknInsurTech.OCR;
using ThinknInsurTech.Audit.Dtos;
using ThinknInsurTech.Audit;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Reports;
using ThinknInsurTech.Branches.Dtos;
using ThinknInsurTech.Branches;
using ThinknInsurTech.Companies.Dtos;
using ThinknInsurTech.Companies;
using ThinknInsurTech.Workshops.Dtos;
using ThinknInsurTech.Workshops;
using ThinknInsurTech.LawFirms.Dtos;
using ThinknInsurTech.LawFirms;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Common;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Registration;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Extensions;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using ThinknInsurTech.Auditing.Dto;
using ThinknInsurTech.Authorization.Accounts.Dto;
using ThinknInsurTech.Authorization.Delegation;
using ThinknInsurTech.Authorization.Permissions.Dto;
using ThinknInsurTech.Authorization.Roles;
using ThinknInsurTech.Authorization.Roles.Dto;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Authorization.Users.Delegation.Dto;
using ThinknInsurTech.Authorization.Users.Dto;
using ThinknInsurTech.Authorization.Users.Importing.Dto;
using ThinknInsurTech.Authorization.Users.Profile.Dto;
using ThinknInsurTech.Case;
using ThinknInsurTech.Case.Dto;
using ThinknInsurTech.Chat;
using ThinknInsurTech.Chat.Dto;
using ThinknInsurTech.Common.Dto;
using ThinknInsurTech.DynamicEntityProperties.Dto;
using ThinknInsurTech.Editions;
using ThinknInsurTech.Editions.Dto;
using ThinknInsurTech.Friendships;
using ThinknInsurTech.Friendships.Cache;
using ThinknInsurTech.Friendships.Dto;
using ThinknInsurTech.Localization.Dto;
using ThinknInsurTech.MultiTenancy;
using ThinknInsurTech.MultiTenancy.Dto;
using ThinknInsurTech.MultiTenancy.HostDashboard.Dto;
using ThinknInsurTech.MultiTenancy.Payments;
using ThinknInsurTech.MultiTenancy.Payments.Dto;
using ThinknInsurTech.Notifications.Dto;
using ThinknInsurTech.Organizations.Dto;
using ThinknInsurTech.Registration.Dto;
using ThinknInsurTech.Sessions.Dto;
using ThinknInsurTech.WebHooks.Dto;

namespace ThinknInsurTech
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditCasePoliceReportSummaryDto, CasePoliceReportSummary>().ReverseMap();
            configuration.CreateMap<CasePoliceReportSummaryDto, CasePoliceReportSummary>().ReverseMap();
            configuration.CreateMap<CreateOrEditViewThirdPartyCaseRequestDto, ViewThirdPartyCaseRequest>().ReverseMap();
            configuration.CreateMap<ViewThirdPartyCaseRequestDto, ViewThirdPartyCaseRequest>().ReverseMap();
            configuration.CreateMap<CreateOrEditViewThirdPartyCaseDto, ViewThirdPartyCases>().ReverseMap();
            configuration.CreateMap<ViewThirdPartyCasesDto, ViewThirdPartyCases>().ReverseMap();
            configuration.CreateMap<CreateOrEditDocumentSettingDto, DocumentSetting>().ReverseMap();
            configuration.CreateMap<DocumentSettingDto, DocumentSetting>().ReverseMap();
            configuration.CreateMap<CreateOrEditVehicleDto, Vehicle>().ReverseMap();
            configuration.CreateMap<VehicleDto, Vehicle>().ReverseMap();
            configuration.CreateMap<OpenAIIntegrationLogDto, OpenAIIntegrationLog>().ReverseMap();
            configuration.CreateMap<FileOrgDto, FileOrg>().ReverseMap();
            configuration.CreateMap<FolderDto, Folder>().ReverseMap();
            configuration.CreateMap<CreateOrEditPromptDto, Prompt>().ReverseMap();
            configuration.CreateMap<PromptDto, Prompt>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseCreditNoteDto, CaseCreditNote>().ReverseMap();
            configuration.CreateMap<CaseCreditNoteDto, CaseCreditNote>().ReverseMap();
            configuration.CreateMap<CreateOrEditCreditNoteItemDto, CreditNoteItem>().ReverseMap();
            configuration.CreateMap<CreditNoteItemDto, CreditNoteItem>().ReverseMap();
            configuration.CreateMap<InvoiceReportDto, InvoiceReport>().ReverseMap();
            configuration.CreateMap<CreateOrEditDebitNoteItemDto, DebitNoteItem>().ReverseMap();
            configuration.CreateMap<DebitNoteItemDto, DebitNoteItem>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseDebitNoteDto, CaseDebitNote>().ReverseMap();
            configuration.CreateMap<CaseDebitNoteDto, CaseDebitNote>().ReverseMap();
            configuration.CreateMap<CreateOrEditAuditEntryDto, AuditEntry>().ReverseMap();
            configuration.CreateMap<AuditEntryDto, AuditEntry>().ReverseMap();
            configuration.CreateMap<CreateOrEditAuditTrailDto, AuditTrail>().ReverseMap();
            configuration.CreateMap<AuditTrailDto, AuditTrail>().ReverseMap();
            configuration.CreateMap<CaseReportDto, CaseReport>().ReverseMap();
            configuration.CreateMap<CreateOrEditInvoiceItemDto, InvoiceItem>().ReverseMap();
            configuration.CreateMap<InvoiceItemDto, InvoiceItem>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseInvoiceDto, CaseInvoice>().ReverseMap();
            configuration.CreateMap<CaseInvoiceDto, CaseInvoice>().ReverseMap();
            configuration.CreateMap<CreateOrEditCompanyDto, InsuranceCompany>().ReverseMap();
            configuration.CreateMap<CompanyDto, InsuranceCompany>().ReverseMap();
            configuration.CreateMap<WIPReportDto, WIPReport>().ReverseMap();
            //configuration.CreateMap<CreateOrEditWIPSummaryReportDto, WIPSummaryReport>().ReverseMap();
            configuration.CreateMap<WIPSummaryReportDto, WIPSummaryReport>().ReverseMap();
            configuration.CreateMap<CreateExpenseInput, CaseExpense>();
            configuration.CreateMap<CaseExpenseDto, CaseExpense>().ReverseMap();
            configuration.CreateMap<CreateOrEditStaffDto, Staff>().ReverseMap();
            configuration.CreateMap<StaffDto, Staff>().ReverseMap();
            configuration.CreateMap<CreateOrEditGroupDto, Group>().ReverseMap();
            configuration.CreateMap<GroupDto, Group>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseClaimDto, CaseClaim>().ReverseMap();
            configuration.CreateMap<CaseClaimDto, CaseClaim>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseSearchFeeDto, CaseSearchFee>().ReverseMap();
            configuration.CreateMap<CaseSearchFeeDto, CaseSearchFee>().ReverseMap();
            configuration.CreateMap<CreateOrEditBranchDto, Branch>().ReverseMap();
            configuration.CreateMap<BranchDto, Branch>().ReverseMap();
            configuration.CreateMap<CreateOrEditLookupDto, Lookup>().ReverseMap();
            configuration.CreateMap<LookupDto, Lookup>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseDeclarationAnswerDto, CaseDeclarationAnswer>().ReverseMap();
            configuration.CreateMap<CaseDeclarationAnswerDto, CaseDeclarationAnswer>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseThirdPartyInfoDto, CaseThirdPartyInfo>().ReverseMap();
            configuration.CreateMap<CaseThirdPartyInfoDto, CaseThirdPartyInfo>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseThirdPartyVehicleDto, CaseThirdPartyVehicle>().ReverseMap();
            configuration.CreateMap<CaseThirdPartyVehicleDto, CaseThirdPartyVehicle>().ReverseMap();
            configuration.CreateMap<CreateOrEditCasePoliceReportDto, CasePoliceReport>().ReverseMap();
            configuration.CreateMap<CasePoliceReportDto, CasePoliceReport>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseIncidentDetailDto, CaseIncidentDetail>().ReverseMap();
            configuration.CreateMap<CaseIncidentDetailDto, CaseIncidentDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseWorkshopDto, CaseWorkshop>().ReverseMap();
            configuration.CreateMap<CaseWorkshopDto, CaseWorkshop>().ReverseMap();
            configuration.CreateMap<CreateOrEditWorkshopDto, Workshop>().ReverseMap();
            configuration.CreateMap<WorkshopDto, Workshop>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseLawyerDto, CaseLawyer>().ReverseMap();
            configuration.CreateMap<CaseLawyerDto, CaseLawyer>().ReverseMap();
            configuration.CreateMap<CreateOrEditLawFirmDto, LawFirm>().ReverseMap();
            configuration.CreateMap<LawFirmDto, LawFirm>().ReverseMap();
            configuration.CreateMap<CreateOrEditInsuredPersonDto, CaseInsuredPerson>().ReverseMap();
            configuration.CreateMap<InsuredPersonDto, CaseInsuredPerson>().ReverseMap();
            configuration.CreateMap<CreateOrEditInsuredPersonDto, CaseInsuredPerson>().ReverseMap();
            configuration.CreateMap<InsuredPersonDto, CaseInsuredPerson>().ReverseMap();
            configuration.CreateMap<CreateOrEditHospitalDto, Hospital>().ReverseMap();
            configuration.CreateMap<HospitalDto, Hospital>().ReverseMap();
            configuration.CreateMap<CreateOrEditLocationDto, Location>().ReverseMap();
            configuration.CreateMap<LocationDto, Location>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseInsurerDto, CaseInsurer>().ReverseMap();
            configuration.CreateMap<CaseInsurerDto, CaseInsurer>().ReverseMap();
            configuration.CreateMap<CreateOrEditDeclarationQuestionDto, DeclarationQuestion>().ReverseMap();
            configuration.CreateMap<DeclarationQuestionDto, DeclarationQuestion>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseAdjusterDto, CaseAdjuster>().ReverseMap();
            configuration.CreateMap<CaseAdjusterDto, CaseAdjuster>().ReverseMap();
            configuration.CreateMap<CreateOrEditScopeAssignmentDto, ScopeAssignment>().ReverseMap();
            configuration.CreateMap<ScopeAssignmentDto, ScopeAssignment>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaseTypeDto, CaseType>().ReverseMap();
            configuration.CreateMap<CaseTypeDto, CaseType>().ReverseMap();

            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();

            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>()
                .ReverseMap()
                .ForMember(dto => dto.TotalAmount, options => options.MapFrom(e => e.GetTotalAmount()));
            configuration.CreateMap<SubscriptionPaymentProductDto, SubscriptionPaymentProduct>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();
            configuration.CreateMap<User, FindUsersOutputDto>();
            configuration.CreateMap<User, FindOrganizationUnitUsersOutputDto>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.DisplayName.IsNullOrEmpty() ? entity.DynamicProperty.PropertyName : entity.DynamicProperty.DisplayName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();

            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}