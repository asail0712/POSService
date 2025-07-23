using Common.Extens;
using Common.Filter;
using POSService.Extension;

using XPlan.Utility;

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
await builder.Services.InitialMongoDBEntity(builder.Configuration);

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
builder.Services.AddDataAccesses();

/********************************************
 * 加上Repository
 * ******************************************/
builder.Services.AddRepositorys();

/********************************************
 * 加上Service
 * ******************************************/
builder.Services.AddServices();

/********************************************
 * 加上Controller
 * ******************************************/
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/********************************************
 * 加上Swagger 設定
 * ******************************************/
builder.Services.AddSwaggerGen(c =>
{
    c.GenerateSwaggerDoc(builder.Environment);

    c.OperationFilter<ControllerAddSummaryFilter>();    // 使用 Operation Filter 來給API加上註解
    c.DocumentFilter<ApiHiddenFilter>();                // 使用 Operation Filter 來給API加上開關
    c.OperationFilter<AddAuthorizeCheckFilter>();       // 使用 Operation Filter 來給API加上認證
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoints();
    });    
}

app.UseHttpsRedirection();

app.UseAuthentication();              // 先認證身分
app.UseAuthorization();               // 再授權權限

app.MapControllers();

app.Run();
