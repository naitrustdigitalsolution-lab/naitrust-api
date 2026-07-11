using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Payments;

namespace Naitrust.Domain.Models.Entities;

public class PaymentInstruction : BaseEntity
{
    public Guid TransactionId { get; set; }
    public Guid VirtualAccountId { get; set; }
    public PaymentInstructionType InstructionType { get; set; }
    public PaymentPartnerId Partner { get; set; }
    public string IdempotencyKey { get; set; } = default!;
    public string? SignedPayloadHash { get; set; }
    public PaymentInstructionStatus Status { get; set; }
    public string? PartnerReference { get; set; }
    public string? PartnerResponse { get; set; }
}
