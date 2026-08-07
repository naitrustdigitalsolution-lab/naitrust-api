using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Infrastructure.Context;

public class NaitrustDbContext : IdentityDbContext<NaitrustUser, NaitrustRole, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, NaitrustRoleClaim, IdentityUserToken<Guid>>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private bool _isSavingAudit;

    public NaitrustDbContext(DbContextOptions<NaitrustDbContext> options, IHttpContextAccessor? httpContextAccessor = null)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Core (Users is provided by IdentityDbContext)
    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<BusinessMember> BusinessMembers => Set<BusinessMember>();
    public DbSet<Party> Parties => Set<Party>();

    // Verification
    public DbSet<VerificationRequest> VerificationRequests => Set<VerificationRequest>();
    public DbSet<VerificationStep> VerificationSteps => Set<VerificationStep>();
    public DbSet<VerificationDocument> VerificationDocuments => Set<VerificationDocument>();
    public DbSet<FaceMatchResult> FaceMatchResults => Set<FaceMatchResult>();
    public DbSet<OwnershipCheck> OwnershipChecks => Set<OwnershipCheck>();
    public DbSet<VerificationProviderEvent> VerificationProviderEvents => Set<VerificationProviderEvent>();

    // Invitations
    public DbSet<DealInvitation> DealInvitations => Set<DealInvitation>();

    // Deals
    public DbSet<Deal> Deals => Set<Deal>();
    public DbSet<TransactionType> TransactionTypes => Set<TransactionType>();
    public DbSet<DealParty> DealParties => Set<DealParty>();
    public DbSet<Agreement> Agreements => Set<Agreement>();
    public DbSet<Milestone> Milestones => Set<Milestone>();
    public DbSet<EvidenceFile> EvidenceFiles => Set<EvidenceFile>();
    public DbSet<DealMessage> DealMessages => Set<DealMessage>();
    public DbSet<DealTermination> DealTerminations => Set<DealTermination>();

    // Payments
    public DbSet<VirtualAccount> VirtualAccounts => Set<VirtualAccount>();
    public DbSet<PaymentPartnerEvent> PaymentPartnerEvents => Set<PaymentPartnerEvent>();
    public DbSet<LedgerEntry> LedgerEntries => Set<LedgerEntry>();
    public DbSet<PaymentInstruction> PaymentInstructions => Set<PaymentInstruction>();
    public DbSet<ReleaseRequest> ReleaseRequests => Set<ReleaseRequest>();
    public DbSet<PayoutAccount> PayoutAccounts => Set<PayoutAccount>();

    // Disputes
    public DbSet<Dispute> Disputes => Set<Dispute>();
    public DbSet<DisputeMessage> DisputeMessages => Set<DisputeMessage>();
    public DbSet<DisputeEvidence> DisputeEvidence => Set<DisputeEvidence>();

    // Negotiations
    public DbSet<Negotiation> Negotiations => Set<Negotiation>();
    public DbSet<NegotiationProposal> NegotiationProposals => Set<NegotiationProposal>();

    // Reputation
    public DbSet<ReputationProfile> ReputationProfiles => Set<ReputationProfile>();
    public DbSet<Review> Reviews => Set<Review>();

    // Notifications
    public DbSet<Notification> Notifications => Set<Notification>();

    // AI
    public DbSet<AiAssessment> AiAssessments => Set<AiAssessment>();
    public DbSet<AiFeedback> AiFeedbacks => Set<AiFeedback>();
    public DbSet<AiPromptVersion> AiPromptVersions => Set<AiPromptVersion>();
    public DbSet<VectorDocument> VectorDocuments => Set<VectorDocument>();

    // Public
    public DbSet<WaitlistEntry> WaitlistEntries => Set<WaitlistEntry>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<NewsletterSubscriber> NewsletterSubscribers => Set<NewsletterSubscriber>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<ReportedConcern> ReportedConcerns => Set<ReportedConcern>();

    // Roles
    public DbSet<NaitrustRoleClaim> NaitrustRoleClaims => Set<NaitrustRoleClaim>();

    // Infrastructure
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<IdempotencyKey> IdempotencyKeys => Set<IdempotencyKey>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseEntity).Assembly);

        // Soft-delete query filters
        modelBuilder.Entity<NaitrustUser>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Business>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Deal>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<VirtualAccount>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Dispute>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Party>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Notification>().HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<NaitrustRoleClaim>().ToTable("RoleClaims");

        // Concurrency tokens on financial/critical entities
        modelBuilder.Entity<Deal>().Property<uint>("xmin").IsRowVersion();
        modelBuilder.Entity<LedgerEntry>().Property<uint>("xmin").IsRowVersion();
        modelBuilder.Entity<VirtualAccount>().Property<uint>("xmin").IsRowVersion();
        modelBuilder.Entity<ReleaseRequest>().Property<uint>("xmin").IsRowVersion();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_isSavingAudit)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        // Auto-set timestamps on BaseEntity-derived entities
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    if (entry.Entity.Id == Guid.Empty)
                    {
                        entry.Entity.Id = Guid.NewGuid();
                    }

                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        // Auto-set timestamps on NaitrustUser (not a BaseEntity)
        foreach (var entry in ChangeTracker.Entries<NaitrustUser>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        var auditEntries = OnBeforeSaveChanges();
        var result = await base.SaveChangesAsync(cancellationToken);

        if (auditEntries.Count > 0)
        {
            _isSavingAudit = true;
            try
            {
                await OnAfterSaveChangesAsync(auditEntries, cancellationToken);
            }
            finally
            {
                _isSavingAudit = false;
            }
        }

        return result;
    }

    private List<AuditEntry> OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
            {
                continue;
            }

            var auditEntry = new AuditEntry(entry)
            {
                EntityType = entry.Entity.GetType().Name,
                Action = entry.State.ToString(),
                UserId = GetUserId(),
                IpAddress = GetIpAddress()
            };

            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    auditEntry.TemporaryProperties.Add(property);
                    continue;
                }

                string propertyName = property.Metadata.Name;

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;
                    case EntityState.Deleted:
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }

            auditEntries.Add(auditEntry);
        }

        return auditEntries;
    }

    private async Task OnAfterSaveChangesAsync(List<AuditEntry> auditEntries, CancellationToken cancellationToken)
    {
        foreach (var auditEntry in auditEntries)
        {
            foreach (var prop in auditEntry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    auditEntry.EntityId = (Guid)prop.CurrentValue!;
                }
                else
                {
                    auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }

            AuditLogs.Add(new AuditLog
            {
                Id = Guid.NewGuid(),
                ActorUserId = auditEntry.UserId != null ? Guid.Parse(auditEntry.UserId) : null,
                Action = auditEntry.Action,
                EntityType = auditEntry.EntityType,
                EntityId = auditEntry.EntityId,
                Before = auditEntry.OldValues.Count > 0 ? JsonSerializer.Serialize(auditEntry.OldValues) : null,
                After = auditEntry.NewValues.Count > 0 ? JsonSerializer.Serialize(auditEntry.NewValues) : null,
                IpAddress = auditEntry.IpAddress,
                CreatedAt = DateTime.UtcNow
            });
        }

        await base.SaveChangesAsync(cancellationToken);
    }

    private string? GetUserId()
    {
        return _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    private string? GetIpAddress()
    {
        return _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
    }

    private sealed class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string EntityType { get; set; } = default!;
        public string Action { get; set; } = default!;
        public string? UserId { get; set; }
        public string? IpAddress { get; set; }
        public Guid EntityId { get; set; }
        public Dictionary<string, object?> OldValues { get; } = new();
        public Dictionary<string, object?> NewValues { get; } = new();
        public List<PropertyEntry> TemporaryProperties { get; } = new();
    }
}
