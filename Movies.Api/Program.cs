using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Movies.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<MoviesDbContext>(options => options.UseInMemoryDatabase(databaseName: "Movies.Api"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// DBInit
var provider = builder.Services.BuildServiceProvider();
DatabaseSeeder.Initialize(provider);

app.Run();