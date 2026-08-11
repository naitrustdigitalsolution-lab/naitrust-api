namespace Naitrust.Domain.Models.Entities;

/// <summary>Stores per-user favourite/blocked flags for counterparties derived from deal history.</summary>
public class UserCounterpartyPreference : BaseEntity
{
    public Guid OwnerUserId { get; set; }
    public Guid CounterpartyUserId { get; set; }
    public bool IsFavourite { get; set; }
    public bool IsBlocked { get; set; }
}
