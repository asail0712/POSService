using DataAccess;
using DataAccess.Interface;
using Repository;
using Repository.Interface;
using Service;
using Service.Interface;

namespace POSService.Extension
{
    public static class AddServiceExtension
    {
        static public void AddDataAccesses(this IServiceCollection services)
        {
            services.AddScoped<IDishItemDataAccess, DishItemDataAccess>();
            services.AddScoped<IManagementDataAccess, ManagementDataAccess>();
            services.AddScoped<IOrderDataAccess, OrderDataAccess>();
            services.AddScoped<IProductDataAccess, ProductDataAccess>();
            services.AddScoped<ISalesDataAccess, OrderRecallDataAccess>();
        }

        static public void AddRepositorys(this IServiceCollection services)
        {
            services.AddScoped<IDishItemRepository, DishItemRepository>();
            services.AddScoped<IManagementRepository, ManagementRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISalesRepository, OrderRecallRepository>();
        }

        static public void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDishItemService, DishItemService>();
            services.AddScoped<IManagementService, ManagementService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderRecallService, OrderRecallService>();
        }
    }
}
