using AutoFilterer.Swagger;
using FilterAPI.Endpoints;
using FilterAPI.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.UseAutoFiltererParameters();
});

builder.Services.AddDbContext<FilterDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("FilterDb"));
    opt.UseSnakeCaseNamingConvention();
    opt.EnableSensitiveDataLogging();
    opt.LogTo(Console.WriteLine, LogLevel.Information);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.RegisterEndpoints();

app.Run();
