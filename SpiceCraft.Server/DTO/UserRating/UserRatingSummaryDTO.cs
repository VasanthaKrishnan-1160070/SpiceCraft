namespace SpiceCraft.Server.DTO.UserRating
{
    public class UserRatingSummaryDTO
    {
        public int? UserItemRatingId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public int Rating { get; set; }
        public string RatingDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
