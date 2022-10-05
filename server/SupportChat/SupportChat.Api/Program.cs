using MassTransit;
using SupportChat.Api.Consumers;
using SupportChat.Api.SignalR;
using SupportChat.Core.Interfaces;
using SupportChat.Core.Services;
using SupportChat.Infrastructure;
using SupportChat.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSignalR();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<MessageConsumer>();
    config.UsingInMemory((context, configurator) =>
    {
        configurator.Host();
        configurator.ReceiveEndpoint(new TemporaryEndpointDefinition(), x =>
        {
            x.ConfigureConsumer<MessageConsumer>(context);
        });
        configurator.ConfigureEndpoints(context);
    });
});
builder.Services.AddScoped<MessageConsumer>();
// builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();
app.UseCors(build =>
    build.WithOrigins(app.Configuration.GetRequiredSection("ClientHost").Value)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

app.UseEndpoints(endpoints => { endpoints.MapHub<ChatHub>("/chatSignalR"); });

app.Run();