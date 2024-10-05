using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Order;
using SpiceCraft.Server.DTO.Payment;
using SpiceCraft.Server.DTO.User;
using SpiceCraft.Server.Models;
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
                    PaymentDate = pt.PaymentDate,
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
                    PaymentDate = pt.PaymentDate
                }).ToListAsync();
        
            return payments;
        }
        
        // Retrieves detailed invoice information for a specific payment transaction
        public async Task<PaymentInvoiceDTO> GetPaymentInvoiceDetailsAsync(int transactionId)
        {
            // Get payment and order details
            var orderPayments = await (from pt in _context.Payments
                join o in _context.Orders on pt.OrderId equals o.OrderId
                join so in _context.ShippingOptions on o.ShippingOptionId equals so.ShippingOptionId
                where pt.TransactionId == transactionId
                select new PaymentInvoiceDTO
                {
                    UserId = pt.UserId,
                    OrderId = pt.OrderId,
                    Amount = pt.Amount,
                    PaymentDate = pt.PaymentDate.ToString("dd/MM/yyyy"),
                    PaymentMethod = pt.PaymentMethod,
                    PaymentStatus = pt.PaymentStatus,
                    ShippingOptionName = so.ShippingOptionName,
                    // CountryName = c.Name,
                    IsFreeShipping = o.IsFreeShipping,
                    ShippingCost =
                        o.IsFreeShipping ? 0.00m : so.Cost,
                    SubTotal = o.IsFreeShipping 
                        ? pt.Amount
                        :  pt.Amount - so.Cost,
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
            
            orderPayments.OrderDetails = orderDetails;
            orderPayments.UserAddress = userAddress;
            orderPayments.ContactInfo = contactInfo;
        
            return orderPayments;
        }

        public async Task<bool> CreateUserPaymentAsync(PaymentDTO paymentDto)
        {
            var payment = new Payment()
            {
                TransactionId = paymentDto.TransactionId,
                UserId = paymentDto.UserId,
                OrderId = paymentDto.OrderId,
                Amount = paymentDto.PaymentAmount,
                PaymentMethod = paymentDto.PaymentMethod,
                PaymentStatus = paymentDto.PaymentStatus,
                PaymentDate = paymentDto.PaymentDate
            };
            await _context.Payments.AddAsync(payment);
            int status = await _context.SaveChangesAsync();
            return status > 0;
        }
    }
}
