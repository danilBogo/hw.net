using SupportChat.Domain;
using SupportChat.WebHost.Extensions;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddServices(builder.Configuration);
builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddMinio(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);

var app = builder.Build();

app.ConfigureStartup();

app.Run();