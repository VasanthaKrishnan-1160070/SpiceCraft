using SpiceCraft.Server.DTO.UserRating;
using SpiceCraft.Server.ML.Models.SentitmentAnalysis;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IUserItemRatingRepository
    {
        Task<UserItemRatingDTO> GetUserItemRatingAsync(int userId, int itemId);
        Task<List<UserRatingSummaryDTO>> GetItemRatingsAsync(int itemId);
        Task<UserItemRatingDTO> AddOrUpdateRatingAsync(UserItemRatingDTO userItemRatingDTO);
        Task<List<StarRatingSummaryDTO>> GetStarRatingsSummaryAsync(int itemId);
        Task<List<RatingDescriptionData>> GetRatingDescriptionsAsync();
    }
}
