using DataAccessLayer.Models;

namespace WebApi.Services.Interfaces
{
    public interface ITokenService
    {
        string Generate(ApplicationUser user);
    }
}