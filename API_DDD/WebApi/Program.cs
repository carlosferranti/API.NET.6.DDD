using AutoMapper;
using Dominio.Interfaces;
using Dominio.Interfaces.Generics;
using Dominio.Interfaces.InterfaceServico;
using Dominio.Servicos;
using Entities.Entities;
using Infraestrutura.Config;
using Infraestrutura.Repositorio.Generic;
using Infraestrutura.Repositorio.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using WebApi.Models;
using WebApi.Token;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ConfigServices
builder.Services.AddDbContext<ContextoBase>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ContextoBase>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// interface e repositorio
builder.Services.AddSingleton(typeof(IGeneric<>), typeof(RepositorioGeneric<>));
builder.Services.AddSingleton<IMensagem, RepositorioMensagem>();

// serviço domínio
builder.Services.AddSingleton<IServicoMensagem, ServicoMensagem>();

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "Teste.Security.Bearer",
            ValidAudience = "Teste.Security.Bearer",
            IssuerSigningKey = JwtSecurityKey.Create("Secret_Key-0987654321")
        };

        option.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Autenticação falha: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validado: " + context.SecurityToken);
                return Task.CompletedTask;
            }
        };

    });

var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.CreateMap<MensagemViewModel, Mensagem>();
    cfg.CreateMap<Mensagem, MensagemViewModel>();
});

IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//var urlDev = "https://dominio.com.br";
//var urlHomolog = "https://dominio.com.br";
//var urlProd = "https://dominio.com.br";

//app.UseCors(b => b.WithOrigins(urlDev, urlHomolog, urlProd));

var devClient = "http://localhost:4200";
app.UseCors(t => t
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader().WithOrigins(devClient));

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerUI();

app.Run();
