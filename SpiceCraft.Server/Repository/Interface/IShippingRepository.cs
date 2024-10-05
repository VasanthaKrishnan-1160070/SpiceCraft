using SpiceCraft.Server.DTO.Checkout;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IShippingRepository
    {
        public Task<IEnumerable<ShippingOptionDTO>> GetShippingOptionsAsync();
    }
}
