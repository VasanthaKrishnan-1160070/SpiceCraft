using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Report;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics
{

    // ReportLogics.cs
    // ReportLogics.cs
    public class ReportLogics : IReportLogics
    {
        private readonly IReportRepository _reportRepository;

        public ReportLogics(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<ResultDetail<IEnumerable<object>>> GetReportByNameAsync(string reportName)
        {
            switch (reportName.ToLower())
            {
                case "monthly-order-summary":
                case "monthly-sales-summary":    
                    return await GetMonthlyOrderSummaryAsync();
                case "category-sales-summary":
                    return await GetCategoryWiseSalesSummaryAsync();
                case "payment-method-summary":
                    return await GetPaymentMethodSummaryAsync();
                case "product-sales-by-month":
                    return await GetProductSalesByMonthAsync();
                case "most-sold-products":
                    return await GetMostSoldProductsAsync();
                case "monthly-profit":
                    return await GetMonthlyProfitAsync();
                default:
                    return HelperFactory.Msg.Error<IEnumerable<object>>("Invalid report name.");
            }
        }

        private async Task<ResultDetail<IEnumerable<object>>> GetMonthlyOrderSummaryAsync()
        {
            var monthlyOrderSummary = _reportRepository.GetMonthlyOrderSummary();
            if (monthlyOrderSummary != null && monthlyOrderSummary.Any())
            {
                return HelperFactory.Msg.Success(monthlyOrderSummary.Cast<object>(),
                    "Monthly order summary retrieved successfully.");
            }

            return HelperFactory.Msg.Error<IEnumerable<object>>("No data found for monthly order summary.");
        }

        private async Task<ResultDetail<IEnumerable<object>>> GetCategoryWiseSalesSummaryAsync()
        {
            var categorySalesSummary = _reportRepository.GetCategoryWiseSalesSummary();
            if (categorySalesSummary != null && categorySalesSummary.Any())
            {
                return HelperFactory.Msg.Success(categorySalesSummary.Cast<object>(),
                    "Category-wise sales summary retrieved successfully.");
            }

            return HelperFactory.Msg.Error<IEnumerable<object>>("No data found for category-wise sales summary.");
        }

        private async Task<ResultDetail<IEnumerable<object>>> GetPaymentMethodSummaryAsync()
        {
            var paymentMethodSummary = _reportRepository.GetPaymentMethodSummary();
            if (paymentMethodSummary != null && paymentMethodSummary.Any())
            {
                return HelperFactory.Msg.Success(paymentMethodSummary.Cast<object>(),
                    "Payment method summary retrieved successfully.");
            }

            return HelperFactory.Msg.Error<IEnumerable<object>>("No data found for payment method summary.");
        }

        private async Task<ResultDetail<IEnumerable<object>>> GetProductSalesByMonthAsync()
        {
            var productSalesByMonth = _reportRepository.GetProductSalesByMonth();
            if (productSalesByMonth != null && productSalesByMonth.Any())
            {
                return HelperFactory.Msg.Success(productSalesByMonth.Cast<object>(),
                    "Product sales by month retrieved successfully.");
            }

            return HelperFactory.Msg.Error<IEnumerable<object>>("No data found for product sales by month.");
        }

        private async Task<ResultDetail<IEnumerable<object>>> GetMostSoldProductsAsync()
        {
            var mostSoldProducts = _reportRepository.GetMostSoldProducts();
            if (mostSoldProducts != null && mostSoldProducts.Any())
            {
                return HelperFactory.Msg.Success(mostSoldProducts.Cast<object>(),
                    "Most sold products retrieved successfully.");
            }

            return HelperFactory.Msg.Error<IEnumerable<object>>("No data found for most sold products.");
        }

        private async Task<ResultDetail<IEnumerable<object>>> GetMonthlyProfitAsync()
        {
            var monthlyProfit = _reportRepository.GetMonthlyProfit();
            if (monthlyProfit != null && monthlyProfit.Any())
            {
                return HelperFactory.Msg.Success(monthlyProfit.Cast<object>(),
                    "Monthly profit report retrieved successfully.");
            }

            return HelperFactory.Msg.Error<IEnumerable<object>>("No data found for monthly profit report.");
        }
    }


}
