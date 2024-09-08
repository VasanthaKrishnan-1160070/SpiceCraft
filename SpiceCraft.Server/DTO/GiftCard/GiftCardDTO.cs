namespace SpiceCraft.Server.DTO.GiftCard
{
    public class GiftCardDTO
    {
        public int GiftCardId { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public string Balance { get; set; }
        public string IsActive { get; set; }
        public string? PurchasedDate { get; set; }
    }
}
