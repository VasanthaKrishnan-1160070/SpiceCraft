using SpiceCraft.Server.DTO.UserRating;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IUserItemRatingRepository
    {
        Task<UserItemRatingDTO> GetUserItemRatingAsync(int userId, int itemId);
        Task<List<UserRatingSummaryDTO>> GetItemRatingsAsync(int itemId);
        Task<UserItemRatingDTO> AddOrUpdateRatingAsync(UserItemRatingDTO userItemRatingDTO);
    }
}
