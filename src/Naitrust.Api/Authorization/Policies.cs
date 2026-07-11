namespace Naitrust.Api.Authorization;

public static class Policies
{
    public const string RequireUser = "RequireUser";
    public const string RequireAdmin = "RequireAdmin";
    public const string RequireSuperAdmin = "RequireSuperAdmin";
    public const string RequireBusinessOwner = "RequireBusinessOwner";
    public const string RequireBusinessAdmin = "RequireBusinessAdmin";
    public const string RequireTransactionParty = "RequireTransactionParty";
}
