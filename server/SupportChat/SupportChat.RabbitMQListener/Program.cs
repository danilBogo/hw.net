using SupportChat.Domain;
using SupportChat.RabbitMQListener.Extensions;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));

app.Run();