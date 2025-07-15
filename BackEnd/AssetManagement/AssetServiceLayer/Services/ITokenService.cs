using AssetDAL.Models;

namespace AssetServiceLayer.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
