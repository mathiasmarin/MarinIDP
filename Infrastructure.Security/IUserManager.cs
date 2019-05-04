using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security
{
    public interface IUserManager
    {
        Task<IdentityResult> CreateUser(string firstName, string lastName, string email, string password);
        Task<MarinAppUser> FindByEmailAsync(string email);
        Task<bool> IsEmailConfirmedAsync(MarinAppUser user);
        string CreateEmailConfirmationToken(MarinAppUser user);
        string CreatePasswordResetToken(MarinAppUser user);
        Task<IdentityResult> ConfirmEmailAsync(MarinAppUser user, string code);
        Task<IdentityResult> ResetPasswordAsync(MarinAppUser user, string code, string newPassword);
        Task<IList<Claim>> GetClaimsForUser(MarinAppUser user);
    }
}