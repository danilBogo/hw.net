using SupportChat.Domain;
using SupportChat.WebHost.Extensions;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddServices();
builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

app.ConfigureStartup();

app.Run();