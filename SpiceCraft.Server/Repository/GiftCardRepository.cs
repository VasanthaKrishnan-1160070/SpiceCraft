using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.GiftCard;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.Repository
{
    public class GiftCardRepository : IGiftCardRepository
    {
        private readonly SpiceCraftContext _context;

        public GiftCardRepository(SpiceCraftContext context)
        {
            _context = context;
        }

        // Get all gift cards for a user
        public IEnumerable<GiftCardDTO> GetGiftCards(int userId)
        {
            var giftCards = from gc in _context.GiftCards
                            join cgc in _context.CustomerGiftCards on gc.GiftCardId equals cgc.GiftCardId
                            where cgc.UserId == userId
                            orderby gc.CreatedAt descending
                            select new GiftCardDTO
                            {
                                GiftCardId = cgc.GiftCardId,
                                UserId = cgc.UserId,
                                Code = gc.Code,
                                Balance = gc.Balance,
                                IsActive = gc.IsActive ? "Yes" : "No",
                                PurchasedDate = gc.CreatedAt.HasValue ? gc.CreatedAt.Value.ToString("dd'/'MM'/'yyyy") : string.Empty
                            };

            return giftCards.ToList();
        }

        // Get a gift card by its code
        public GiftCardDTO GetGiftCardByCode(string code)
        {
            var giftCard = (from gc in _context.GiftCards
                            join cgc in _context.CustomerGiftCards on gc.GiftCardId equals cgc.GiftCardId
                            where gc.Code == code
                            select new GiftCardDTO
                            {
                                GiftCardId = gc.GiftCardId,
                                Code = gc.Code,
                                Balance = gc.Balance,
                                IsActive = gc.IsActive ? "Yes" : "No",
                                PurchasedDate = gc.CreatedAt.HasValue ? gc.CreatedAt.Value.ToString("dd'/'MM'/'yyyy") : string.Empty
                            }).FirstOrDefault();

            return giftCard;
        }

        // Update the balance of a gift card by its code
        public bool UpdateGiftCardBalance(string code, decimal balance)
        {
            var giftCard = (from gc in _context.GiftCards
                            join cgc in _context.CustomerGiftCards on gc.GiftCardId equals cgc.GiftCardId
                            where gc.Code == code
                            select gc).FirstOrDefault();

            if (giftCard != null)
            {
                giftCard.Balance = balance;
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
