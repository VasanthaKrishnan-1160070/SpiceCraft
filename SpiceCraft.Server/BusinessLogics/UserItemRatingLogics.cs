using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.UserRating;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics
{
    public class UserItemRatingLogics : IUserItemRatingLogics
    {
        private readonly IUserItemRatingRepository _userItemRatingRepository;

        public UserItemRatingLogics(IUserItemRatingRepository userItemRatingRepository)
        {
            _userItemRatingRepository = userItemRatingRepository;
        }

        public async Task<ResultDetail<UserItemRatingDTO>> AddOrUpdateRatingAsync(UserItemRatingDTO userItemRatingDTO)
        {
            var result = await _userItemRatingRepository.AddOrUpdateRatingAsync(userItemRatingDTO);
            return HelperFactory.Msg.Success(result);
        }

        public async Task<ResultDetail<UserItemRatingDTO>> GetUserItemRatingAsync(int userId, int itemId)
        {
            var result = await _userItemRatingRepository.GetUserItemRatingAsync(userId, itemId);
            if (result == null)
            {
                return HelperFactory.Msg.Error<UserItemRatingDTO>("Rating not found");
            }
            return HelperFactory.Msg.Success(result);            
        }

        public async Task<ResultDetail<List<UserRatingSummaryDTO>>> GetItemRatingsAsync(int itemId)
        {
            var result = await _userItemRatingRepository.GetItemRatingsAsync(itemId);
            return HelperFactory.Msg.Success(result);
        }
    }

}
