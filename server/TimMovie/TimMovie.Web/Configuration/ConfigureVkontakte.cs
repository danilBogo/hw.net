using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace TimMovie.Web.Configuration;

public static class ConfigureVkontakte
{
    public static AuthenticationBuilder AddVkontakte(this AuthenticationBuilder builder, string clientId, string appSecret)
    {
        builder.AddOAuth("VK", "VKontakte", config =>
        {
            config.CorrelationCookie.SameSite = SameSiteMode.Lax;
            config.ClientId = clientId;
            config.ClientSecret = appSecret;
            config.ClaimsIssuer = "VKontakte";
            config.CallbackPath = new PathString("/signin-vkontakte-token");
            config.AuthorizationEndpoint = "https://oauth.vk.com/authorize";
            config.TokenEndpoint = "https://oauth.vk.com/access_token";
            config.Scope.Add("email");
            config.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
            config.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            config.SaveTokens = true;
            config.Events = new OAuthEvents
            {
                OnCreatingTicket = context =>
                {
                    context.RunClaimActions(context.TokenResponse.Response.RootElement);
                    return Task.CompletedTask;
                }
            };
        });

        return builder;
    }
}