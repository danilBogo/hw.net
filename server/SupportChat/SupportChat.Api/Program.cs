using SupportChat.BackgroundService;
using SupportChat.Core.Interfaces;
using SupportChat.Core.Services;
using SupportChat.Infrastructure;
using SupportChat.Infrastructure.Database;
using SupportChat.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// builder.Services.AddHostedService<ChatBackgroundService>();
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(MyAllowSpecificOrigins,
//         policy =>
//         {
//             policy.WithOrigins(MyAllowSpecificOrigins)
//                 .AllowAnyOrigin();
//         });
// });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();
app.UseCors(build => build.AllowAnyOrigin());

app.Run();