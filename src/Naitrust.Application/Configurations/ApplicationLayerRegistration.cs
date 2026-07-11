using Microsoft.Extensions.DependencyInjection;
using Naitrust.Application.Services.Implementations.Admin;
using Naitrust.Application.Services.Implementations.Ai;
using Naitrust.Application.Services.Implementations.Auth;
using Naitrust.Application.Services.Implementations.Businesses;
using Naitrust.Application.Services.Implementations.Disputes;
using Naitrust.Application.Services.Implementations.Evidence;
using Naitrust.Application.Services.Implementations.Notifications;
using Naitrust.Application.Services.Implementations.Parties;
using Naitrust.Application.Services.Implementations.Payments;
using Naitrust.Application.Services.Implementations.Public;
using Naitrust.Application.Services.Implementations.Reputation;
using Naitrust.Application.Services.Implementations.Transactions;
using Naitrust.Application.Services.Implementations.Users;
using Naitrust.Application.Services.Implementations.Verification;
using Naitrust.Application.Services.Interfaces;

namespace Naitrust.Application.Configurations;

public static class ApplicationLayerRegistration
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // Auth
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();

        // Users
        services.AddScoped<IUserService, UserService>();

        // Businesses
        services.AddScoped<IBusinessService, BusinessService>();

        // Transactions
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ITransactionOrchestrator, TransactionOrchestrator>();
        services.AddScoped<IPartyService, PartyService>();

        // Payments
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ILedgerService, LedgerService>();

        // Verification
        services.AddScoped<IVerificationService, VerificationService>();

        // Disputes
        services.AddScoped<IDisputeService, DisputeService>();

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

        // Public
        services.AddScoped<IPublicFormService, PublicFormService>();

        return services;
    }
}
