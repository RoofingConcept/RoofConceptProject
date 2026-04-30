namespace RoofingConcept.Business.Results;

public class JwtServiceResult : ServiceResult
{
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAtUtc { get; init; }
}
