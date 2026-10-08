using Khabarov_Artem_AS2304.Data; //БД
using Microsoft.EntityFrameworkCore; //FrameworkCore 
using Khabarov_Artem_AS2304.Models; // модели
using Microsoft.AspNetCore.Authentication.JwtBearer; //аутентификация
using Microsoft.IdentityModel.Tokens; // токены
using System.Text; 

var builder = WebApplication.CreateBuilder(args); //для веба

builder.Services.AddControllers(); 
var secretKey = "SuperSecretKeyARTEMKA1175WOAH67SIXSEVEEEEENUWUOWO"; // Ключ шифрования токена (32+)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //аутентификация
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,    
            ValidateAudience = false,  
            ValidateLifetime = false,  
            ValidateIssuerSigningKey = true, //чей токен
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // проверяет токен
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddDbContext<LabContext>(options =>
    options.UseInMemoryDatabase("LabVirtualDb")); // потом поменяю, здесь должна быть БД

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();  
app.UseAuthorization();  
app.MapControllers();


app.Run();
/*
http://localhost:5065/api/auth/get-token?role=Admin
ОШИБКА 401 UNAUTHORIZED  = не знаем кто это
ОШИБКА 403 FORBIDDEN  = знаем, вы слишком многого хотите.
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 9.0.0
Install-Package Microsoft.EntityFrameworkCore.InMemory -Version 9.0.0
 */