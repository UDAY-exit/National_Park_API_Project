using Microsoft.EntityFrameworkCore;
using NationalPark_API.Data;

var builder = WebApplication.CreateBuilder(args);

string cs = builder.Configuration.GetConnectionString("conStr");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(cs));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
