global using Demo.Data;
global using Microsoft.EntityFrameworkCore;
using Demo.Converter;
using Demo.DTOs;
using Demo.DTOs.Response;
using Demo.Services;
using Demo.Services.Impl;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<StudentService, StudentServiceImpl>();
// builder.Services.AddSingleton(typeof(ResponseObject<>));
// builder.Services.AddTransient<ResponseObject<StudentDTO>>();
// builder.Services.AddTransient<ResponseObject<List<StudentDTO>>>();
builder.Services.AddSingleton<StudentConverter>();
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
