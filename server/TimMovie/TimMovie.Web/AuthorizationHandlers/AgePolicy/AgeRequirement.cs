using Microsoft.AspNetCore.Authorization;

namespace TimMovie.Web.AuthorizationHandlers.AgePolicy;

public class AgeRequirement : IAuthorizationRequirement
{
    protected internal int Age { get; set; }
     
    public AgeRequirement(int age)
    {
        Age = age;
    }
}