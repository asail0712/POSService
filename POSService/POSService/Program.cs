using Common.Profiles;
using DataAccess;
using DataAccess.Interface;
using Repository;
using Repository.Interface;
using Service;
using Service.Interface;

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

/********************************************
 * 註冊AutoMapper
 * ******************************************/
builder.Services.AddAutoMapper(typeof(MenuItemProfile));
builder.Services.AddAutoMapper(typeof(ProductInfoProfile));
builder.Services.AddAutoMapper(typeof(OrderDetailProfile));
builder.Services.AddAutoMapper(typeof(StaffDataProfile));
builder.Services.AddAutoMapper(typeof(SoldItemProfile));

/********************************************
 * 加上Data Access
 * ******************************************/
builder.Services.AddScoped<IItemDataAccess, ItemDataAccess>();
builder.Services.AddScoped<IManagementDataAccess, ManagementDataAccess>();
builder.Services.AddScoped<IOrderDataAccess, OrderDataAccess>();
builder.Services.AddScoped<IProductDataAccess, ProductDataAccess>();
builder.Services.AddScoped<ISalesDataAccess, SalesDataAccess>();

/********************************************
 * 加上Repository
 * ******************************************/
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IManagementRepository, ManagementRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>();

/********************************************
 * 加上Services
 * ******************************************/
builder.Services.AddScoped<IItemService, ItemService>();
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
