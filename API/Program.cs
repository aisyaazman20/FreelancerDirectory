using Infrastructure.Data;
using Core.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args); //entrypoint

//REGISTER SERVICES

builder.Services.AddDbContext<FreelancerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));// register DbContext using DI container     

builder.Services.AddScoped<IFreelancerRepository, FreelancerRepository>();//register repository
    
builder.Services.AddAutoMapper(typeof(Program));//register automapper

builder.Services.AddControllers(); //register controller

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build(); //build app
app.UseHttpsRedirection(); //redirect http reqs to https
app.UseMiddleware<ExceptionMiddleware>(); //register errorhandler
app.MapControllers(); //enables [ApiController] and [Route] 

app.Run(); 

