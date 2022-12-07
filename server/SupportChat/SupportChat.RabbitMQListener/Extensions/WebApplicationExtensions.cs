using SupportChat.RabbitMQListener.SignalR;

namespace SupportChat.RabbitMQListener.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureStartup(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseRouting();
        app.UseCors(build =>
            build.WithOrigins(app.Configuration.GetRequiredSection("ClientHost").Value)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );
        app.UseEndpoints(endpoints => { endpoints.MapHub<FileUploadedHub>("/fileUploadedSignalR"); });
        return app;
    }
}