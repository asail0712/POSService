﻿using AutoMapper;
using Common.Filter;
using Common.Profiles;
using DataAccess;
using DataAccess.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Repository;
using Repository.Interface;
using Service;
using Service.Interface;

using XPlan.Utility;
using XPlan.Utility.Databases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/********************************************
 * 加上 Filter 
 * ******************************************/
builder.Services.AddGlobalExceptionHandling();
//builder.Services.AddJwtPathAuthFilter();

/********************************************
 * 加上 Settings
 * ******************************************/
builder.Services.AddCacheSettings(builder.Configuration);
builder.Services.AddJWTSecurity();

/********************************************
 * 加上Database Settings
 * ******************************************/
builder.Services.InitialMongoDB(builder.Configuration);

/********************************************
 * 註冊AutoMapper
 * ******************************************/
builder.Services.AddAutoMapperProfiles(LoggerFactory.Create(builder =>
{
    builder.AddConsole(); // 或其他你需要的設定
}));

/********************************************
 * 註冊 JWT 認證
 * ******************************************/
builder.Services.AddJwtAuthentication(new JwtOptions() 
{
    ValidateIssuer              = true,
    ValidateAudience            = false,
    ValidateLifetime            = false,
    ValidateIssuerSigningKey    = true,
    Issuer                      = builder.Configuration["Jwt:Issuer"]!,
    Audience                    = builder.Configuration["Jwt:Audience"]!,
    Secret                      = builder.Configuration["Jwt:Secret"]!
});

/********************************************
 * 加上Data Access
 * ******************************************/
builder.Services.AddScoped<IDishItemDataAccess, DishItemDataAccess>();
builder.Services.AddScoped<IManagementDataAccess, ManagementDataAccess>();
builder.Services.AddScoped<IOrderDataAccess, OrderDataAccess>();
builder.Services.AddScoped<IProductDataAccess, ProductDataAccess>();
builder.Services.AddScoped<ISalesDataAccess, SalesDataAccess>();

/********************************************
 * 加上Repository
 * ******************************************/
builder.Services.AddScoped<IDishItemRepository, DishItemRepository>();
builder.Services.AddScoped<IManagementRepository, ManagementRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>();

/********************************************
 * 加上Services
 * ******************************************/
builder.Services.AddScoped<IDishItemService, DishItemService>();
builder.Services.AddScoped<IManagementService, ManagementService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISalesService, SalesService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/********************************************
 * 加上Swagger 註解
 * ******************************************/
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<ControllerAddSummaryFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();    
}

app.UseHttpsRedirection();

// 由中介層加上JSON驗證
app.UseJwtPathAuth();

app.MapControllers();

app.Run();
