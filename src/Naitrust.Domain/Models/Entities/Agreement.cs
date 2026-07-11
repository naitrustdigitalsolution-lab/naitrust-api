namespace Naitrust.Domain.Models.Entities;

public class Agreement : BaseEntity
{
    public Guid TransactionId { get; set; }
    public int Version { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public string? DeliveryConditions { get; set; }
    public string? ReleaseConditions { get; set; }
    public string? ProofRequirements { get; set; }
    public string? DisputeRules { get; set; }
    public int? AutoConfirmWindowHours { get; set; }
    public DateTime? DeliveryDueAt { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime? BuyerAcceptedAt { get; set; }
    public DateTime? SellerAcceptedAt { get; set; }
    public DateTime? FrozenAt { get; set; }
}
