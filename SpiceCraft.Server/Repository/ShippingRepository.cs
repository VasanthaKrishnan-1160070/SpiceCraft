using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Checkout;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.Repository
{
    public class ShippingRepository : IShippingRepository
    {
        private SpiceCraftContext _context;

        public ShippingRepository(SpiceCraftContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShippingOptionDTO>> GetShippingOptionsAsync()
        {
            var shippingOptions = await _context.ShippingOptions.ToListAsync();
            return shippingOptions.Select(so => new ShippingOptionDTO
            {
                ShippingOptionId = so.ShippingOptionId,
                ShippingOptionName = so.ShippingOptionName,
                Cost = so.Cost,
                FreeShippingThreshold = so.FreeShippingThreshold
            });
        }
    }
}
