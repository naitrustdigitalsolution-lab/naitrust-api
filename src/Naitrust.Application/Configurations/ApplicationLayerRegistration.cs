using Microsoft.Extensions.DependencyInjection;
using Naitrust.Application.ExternalServices;
using Naitrust.Application.ExternalServices.CacheServices;
using Naitrust.Application.ExternalServices.Communication;
using Naitrust.Application.ExternalServices.Storage;
using Naitrust.Application.ExternalServices.QoreId;
using Naitrust.Application.ExternalServices.Communication.Resend;
using Naitrust.Application.ExternalServices.Anchor;
using Naitrust.Application.ExternalServices.Providus;
using Naitrust.Application.Services.Implementations.Admin;
using Naitrust.Application.Services.Implementations.Ai;
using Naitrust.Application.Services.Implementations.Auth;
using Naitrust.Application.Services.Implementations.Agreements;
using Naitrust.Application.Services.Implementations.Businesses;
using Naitrust.Application.Services.Implementations.Disputes;
using Naitrust.Application.Services.Implementations.Negotiations;
using Naitrust.Application.Services.Implementations.Invitations;
using Naitrust.Application.Services.Implementations.Evidence;
using Naitrust.Application.Services.Implementations.Notifications;
using Naitrust.Application.Services.Implementations.Parties;
using Naitrust.Application.Services.Implementations.Payments;
using Naitrust.Application.Services.Implementations.Public;
using Naitrust.Application.Services.Implementations.Reputation;
using Naitrust.Application.Services.Implementations.Roles;
using Naitrust.Application.Services.Implementations.Security;
using Naitrust.Application.Services.Implementations.Transactions;
using Naitrust.Application.Services.Implementations.Users;
using Naitrust.Application.Services.Implementations.Verification;
using Naitrust.Application.Services.Implementations.InstantTransfers;
using Naitrust.Application.Services.Implementations.Beneficiaries;
using Naitrust.Application.Services.Implementations.PaymentRequests;
using Naitrust.Application.Services.Implementations.Counterparties;
using Naitrust.Application.Services.Implementations.TrustProfile;
using Naitrust.Application.Services.Interfaces;

namespace Naitrust.Application.Configurations;

public static class ApplicationLayerRegistration
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // External services
        services.AddMemoryCache();
        services.AddScoped<ICacheService, InMemoryCacheService>();

        // Email: provider → factory → service
        services.AddHttpClient<ResendEmailProvider>();
        services.AddScoped<IEmailProviderFactory, EmailProviderFactory>();
        services.AddScoped<IEmailService, EmailService>();

        // Payment partners: provider → factory
        services.AddHttpClient<AnchorPaymentPartner>();
        services.AddScoped<IPaymentPartnerFactory, PaymentPartnerFactory>();

        // Storage
        services.AddScoped<IStorageService, S3StorageService>();

        // QoreID identity verification
        services.AddHttpClient<QoreIdVerificationProvider>();
        services.AddScoped<IVerificationProvider, QoreIdVerificationProvider>();

        // Auth
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();

        // Users
        services.AddScoped<IUserService, UserService>();

        // Businesses
        services.AddScoped<IBusinessService, BusinessService>();

        // Agreements
        services.AddScoped<IAgreementService, AgreementService>();

        // Invitations
        services.AddScoped<IInvitationService, InvitationService>();

        // Deals
        services.AddScoped<IDealService, DealService>();
        services.AddScoped<IDealOrchestrator, DealOrchestrator>();
        services.AddScoped<IDealSubResourceService, DealSubResourceService>();
        services.AddScoped<IPartyService, PartyService>();

        // Payments
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ILedgerService, LedgerService>();

        // Roles
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IRoleClaimService, RoleClaimService>();

        // Verification
        services.AddScoped<IVerificationService, VerificationService>();

        // Disputes
        services.AddScoped<IDisputeService, DisputeService>();

        // Negotiations
        services.AddScoped<INegotiationService, NegotiationService>();

        // Evidence
        services.AddScoped<IEvidenceService, EvidenceService>();

        // Reputation
        services.AddScoped<IReputationService, ReputationService>();

        // Notifications
        services.AddScoped<INotificationService, NotificationService>();

        // Admin
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IAuditLogService, AuditLogService>();

        // AI
        services.AddScoped<IAiService, AiIntelligenceService>();

        // Wallet
        services.AddScoped<IWalletService, WalletService>();

        // Security
        services.AddScoped<ISecurityService, SecurityService>();

        // Public
        services.AddScoped<IPublicFormService, PublicFormService>();

        // Instant Transfers
        services.AddScoped<IInstantTransferService, InstantTransferService>();

        // Beneficiaries
        services.AddScoped<IBeneficiaryService, BeneficiaryService>();

        // Payment Requests
        services.AddScoped<IPaymentRequestService, PaymentRequestService>();

        // Counterparties
        services.AddScoped<ICounterpartyService, CounterpartyService>();

        // Trust Profile
        services.AddScoped<ITrustProfileService, TrustProfileService>();

        return services;
    }
}
