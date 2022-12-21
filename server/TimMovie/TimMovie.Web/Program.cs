using Autofac;
using Autofac.Extensions.DependencyInjection;
using TimMovie.Core;
using TimMovie.Infrastructure;
using TimMovie.Web.Configuration;
using TimMovie.Web.Extensions;
using TimMovie.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.ConfigureServices(builder.Configuration,builder.Environment);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<CoreModule>();
    containerBuilder.RegisterModule(new InfrastructureModule(builder.Configuration));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseFileService();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// app.UseBannedUserService();

// app.UseUserStatusUpdateService();
// app.UseUserStatusDeleteService();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MainPage}/{action=MainPage}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");
});

app.Run();
