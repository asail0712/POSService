using AutoMapper;
using Common.Profiles;
using DataAccess;
using DataAccess.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Repository;
using Repository.Interface;
using Service;
using Service.Interface;

using XPlan.Database;
using XPlan.Interface;
using XPlan.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/********************************************
 * 加上Exception Filter 
 * ******************************************/
builder.Services.AddGlobalExceptionHandling();

/********************************************
 * 加上Cache Settings
 * ******************************************/
builder.Services.AddCacheSettings(builder.Configuration);

/********************************************
 * 加上Database Settings
 * ******************************************/
builder.Services.InitialMongoDB(builder.Configuration);
builder.Services.AddSingleton<IMongoClient>((sp) =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

/********************************************
 * 註冊AutoMapper
 * ******************************************/
builder.Services.AddAutoMapperProfiles(LoggerFactory.Create(builder =>
{
    builder.AddConsole(); // 或其他你需要的設定
}));

/********************************************
 * 加上Data Access
 * ******************************************/
builder.Services.AddScoped<IMenuItemDataAccess, MenuItemDataAccess>();
builder.Services.AddScoped<IManagementDataAccess, ManagementDataAccess>();
builder.Services.AddScoped<IOrderDataAccess, OrderDataAccess>();
builder.Services.AddScoped<IProductDataAccess, ProductDataAccess>();
builder.Services.AddScoped<ISalesDataAccess, SalesDataAccess>();

/********************************************
 * 加上Repository
 * ******************************************/
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IManagementRepository, ManagementRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>();

/********************************************
 * 加上Services
 * ******************************************/
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IManagementService, ManagementService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISalesService, SalesService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
