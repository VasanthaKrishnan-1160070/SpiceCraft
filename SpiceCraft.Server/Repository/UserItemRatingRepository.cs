using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.UserRating;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using SpiceCraft.Server.ML.Models.SentitmentAnalysis;

namespace SpiceCraft.Server.Repository
{
    public class UserItemRatingRepository : IUserItemRatingRepository
    {
        private readonly SpiceCraftContext _context;

        public UserItemRatingRepository(SpiceCraftContext context)
        {
            _context = context;
        }

        public async Task<UserItemRatingDTO> GetUserItemRatingAsync(int userId, int itemId)
        {
            var rating = await _context.UserItemRatings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ItemId == itemId);
            return rating == null ? null : new UserItemRatingDTO
            {
                UserItemRatingId = rating.UserItemRating1,
                UserId = rating.UserId,
                ItemId = rating.ItemId,
                Rating = rating.Rating ?? 0,
                RatingDescription = rating.RatingDescription,
                IsNegativeReview = rating.IsNegativeReview,
                ImprovementDescription = rating.ImprovementDescription,
            };
        }

        public async Task<List<UserRatingSummaryDTO>> GetItemRatingsAsync(int itemId)
        {
            return await _context.UserItemRatings
                .Where(r => r.ItemId == itemId)
                .Select(r => new UserRatingSummaryDTO
                {
                    UserItemRatingId = r.UserItemRating1,
                    UserName = r.User.UsersCredential.UserName,
                    FirstName = r.User.FirstName,
                    UserId = r.UserId,
                    Rating = r.Rating ?? 0,
                    RatingDescription = r.RatingDescription,
                })
                .ToListAsync();
        }

        public async Task<UserItemRatingDTO> AddOrUpdateRatingAsync(UserItemRatingDTO userItemRatingDTO)
        {
            var rating = await _context.UserItemRatings
                .FirstOrDefaultAsync(r => r.UserId == userItemRatingDTO.UserId && r.ItemId == userItemRatingDTO.ItemId);

            if (rating == null)
            {
                rating = new UserItemRating
                {
                    UserId = userItemRatingDTO.UserId,
                    ItemId = userItemRatingDTO.ItemId,
                    Rating = userItemRatingDTO.Rating,
                    RatingDescription = userItemRatingDTO.RatingDescription
                };
                _context.UserItemRatings.Add(rating);
            }
            else
            {
                rating.Rating = userItemRatingDTO.Rating;
                rating.RatingDescription = userItemRatingDTO.RatingDescription;
                rating.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            userItemRatingDTO.UserItemRatingId = rating.UserItemRating1;
            return userItemRatingDTO;
        }
        
        public async Task<List<StarRatingSummaryDTO>> GetStarRatingsSummaryAsync(int itemId)
        {
            var ratings = await _context.UserItemRatings
                .Where(r => r.ItemId == itemId)
                .GroupBy(r => r.Rating)
                .Select(g => new StarRatingSummaryDTO
                {
                    Stars = g.Key ?? 0,
                    Count = g.Count()
                }).ToListAsync();

            return ratings;
        }
        
        public async Task<List<RatingDescriptionData>> GetRatingDescriptionsAsync()
        {
            return await _context.UserItemRatings
                .Where(r => !string.IsNullOrEmpty(r.RatingDescription))
                .Select(r => new RatingDescriptionData
                {
                    RatingDescription = r.RatingDescription,
                    Label = r.Rating >= 3  // ratings of 3 or higher are positive, others are negative
                })
                .ToListAsync();
        }
    }

}
