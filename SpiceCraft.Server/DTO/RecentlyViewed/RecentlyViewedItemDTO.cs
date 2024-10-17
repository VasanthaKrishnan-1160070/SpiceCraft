namespace SpiceCraft.Server.DTO.RecentlyViewed
{
    public class RecentlyViewedItemDTO
    {
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public string ItemName { get; set; }
        public DateTime? ViewedAt { get; set; }
    }
}
