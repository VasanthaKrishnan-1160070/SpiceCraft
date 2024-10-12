using SpiceCraft.Server.DTO.Report;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IReportRepository
    {
        IEnumerable<OrderReportDTO> GetMonthlyOrderSummary();
        IEnumerable<CategorySalesDTO> GetCategoryWiseSalesSummary();
        IEnumerable<PaymentMethodReportDTO> GetPaymentMethodSummary();
        IEnumerable<ProductSalesByMonthDTO> GetProductSalesByMonth();
        IEnumerable<MostSoldProductsDTO> GetMostSoldProducts();
        IEnumerable<MonthlyProfitDTO> GetMonthlyProfit();
    }
}
