using Microsoft.EntityFrameworkCore;
using NationalPark_API.Data;
using NationalPark_API.Repository;
using NationalPark_API.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

string cs = builder.Configuration.GetConnectionString("conStr");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(cs));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

//Register Swagger

builder.Services.AddEndpointsApiExplorer();//this line add
builder.Services.AddSwaggerGen();//This line add

//builder.Services.AddOpenApi();//Comment this line

builder.Services.AddAutoMapper(cfg => { },AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<INationalParkRepository, INationalParkRepository>();
builder.Services.AddScoped<ITrailRepository, TrailRepository>();

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
