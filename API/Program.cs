using Infrastructure.Data;
using Core.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

//

var builder = WebApplication.CreateBuilder(args);

//REGISTER SERVICES

// register DbContext
builder.Services.AddDbContext<FreelancerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//register repository
builder.Services.AddScoped<IFreelancerRepository, FreelancerRepository>();

//register automapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Freelancer Directory API", Version = "v1" });
});

var app = builder.Build();

app.MapControllers(); //enables [ApiController] routes

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}


app.UseHttpsRedirection();

app.Run();

