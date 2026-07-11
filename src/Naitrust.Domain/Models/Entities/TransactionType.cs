using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Domain.Models.Enums.Verification;

namespace Naitrust.Domain.Models.Entities;

public class TransactionType : BaseEntity
{
    public string Key { get; set; } = default!;
    public string Name { get; set; } = default!;
    public VerificationLevel RequiredVerificationLevel { get; set; }
    public string? EvidenceRequirements { get; set; }
    public ReleaseMode ReleaseMode { get; set; }
    public string? DisputeRules { get; set; }
    public string? FeeModel { get; set; }
    public int AutoConfirmWindowHours { get; set; }
}
