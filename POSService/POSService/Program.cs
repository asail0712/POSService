using AutoMapper;
using Common.Filter;
using Common.Profiles;
using DataAccess;
using DataAccess.Interface;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Interface;
using Service;
using Service.Interface;

using XPlan.Utility;
using XPlan.Utility.Databases;
using XPlan.Utility.Filter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/********************************************
 * 加上 Filter 
 * ******************************************/
builder.Services.AddGlobalExceptionHandling();

/********************************************
 * 加上 Settings
 * ******************************************/
builder.Services.AddCacheSettings(builder.Configuration);
builder.Services.AddJWTSecurity();

/********************************************
 * 加上Database Settings
 * ******************************************/
builder.Services.InitialMongoDB(builder.Configuration);
await builder.Services.InitialMongoDBEntity(builder.Configuration["MongoDbSetting:DatabaseName"]);

/********************************************
 * 流水號設定
 * ******************************************/
builder.Services.AddSingleton<ISequenceGenerator, SequenceGenerator>();

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
builder.Services.AddScoped<ISalesDataAccess, OrderRecallDataAccess>();

/********************************************
 * 加上Repository
 * ******************************************/
builder.Services.AddScoped<IDishItemRepository, DishItemRepository>();
builder.Services.AddScoped<IManagementRepository, ManagementRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISalesRepository, OrderRecallRepository>();

/********************************************
 * 加上Services
 * ******************************************/
builder.Services.AddScoped<IDishItemService, DishItemService>();
builder.Services.AddScoped<IManagementService, ManagementService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderRecallService, OrderRecallService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/********************************************
 * 加上Swagger 設定
 * ******************************************/
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<ControllerAddSummaryFilter>();    // 使用 Operation Filter 來給API加上註解
    c.DocumentFilter<ApiHiddenFilter>();                // 使用 Operation Filter 來給API加上開關
    c.OperationFilter<AddAuthorizeCheckFilter>();       // 使用 Operation Filter 來給API加上認證
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
