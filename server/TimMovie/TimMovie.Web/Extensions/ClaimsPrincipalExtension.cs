using System.Security.Claims;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.Web.Extensions;

public static class ClaimsPrincipalExtension
{
    public static Guid? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        ArgumentValidator.ThrowExceptionIfNull(claimsPrincipal, nameof(claimsPrincipal));
        var idInStr = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            
        if (idInStr is null)
        {
            return null;
        }
        
        return Guid.Parse(idInStr);
    }
    
    public static bool HasRoleClaim(this ClaimsPrincipal claimsPrincipal, string nameRole)
    {
        ArgumentValidator.ThrowExceptionIfNull(claimsPrincipal, nameof(claimsPrincipal));
        ArgumentValidator.ThrowExceptionIfNull(nameRole, nameof(nameRole));

        return claimsPrincipal.HasClaim(ClaimTypes.Role, nameRole);
    }
}