using SupportChat.Domain;
using SupportChat.RabbitMQListener.Extensions;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddServices(builder.Configuration);
builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddMongoDb(builder.Configuration);

var app = builder.Build();

app.Run();