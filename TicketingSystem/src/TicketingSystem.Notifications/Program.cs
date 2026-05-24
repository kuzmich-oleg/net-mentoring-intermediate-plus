using TicketingSystem.Notifications.Extensions;
using TicketingSystem.Notifications.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.RegisterInfrastructure();

builder.Services.AddHostedService<TicketingNotificationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();
