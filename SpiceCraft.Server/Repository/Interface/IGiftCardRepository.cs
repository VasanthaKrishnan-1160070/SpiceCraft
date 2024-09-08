using SpiceCraft.Server.DTO.GiftCard;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IGiftCardRepository
    {
        IEnumerable<GiftCardDTO> GetGiftCards(int userId);
        GiftCardDTO GetGiftCardByCode(string code);
        bool UpdateGiftCardBalance(string code, decimal balance);
    }
}
