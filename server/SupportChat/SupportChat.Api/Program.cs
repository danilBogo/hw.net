using SupportChat.BackgroundService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<ChatBackgroundService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();