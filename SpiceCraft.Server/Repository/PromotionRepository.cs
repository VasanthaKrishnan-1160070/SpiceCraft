using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Promotions;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.Repository
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly SpiceCraftContext _context;

        public PromotionRepository(SpiceCraftContext context)
        {
            _context = context;
        }

        // Get promotions for products, categories, BOGO, and bulk products
        public PromotionDTO GetPromotions()
        {
            // Items with Promotions
            var products = (from p in _context.Items
                join pp in _context.PromotionItems on p.ItemId equals pp.ItemId into ppGroup
                from pp in ppGroup.DefaultIfEmpty()
                where p.IsRemoved == false
                select new ItemPromotionDTO
                {
                    ItemId = p.ItemId,
                    ItemName = p.ItemName,
                    CategoryName = _context.ItemCategories
                        .Where(c => c.CategoryId == p.CategoryId)
                        .Select(c => c.CategoryName.ToUpper())
                        .FirstOrDefault(),
                    ActualPrice = p.Price.ToString("C"),
                    DiscountRate = pp != null ? $"{pp.DiscountRate}%" : "No Discount",
                    PriceAfterDiscount = (p.Price * (1 - (pp != null ? pp.DiscountRate / 100.0M : 0))).ToString("C"),
                    HasPromotion = pp != null ? "Yes" : "No",
                    ActionHidden = pp != null ? "Remove" : "Add",
                    ActionAdd = pp != null ? false : true,
                    ActionRemove = pp != null ? true : false
                }).ToList();

            // Categories with Promotions
            var categories = (from pc in _context.ItemCategories
                join prc in _context.PromotionCategories on pc.CategoryId equals prc.CategoryId into prcGroup
                from prc in prcGroup.DefaultIfEmpty()
                where pc.ParentCategoryId != null
                select new CategoryPromotionDTO
                {
                    CategoryId = pc.CategoryId,
                    CategoryName = pc.CategoryName.ToUpper(),
                    ParentCategoryName = _context.ItemCategories
                        .Where(c => c.CategoryId == pc.ParentCategoryId)
                        .Select(c => c.CategoryName.ToUpper())
                        .FirstOrDefault(),
                    DiscountRate = prc != null ? $"{prc.DiscountRate}%" : "No Discount",
                    HasPromotion = prc != null ? "Yes" : "No",
                    ActionHidden = prc != null ? "Remove" : "Add",
                    ActionAdd = prc == null,
                    ActionRemove = prc != null
                }).ToList();

            // BOGO Promotions
            var bogoPromotions = (from p in _context.Items
                join pcp in _context.PromotionComboItems on p.ItemId equals pcp.ItemId into pcpGroup
                from pcp in pcpGroup.DefaultIfEmpty()
                where p.IsRemoved == false
                select new BogoPromotionDTO
                {
                    ItemId = p.ItemId,
                    ItemName = p.ItemName,
                    CategoryName = _context.ItemCategories
                        .Where(c => c.CategoryId == p.CategoryId)
                        .Select(c => c.CategoryName.ToUpper())
                        .FirstOrDefault(),
                    ActualPrice = p.Price.ToString("C"),
                    ComboName = pcp != null ? pcp.ComboName : "Not Specified",
                    BuyQuantity = pcp != null ? pcp.BuyQuantity.ToString() : "Not Specified",
                    GetQuantity = pcp != null ? pcp.GetQuantity.ToString() : "Not Specified",
                    HasPromotion = pcp != null ? "Yes" : "No",
                    ActionHidden = pcp != null ? "Remove" : "Add",
                    ActionAdd = pcp == null,
                    ActionRemove = pcp != null
                }).ToList();

            // Bulk Item Promotions
            var bulkPromotions = (from p in _context.Items
                join pbp in _context.PromotionBulkItems on p.ItemId equals pbp.ItemId into pbpGroup
                from pbp in pbpGroup.DefaultIfEmpty()
                where p.IsRemoved == false
                select new BulkPromotionDTO
                {
                    ItemId = p.ItemId,
                    ItemName = p.ItemName,
                    CategoryName = _context.ItemCategories
                        .Where(c => c.CategoryId == p.CategoryId)
                        .Select(c => c.CategoryName.ToUpper())
                        .FirstOrDefault(),
                    ActualPrice = p.Price.ToString("C"),
                    RequiredQuantityForPromotion = pbp != null ? pbp.RequiredQuantity.ToString() : "Not Specified",
                    DiscountRate = pbp != null ? $"{pbp.DiscountRate}%" : "No Discount",
                    HasPromotion = pbp != null ? "Yes" : "No",
                    ActionHidden = pbp != null ? "Remove" : "Add",
                    ActionAdd = pbp == null,
                    ActionRemove = pbp != null
                }).ToList();

            return new PromotionDTO
            {
                Items = products,
                Categories = categories,
                BogoPromotions = bogoPromotions,
                BulkPromotions = bulkPromotions
            };
        }

        // Add a new item promotion
        public bool AddItemPromotion(ItemPromotionDTO promotion)
        {
            var existingPromotion = _context.PromotionItems.FirstOrDefault(pp => pp.ItemId == promotion.ItemId);
            if (existingPromotion == null)
            {
                var newPromotion = new PromotionItem
                {
                    ItemId = promotion.ItemId,
                    DiscountRate = Convert.ToDecimal(promotion.DiscountRate) // Assuming DiscountRate is of type decimal
                };

                _context.PromotionItems.Add(newPromotion);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        
        // Add a new category promotion
        public bool AddCategoryPromotion(CategoryPromotionDTO promotion)
        {
            var existingPromotion = _context.PromotionCategories.FirstOrDefault(pc => pc.CategoryId == promotion.CategoryId);
            if (existingPromotion == null)
            {
                var newPromotion = new PromotionCategory
                {
                    CategoryId = promotion.CategoryId,
                    DiscountRate = Convert.ToDecimal(promotion.DiscountRate) // Assuming DiscountRate is of type decimal
                };

                _context.PromotionCategories.Add(newPromotion);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        
        // Add a new BOGO promotion
        public bool AddBogoPromotion(BogoPromotionDTO promotion)
        {
            var existingPromotion = _context.PromotionComboItems.FirstOrDefault(pcp => pcp.ItemId == promotion.ItemId);
            if (existingPromotion == null)
            {
                var newPromotion = new PromotionComboItem
                {
                    ItemId = promotion.ItemId,
                    ComboName = promotion.ComboName,
                    BuyQuantity = int.Parse(promotion.BuyQuantity), // Assuming quantities are integers
                    GetQuantity = int.Parse(promotion.GetQuantity)
                };

                _context.PromotionComboItems.Add(newPromotion);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        
        // Add a new bulk item promotion
        public bool AddBulkItemPromotion(BulkPromotionDTO promotion)
        {
            var existingPromotion = _context.PromotionBulkItems.FirstOrDefault(pbp => pbp.ItemId == promotion.ItemId);
            if (existingPromotion == null)
            {
                var newPromotion = new PromotionBulkItem
                {
                    ItemId = promotion.ItemId,
                    RequiredQuantity = int.Parse(promotion.RequiredQuantityForPromotion),
                    DiscountRate = Convert.ToDecimal(promotion.DiscountRate) // Assuming DiscountRate is of type decimal
                };

                _context.PromotionBulkItems.Add(newPromotion);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        // Remove product promotion
        public bool RemoveItemPromotion(int itemId)
        {
            var promotion = _context.PromotionItems.FirstOrDefault(pp => pp.ItemId == itemId);
            if (promotion != null)
            {
                _context.PromotionItems.Remove(promotion);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        // Remove category promotion
        public bool RemoveCategoryPromotion(int categoryId)
        {
            var promotion = _context.PromotionCategories.FirstOrDefault(pc => pc.CategoryId == categoryId);
            if (promotion != null)
            {
                _context.PromotionCategories.Remove(promotion);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        // Remove BOGO promotion
        public bool RemoveBogoPromotion(int itemId)
        {
            var promotion = _context.PromotionComboItems.FirstOrDefault(pcp => pcp.ItemId == itemId);
            if (promotion != null)
            {
                _context.PromotionComboItems.Remove(promotion);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        // Remove bulk product promotion
        public bool RemoveBulkItemPromotion(int itemId)
        {
            var promotion = _context.PromotionBulkItems.FirstOrDefault(pbp => pbp.ItemId == itemId);
            if (promotion != null)
            {
                _context.PromotionBulkItems.Remove(promotion);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        // Remove all product promotions
        public bool RemoveAllItemPromotions()
        {
            _context.PromotionItems.RemoveRange(_context.PromotionItems);
            _context.SaveChanges();
            return true;
        }

        // Remove all category promotions
        public bool RemoveAllCategoryPromotions()
        {
            _context.PromotionCategories.RemoveRange(_context.PromotionCategories);
            _context.SaveChanges();
            return true;
        }

        // Remove all BOGO promotions
        public bool RemoveAllBogoPromotions()
        {
            _context.PromotionComboItems.RemoveRange(_context.PromotionComboItems);
            _context.SaveChanges();
            return true;
        }

        // Remove all bulk product promotions
        public bool RemoveAllBulkItemPromotions()
        {
            _context.PromotionBulkItems.RemoveRange(_context.PromotionBulkItems);
            _context.SaveChanges();
            return true;
        }
    }
}
