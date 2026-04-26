using TicketingSystem.DataAccess.Extensions;
using TicketingSystem.Application.Extensions;
using TicketingSystem.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.RegisterDomain();
builder.Services.RegisterDataAccess(builder.Configuration);
builder.Services.RegisterApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "TicketingSystem API");
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
