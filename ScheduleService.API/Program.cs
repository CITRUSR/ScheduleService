using ScheduleService.Application;
using ScheduleService.Infrastructure;
using ScheduleService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigratrations();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await app.RunAsync();
