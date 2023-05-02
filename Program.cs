global using Demo.Data;
global using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Demo;
using Demo.Configuration;
using Demo.Converter;
using Demo.DTOs;
using Demo.DTOs.Response;
using Demo.Services;
using Demo.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<StudentService, StudentServiceImpl>();
builder.Services.AddTransient<AuthService, AuthServiceImpl>();
// builder.Services.AddSingleton(typeof(ResponseObject<>));
// builder.Services.AddTransient<ResponseObject<StudentDTO>>();
// builder.Services.AddTransient<ResponseObject<List<StudentDTO>>>();
builder.Services.AddSingleton<StudentConverter>();
builder.Services.AddSingleton<SecurityConfiguration>();
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o => {
    o.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/security/login", [AllowAnonymous] (Student user) => {
    if (user.Username.Equals("usernameTest")  && user.Password.Equals("testPassword")) {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);
        return Results.Ok(stringToken);
    }

    return Results.Unauthorized();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
