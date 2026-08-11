namespace Naitrust.Domain.Models.Dtos.Responses.Counterparties;

public record CounterpartyProfileResponse(
    Guid Id,
    string? NaitrustId,
    string Name,
    string? BusinessName,
    string? Email,
    string? Phone,
    string? Address,
    string? Notes,
    string AvatarInitials,
    string Relation,
    bool IdentityVerified,
    bool BusinessVerified,
    string MemberSince,
    int CompletedDealsCount,
    bool HasPriorTransactionWithYou,
    double? AverageResponseTimeHours,
    int ResolvedDisputesCount,
    double? RatingAverage,
    bool IsFavourite,
    bool IsBlocked);
