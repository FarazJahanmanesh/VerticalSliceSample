using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sample.Api.Data;
using Sample.Api.Features.Books;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("TestDataBase"));
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter();

app.UseHttpsRedirection();

app.Run();

