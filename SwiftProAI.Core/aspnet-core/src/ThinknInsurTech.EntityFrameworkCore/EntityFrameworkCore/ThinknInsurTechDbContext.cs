using ThinknInsurTech.Approval;
using ThinknInsurTech.Organizations;
using ThinknInsurTech.Vehicles;
using ThinknInsurTech.Integration;
using ThinknInsurTech.OCR;
using ThinknInsurTech.Audit;
using ThinknInsurTech.Reports;
using ThinknInsurTech.Workshops;
using ThinknInsurTech.LawFirms;
using ThinknInsurTech.Common;
using System.Collections.Generic;
using System.Text.Json;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThinknInsurTech.Authorization.Delegation;
using ThinknInsurTech.Authorization.Roles;
using ThinknInsurTech.Authorization.Users;
using ThinknInsurTech.Chat;
using ThinknInsurTech.Editions;
using ThinknInsurTech.ExtraProperties;
using ThinknInsurTech.Friendships;
using ThinknInsurTech.MultiTenancy;
using ThinknInsurTech.MultiTenancy.Accounting;
using ThinknInsurTech.MultiTenancy.Payments;
using ThinknInsurTech.OpenIddict.Applications;
using ThinknInsurTech.OpenIddict.Authorizations;
using ThinknInsurTech.OpenIddict.Scopes;
using ThinknInsurTech.OpenIddict.Tokens;
using ThinknInsurTech.Registration;
using ThinknInsurTech.Case;
using ThinknInsurTech.Companies;
using ThinknInsurTech.Branches;
using ThinknInsurTech.Remarks;
using ThinknInsurTech.Storage;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Reflection;
using Abp.Auditing;
using PayPalCheckoutSdk.Orders;
using Abp.Organizations;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using ThinknInsurTech.Runtime;
using Abp.Authorization.Users;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ThinknInsurTech.EntityFrameworkCore
{
    public class ThinknInsurTechDbContext : AbpZeroDbContext<Tenant, Role, User, ThinknInsurTechDbContext>, IOpenIddictDbContext
    {
        public virtual DbSet<CasePoliceReportSummary> CasePoliceReportSummaries { get; set; }

        public virtual DbSet<ViewThirdPartyCaseRequest> ViewThirdPartyCaseRequests { get; set; }

        public virtual DbSet<ViewThirdPartyCases> MainRegistrationOrganizationUnits { get; set; }

        public virtual DbSet<DocumentSetting> DocumentSettings { get; set; }

        public virtual DbSet<Vehicle> Vehicles { get; set; }

        public virtual DbSet<OpenAIIntegrationLog> OpenAIIntegrationLogs { get; set; }

        public virtual DbSet<FileOrg> FileOrgs { get; set; }

        public virtual DbSet<Folder> Folders { get; set; }

        public virtual DbSet<Prompt> Prompts { get; set; }

        public virtual DbSet<CaseCreditNote> CaseCreditNotes { get; set; }

        public virtual DbSet<CreditNoteItem> CreditNoteItems { get; set; }

        public virtual DbSet<DebitNoteItem> DebitNoteItems { get; set; }

        public virtual DbSet<CaseDebitNote> CaseDebitNotes { get; set; }

        public virtual DbSet<AuditEntry> AuditEntries { get; set; }

        public virtual DbSet<AuditTrail> AuditTrails { get; set; }

        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

        public virtual DbSet<CaseInvoice> CaseInvoices { get; set; }

        public virtual DbSet<CaseClaim> CaseClaims { get; set; }

        public virtual DbSet<CaseSearchFee> CaseSearchFees { get; set; }

        public virtual DbSet<CaseExpense> CaseExpenses { get; set; }

        public virtual DbSet<Staff> Staffs { get; set; }

        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }

        public virtual DbSet<CaseDeclarationAnswer> CaseDeclarationAnswers { get; set; }

        public virtual DbSet<Lookup> Lookups { get; set; }

        public virtual DbSet<CaseThirdPartyInfo> CaseThirdPartyInfos { get; set; }

        public virtual DbSet<CaseInsuredPerson> CaseInsuredPersons { get; set; }

        public virtual DbSet<CaseThirdPartyVehicle> CaseThirdPartyVehicles { get; set; }

        public virtual DbSet<CasePoliceReport> CasePoliceReports { get; set; }

        public virtual DbSet<CaseIncidentDetail> CaseIncidentDetails { get; set; }

        public virtual DbSet<CaseWorkshop> CaseWorkshops { get; set; }

        public virtual DbSet<Workshop> Workshops { get; set; }

        public virtual DbSet<CaseLawyer> CaseLawyers { get; set; }

        public virtual DbSet<LawFirm> LawFirms { get; set; }

        public virtual DbSet<Hospital> Hospitals { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<CaseInsurer> CaseInsurers { get; set; }

        public virtual DbSet<DeclarationQuestion> DeclarationQuestions { get; set; }

        public virtual DbSet<CaseAdjuster> CaseAdjusters { get; set; }

        public virtual DbSet<ScopeAssignment> ScopeAssignments { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<OpenIddictApplication> Applications { get; }

        public virtual DbSet<OpenIddictAuthorization> Authorizations { get; }

        public virtual DbSet<OpenIddictScope> Scopes { get; }

        public virtual DbSet<OpenIddictToken> Tokens { get; }

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<SubscriptionPaymentProduct> SubscriptionPaymentProducts { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public virtual DbSet<MainRegistration> MainRegistrations { get; set; }

        public virtual DbSet<CaseType> CaseTypes { get; set; }

        public virtual DbSet<InsuranceCompany> Companies { get; set; }

        public virtual DbSet<Status> Statuses { get; set; }

        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Remark> Remarks { get; set; }

        public virtual DbSet<OrganizationUnit> OrganisationUnits { get; set; }

        protected virtual bool IsOUFilterEnabled => CurrentOUId != null && CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(ThinknInsurTechDataFilters.HaveOrganizationUnit) == true;

        protected virtual long? CurrentOUId => GetCurrentUsersOuIdOrNull();

        public ThinknInsurTechDbContext(DbContextOptions<ThinknInsurTechDbContext> options)
            : base(options)
        {
        }

        // overriding method to check if we need to do OU filter based on if the entity type TEntity implements the interfaces
        protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
        {
            if (typeof(IMayHaveOrganizationUnit).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            if (typeof(IMustHaveOrganizationUnit).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            //this always return true unless the the filtered entity is set to Disable in the CurrentUnitOfWork 
            return base.ShouldFilterEntity<TEntity>(entityType);
        }

        // overriding method to create filter expressions to be applied to queries for entities that should be filtered
        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
        {
            // includes existing filter logic
            var expression = base.CreateFilterExpression<TEntity>();

            if (typeof(IMayHaveOrganizationUnit).IsAssignableFrom(typeof(TEntity)))
            {
                Logger.DebugFormat("In IMayHaveOrganizationUnit Filter Expression, CurrentOUId is: " + CurrentOUId.ToString());
                // filter to include only entities where the OUId matches with current OUId or where current OUId is null or where OUId filter is disabled
                Expression<Func<TEntity, bool>> mayHaveOUFilter = e => !IsOUFilterEnabled || ((IMayHaveOrganizationUnit)e).OrganizationUnitId == CurrentOUId;
                expression = expression == null ? mayHaveOUFilter : CombineExpressions(expression, mayHaveOUFilter);
            }

            if (typeof(IMustHaveOrganizationUnit).IsAssignableFrom(typeof(TEntity)))
            {
                Logger.DebugFormat("In IMustHaveOrganizationUnit Filter Expression, CurrentOUId is: " + CurrentOUId);
                Expression<Func<TEntity, bool>> mustHaveOUFilter = e => !IsOUFilterEnabled || ((IMustHaveOrganizationUnit)e).OrganizationUnitId == CurrentOUId;

                expression = expression == null ? mustHaveOUFilter : CombineExpressions(expression, mustHaveOUFilter);
            }

            return expression;
        }

        protected virtual long? GetCurrentUsersOuIdOrNull()
        {

            if (AbpSession.GetCurrentOUId() == null && CurrentUnitOfWorkProvider != null && CurrentUnitOfWorkProvider.Current != null)
            {
                // get OU filter from current unit of work
                var organisationUnitFilter = CurrentUnitOfWorkProvider.Current.Filters.FirstOrDefault(f => f.FilterName == ThinknInsurTechDataFilters.HaveOrganizationUnit);
                if (organisationUnitFilter != null && organisationUnitFilter.FilterParameters.ContainsKey(ThinknInsurTechDataFilters.HaveOrganizationUnit_Filter_Parameter))
                    return organisationUnitFilter.FilterParameters[ThinknInsurTechDataFilters.HaveOrganizationUnit_Filter_Parameter] as long?;
            }

            return AbpSession.GetCurrentOUId();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    // max char length value in sqlserver
                    if (property.GetMaxLength() == 67108864)
                        // max char length value in postgresql
                        property.SetMaxLength(10485760);
                }
            }

            modelBuilder.Entity<CasePoliceReportSummary>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<ViewThirdPartyCaseRequest>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ViewThirdPartyCases>(m =>
                       {
                           m.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DocumentSetting>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<FileOrg>(f =>
                                  {
                                      f.HasIndex(e => new { e.TenantId });
                                  });
            modelBuilder.Entity<Prompt>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Folder>(f =>
                       {
                           f.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CreditNoteItem>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNoteItem>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseDebitNote>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CaseCreditNote>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<AuditEntry>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<AuditTrail>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<AuditEntry>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<AuditTrail>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<InvoiceItem>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseInvoice>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<InsuranceCompany>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<CaseClaim>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseSearchFee>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseExpense>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Staff>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Group>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Branch>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseDeclarationAnswer>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Lookup>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseThirdPartyInfo>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseThirdPartyVehicle>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CasePoliceReport>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseIncidentDetail>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseWorkshop>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseLawyer>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseWorkshop>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Workshop>(w =>
            {
                w.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseLawyer>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LawFirm>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseInsuredPerson>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseInsuredPerson>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseInsurer>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseInsurer>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<DeclarationQuestion>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CaseAdjuster>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<ScopeAssignment>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<BinaryObject>(b => { b.HasIndex(e => new { e.TenantId }); });

            modelBuilder.Entity<SubscriptionPayment>(x =>
            {
                x.Property(u => u.ExtraProperties)
                    .HasConversion(
                        d => JsonSerializer.Serialize(d, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        }),
                        s => JsonSerializer.Deserialize<ExtraPropertyDictionary>(s, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        })
                    );
            });

            modelBuilder.Entity<SubscriptionPaymentProduct>(x =>
            {
                x.Property(u => u.ExtraProperties)
                    .HasConversion(
                        d => JsonSerializer.Serialize(d, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        }),
                        s => JsonSerializer.Deserialize<ExtraPropertyDictionary>(s, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        })
                    );
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigureOpenIddict();
        }

        #region Audit Trail
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityAuditInformation = BeforeSaveChanges();
                var returnValue = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

                // Only process audit entries if there were changes saved
                if (returnValue > 0)
                {
                    var user = AbpSession.UserId.HasValue
                        ? await Users.FirstOrDefaultAsync(u => u.Id == AbpSession.GetUserId(), cancellationToken)
                        : new User();

                    var organizationUnit = AbpSession.GetCurrentOUId() != null
                        ? await OrganisationUnits.FirstOrDefaultAsync(o => o.Id == AbpSession.GetCurrentOUId(), cancellationToken)
                        : new OrganizationUnit();

                    if (entityAuditInformation.Count > 0)
                    {
                        foreach (var item in entityAuditInformation)
                        {
                            if (Attribute.IsDefined(item.Entity.GetType(), typeof(AuditableAttribute)))
                            {
                                if (item.Changes != null && item.Changes.Any())
                                {
                                    var audit = new AuditTrail
                                    {
                                        TenantId = (int)AbpSession.TenantId,
                                        TableName = item.TableName,
                                        ChangedDate = DateTime.UtcNow,
                                        Changes = item.Changes,
                                        Operation = item.IsDeleteChanged ? "Delete" : item.State.ToString(),
                                        ChangedBy = user.UserName,
                                        OrganizationUnit = organizationUnit?.Id
                                    };

                                    _ = await AddAsync(audit, cancellationToken);
                                }
                            }
                        }

                        await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                    }
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<EntityAuditInformation> BeforeSaveChanges()
        {
            var entityAuditInformation = new List<EntityAuditInformation>();

            foreach (var entityEntry in ChangeTracker.Entries())
            {
                var entityType = entityEntry.Entity.GetType();

                //Skip entities that are not marked with [Auditable]
                if (!Attribute.IsDefined(entityType, typeof(AuditableAttribute)))
                {
                    continue;
                }

                bool isAdd = entityEntry.State == EntityState.Added;
                bool isDeleted = false;

                var changes = new List<AuditEntry>();

                foreach (var property in entityEntry.OriginalValues.Properties)
                {
                    var propertyEntry = entityEntry.Property(property.Name);
                    var propertyInfo = propertyEntry.Metadata.PropertyInfo;

                    //Only consider properties marked with [AuditedTrail]
                    if (propertyInfo != null && Attribute.IsDefined(propertyInfo, typeof(AuditedTrailAttribute)))
                    {
                        if ((isAdd && propertyEntry.CurrentValue != null) ||
                            (propertyEntry.IsModified && !object.Equals(propertyEntry.CurrentValue, propertyEntry.OriginalValue)))
                        {
                            changes.Add(new AuditEntry
                            {
                                TenantId = (int)AbpSession.TenantId,
                                FieldName = propertyEntry.Metadata.Name,
                                NewValue = propertyEntry.CurrentValue?.ToString(),
                                OldValue = isAdd ? null : propertyEntry.OriginalValue?.ToString()
                            });
                        }
                    }
                }

                // Handles entity deletion if applicable
                if (entityEntry.State == EntityState.Deleted)
                {
                    var idProperty = entityEntry.Property("Id");
                    if (idProperty != null)
                    {
                        changes.Add(new AuditEntry
                        {
                            TenantId = (int)AbpSession.TenantId,
                            FieldName = idProperty.Metadata.Name,
                            NewValue = null,
                            OldValue = idProperty.OriginalValue?.ToString()
                        });
                        isDeleted = true;
                    }
                }

                entityAuditInformation.Add(new EntityAuditInformation
                {
                    Entity = entityEntry.Entity,
                    TableName = entityEntry.Metadata.GetTableName() ?? entityType.Name,
                    State = entityEntry.State,
                    IsDeleteChanged = isDeleted,
                    Changes = changes
                });
            }
            return entityAuditInformation;
        }
        #endregion
    }
}