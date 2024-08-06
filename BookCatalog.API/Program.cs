using BookCatalog.API.Data;
using BookCatalog.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IDataStore, InMemoryDataStore>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");
app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Map("/error", (HttpContext httpContext) =>
{
    return Results.Problem();
});

app.Run();
