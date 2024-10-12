using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Report;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.Repository
{
    // ReportRepository.cs
public class ReportRepository : IReportRepository
{
    private readonly SpiceCraftContext _context;

    public ReportRepository(SpiceCraftContext context)
    {
        _context = context;
    }

    public IEnumerable<OrderReportDTO> GetMonthlyOrderSummary()
    {
        return (from o in _context.Orders
                group o by new { o.OrderDate.Year, o.OrderDate.Month } into g
                select new OrderReportDTO
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                    OrderCount = g.Count(),
                    TotalSales = g.Sum(x => x.TotalCost)
                }).ToList();
    }

    public IEnumerable<CategorySalesDTO> GetCategoryWiseSalesSummary()
    {
        return (from od in _context.OrderDetails
                join i in _context.Items on od.ItemId equals i.ItemId
                join ic in _context.ItemCategories on i.CategoryId equals ic.CategoryId
                group od by ic.CategoryName into g
                select new CategorySalesDTO
                {
                    CategoryName = g.Key,
                    TotalSales = g.Sum(x => x.PurchasePrice * x.Quantity)
                }).ToList();
    }

    public IEnumerable<PaymentMethodReportDTO> GetPaymentMethodSummary()
    {
        return (from p in _context.Payments
                group p by p.PaymentMethod into g
                select new PaymentMethodReportDTO
                {
                    PaymentMethod = g.Key,
                    TotalPayments = g.Count(),
                    TotalAmount = g.Sum(x => x.Amount)
                }).ToList();
    }

    public IEnumerable<ProductSalesByMonthDTO> GetProductSalesByMonth()
    {
        return (from od in _context.OrderDetails
                join o in _context.Orders on od.OrderId equals o.OrderId
                join i in _context.Items on od.ItemId equals i.ItemId
                group od by new { o.OrderDate.Year, o.OrderDate.Month, od.ItemId, i.ItemName } into g
                select new ProductSalesByMonthDTO
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                    ItemId = g.Key.ItemId,
                    ItemName = g.Key.ItemName,
                    TotalQuantitySold = g.Sum(x => x.Quantity),
                    TotalSales = g.Sum(x => x.PurchasePrice * x.Quantity)
                }).ToList();
    }

    public IEnumerable<MostSoldProductsDTO> GetMostSoldProducts()
    {
        return (from od in _context.OrderDetails
                join i in _context.Items on od.ItemId equals i.ItemId
                group od by new { od.ItemId, i.ItemName } into g
                orderby g.Sum(x => x.Quantity) descending
                select new MostSoldProductsDTO
                {
                    ItemId = g.Key.ItemId,
                    ItemName = g.Key.ItemName,
                    TotalQuantitySold = g.Sum(x => x.Quantity),
                    TotalSales = g.Sum(x => x.PurchasePrice * x.Quantity)
                }).Take(10).ToList();
    }

    public IEnumerable<MonthlyProfitDTO> GetMonthlyProfit()
    {
        return (from o in _context.Orders
                join od in _context.OrderDetails on o.OrderId equals od.OrderId
                join i in _context.Items on od.ItemId equals i.ItemId
                group new { o, od, i } by new { o.OrderDate.Year, o.OrderDate.Month } into g
                select new MonthlyProfitDTO
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                    TotalSales = g.Sum(x => x.od.PurchasePrice * x.od.Quantity),
                    TotalCost = g.Sum(x => x.i.Price * x.od.Quantity),
                    Profit = g.Sum(x => x.od.PurchasePrice * x.od.Quantity) - g.Sum(x => x.i.Price * x.od.Quantity)
                }).ToList();
    }
}

}
