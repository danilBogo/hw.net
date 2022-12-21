using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TimMovie.Web.AuthorizationHandlers.AgePolicy;

public class AgeHandler : AuthorizationHandler<AgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth)) 
            return Task.CompletedTask;
        {
            var now = DateOnly.FromDateTime(DateTime.Today);
            DateOnly userBirthDay;
            if (!DateOnly.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth)?.Value,
                    out userBirthDay))
                return Task.CompletedTask;
            if (now.Year - userBirthDay.Year >= requirement.Age)
            {
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}