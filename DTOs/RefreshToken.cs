namespace DTOs.Models
{
    public sealed record RefreshToken(string UserName, DateTime CreateDate, DateTime ExpireDate);
}
