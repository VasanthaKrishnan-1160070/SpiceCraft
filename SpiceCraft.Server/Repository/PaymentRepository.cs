using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Order;
using SpiceCraft.Server.DTO.Payment;
using SpiceCraft.Server.DTO.User;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private SpiceCraftContext _context;

        public PaymentRepository(SpiceCraftContext context)
        {
            _context = context;
        }

        // Retrieves payment details for a specific user by userId
        public async Task<List<PaymentDTO>> GetPaymentsForUserAsync(int userId)
        {
            var payments = await (from pt in _context.Payments
                where pt.UserId == userId
                orderby pt.PaymentDate descending
                select new PaymentDTO
                {
                    TransactionId = pt.TransactionId,
                    PaymentAmount = pt.Amount,
                    OrderId = pt.OrderId,
                    PaymentStatus = pt.PaymentStatus,
                    PaymentDate = pt.PaymentDate.ToString("dd/MM/yyyy")
                }).ToListAsync();

            return payments;
        }

        // Retrieves payment details for all internal users
        public async Task<List<PaymentDTO>> GetPaymentsForInternalUsersAsync()
        {
            var payments = await (from pt in _context.Payments
                orderby pt.PaymentDate descending
                select new PaymentDTO
                {
                    TransactionId = pt.TransactionId,
                    PaymentAmount = pt.Amount,
                    OrderId = pt.OrderId,
                    PaymentStatus = pt.PaymentStatus,
                    PaymentDate = pt.PaymentDate.ToString("dd/MM/yyyy")
                }).ToListAsync();

            return payments;
        }

        // Retrieves detailed invoice information for a specific payment transaction
        public async Task<PaymentInvoiceDTO> GetPaymentInvoiceDetailsAsync(int transactionId)
        {
            // Get payment and order details
            var orderPayments = await (from pt in _context.Payments
                join o in _context.Orders on pt.OrderId equals o.OrderId
                // join cs in _context.CountryShippingOptions on o.CountryShippingOptionId equals cs
                //     .CountryShippingOptionId into csGroup
                // from cs in csGroup.DefaultIfEmpty()
                // join so in _context.ShippingOptions on cs.ShippingOptionId equals so.ShippingOptionId into soGroup
                // from so in soGroup.DefaultIfEmpty()
                // join c in _context.Countries on cs.CountryId equals c.CountryId into countryGroup
                // from c in countryGroup.DefaultIfEmpty()
                where pt.TransactionId == transactionId
                select new PaymentInvoiceDTO
                {
                    UserId = pt.UserId,
                    OrderId = pt.OrderId,
                    Amount = pt.Amount,
                    PaymentDate = pt.PaymentDate.ToString("dd/MM/yyyy"),
                    PaymentMethod = pt.PaymentMethod,
                    PaymentStatus = pt.PaymentStatus,
                    // ShippingOptionName = so.ShippingOptionName,
                    // CountryName = c.Name,
                    IsFreeShipping = o.IsFreeShipping ?? false,
                    // ShippingCost =
                    //     (o.IsFreeShipping ?? false) ? 0.00m : (cs != null && so.ShippingOptionId != 4 ? cs.Cost : 0.00m),
                    // SubTotal = (o.IsFreeShipping ?? false)
                    //     ? pt.Amount
                    //     : (cs != null && so.ShippingOptionId != 4 ? pt.Amount - cs.Cost : pt.Amount),
                    ShippingCost = 0,
                    subTotal = pt.Amount,
                    IsPickUp = (so.ShippingOptionId == 4 ? true : false)
                }).FirstOrDefaultAsync();

            if (orderPayments == null)
            {
                throw new Exception("Transaction not found.");
            }

            // Get order details
            var orderDetails = await (from od in _context.OrderDetails
                join p in _context.Items on od.ItemId equals p.ItemId
                where od.OrderId == orderPayments.OrderId
                select new OrderDetailDTO
                {
                    ItemId = od.ItemId,
                    Quantity = od.Quantity,
                    PurchasePrice = od.PurchasePrice,
                    ItemName = p.ItemName,
                    Price = p.Price
                }).ToListAsync();

            // Get user address
            var userAddress = await _context.UserAddresses
                .Where(ua => ua.UserId == orderPayments.UserId)
                .FirstOrDefaultAsync();

            // Get user contact info
            var contactInfo = await (from u in _context.Users
                where u.UserId == orderPayments.UserId
                select new ContactInfoDTO
                {
                    Name = u.Title + " " + u.FirstName + " " + u.LastName,
                    Email = u.Email,
                    Phone = u.Phone
                }).FirstOrDefaultAsync();

            return new PaymentInvoiceDTO
            {
                OrderPayments = orderPayments,
                OrderDetails = orderDetails,
                UserAddress = userAddress,
                ContactInfo = contactInfo
            };
        }
    }
}
