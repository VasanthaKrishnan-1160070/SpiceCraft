namespace SpiceCraft.Server.DTO.UserRating
{
    public class UserItemRatingDTO
    {
        public int? UserItemRatingId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int Rating { get; set; }
        public string RatingDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
