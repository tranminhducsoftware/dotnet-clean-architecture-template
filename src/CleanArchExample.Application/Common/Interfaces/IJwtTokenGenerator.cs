namespace CleanArchExample.Application.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string username, string userId);

    }
}