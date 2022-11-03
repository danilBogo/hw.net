using SupportChat.WebHost.SignalR;

namespace SupportChat.WebHost.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureStartup(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseRouting();
        app.UseCors(build =>
            build.WithOrigins(app.Configuration.GetRequiredSection("ClientHost").Value)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );
        app.UseEndpoints(endpoints => { endpoints.MapHub<ChatHub>("/chatSignalR"); });
        return app;
    }
}