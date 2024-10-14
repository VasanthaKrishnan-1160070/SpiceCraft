using System.Configuration;
using Amazon.S3;
using SpiceCraft.Server.Repository.Interface;
using SpiceCraft.Server.Repository;
using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.BusinessLogics;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Service;
using SpiceCraft.Server.Service.Interface;
using Amazon.Extensions.NETCore.Setup;

namespace SpiceCraft.Server
{
    public static class DIContainer
    {
        public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Repository
            // AdminRepository
            services.AddScoped<IAdminRepository, AdminRepository>();

            // CartItemRepository
            services.AddScoped<ICartItemRepository, CartItemRepository>();

            // CartRepository
            services.AddScoped<ICartRepository, CartRepository>();

            // CheckoutRepository
            services.AddScoped<ICheckoutRepository, CheckoutRepository>();

            // DealersRepository
            services.AddScoped<IDealersRepository, DealersRepository>();

            // EnquiryRepository
            services.AddScoped<IEnquiryRepository, EnquiryRepository>();

            // GiftCardRepository
            services.AddScoped<IGiftCardRepository, GiftCardRepository>();

            // OrderHistoryRepository
            services.AddScoped<IOrderHistroyRepository, OrderHistroyRepository>();

            // OrderRepository
            services.AddScoped<IOrderRepository, OrderRepository>();

            // PaymentRepository
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            // ProductRepository
            services.AddScoped<IProductRepository, ProductRepository>();

            // PromotionRepository
            services.AddScoped<IPromotionRepository, PromotionRepository>();

            // ReportRepository
            services.AddScoped<IReportRepository, ReportRepository>();

            // RewardRepository
            services.AddScoped<IRewardRepository, RewardRepository>();

            // ShippingRepository
            services.AddScoped<IShippingRepository, ShippingRepository>();

            // UserRepository
            services.AddScoped<IUserRepository, UserRepository>();
            
            // Inventory Repository
            services.AddScoped<IInventoryRepository, InventoryRepository>();

            // Register Business Logics
            // AdminLogics
            services.AddScoped<IAdminLogics, AdminLogics>();

            // CartItemLogics
            services.AddScoped<ICartItemLogics, CartItemLogics>();

            // CartLogics
            services.AddScoped<ICartLogics, CartLogics>();

            // CheckoutLogics
            services.AddScoped<ICheckoutLogics, CheckoutLogics>();

            // DealersLogics
            services.AddScoped<IDealersLogics, DealersLogics>();

            // EnquiryLogics
            services.AddScoped<IEnquiryLogics, EnquiryLogics>();

            // GiftCardLogics
            services.AddScoped<IGiftCardLogics, GiftCardLogics>();

            // OrderHistoryLogics
            services.AddScoped<IOrderHistoryLogics, OrderHistoryLogics>();

            // OrderLogics
            services.AddScoped<IOrderLogics, OrderLogics>();

            // PaymentLogics
            services.AddScoped<IPaymentLogics, PaymentLogics>();

            // ProductLogics
            services.AddScoped<IProductLogics, ProductLogics>();

            // PromotionLogics
            services.AddScoped<IPromotionLogics, PromotionLogics>();

            // ReportLogics
            services.AddScoped<IReportLogics, ReportLogics>();

            // RewardLogics
            services.AddScoped<IRewardLogics, RewardLogics>();

            // ShippingLogics
            services.AddScoped<IShippingLogics, ShippingLogics>();

            // UserLogics
            services.AddScoped<IUserLogics, UserLogics>();
            
            // Helpers
            services.AddScoped<ICurrentUser, CurrentUser>();
            
            services.AddScoped<IInventoryLogics, InventoryLogics>();
            
            services.AddScoped<IStorageService, StorageService>();
            
            services.AddScoped<IProductImageService, ProductImageService>();

            // Configure AWS services with options from appsettings.json
           // services.AddDefaultAWSOptions(Configuration.GetAWSOptions());

            services.AddScoped<IAmazonS3, AmazonS3Client>();
        }
    }
}
