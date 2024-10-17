using SpiceCraft.Server.DTO.UserRating;
using SpiceCraft.Server.Helpers;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface IUserItemRatingLogics
    {
        Task<ResultDetail<UserItemRatingDTO>> AddOrUpdateRatingAsync(UserItemRatingDTO userItemRatingDTO);
        Task<ResultDetail<UserItemRatingDTO>> GetUserItemRatingAsync(int userId, int itemId);
        Task<ResultDetail<List<UserRatingSummaryDTO>>> GetItemRatingsAsync(int itemId);
    }
}
