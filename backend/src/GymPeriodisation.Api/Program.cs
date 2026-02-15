using GymPeriodisation.Api.Middleware;
using GymPeriodisation.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GymPeriodisation API v1");
        c.RoutePrefix = string.Empty; // Swagger at root: https://localhost:5001/
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.Run();
