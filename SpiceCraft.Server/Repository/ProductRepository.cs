using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Category;
using SpiceCraft.Server.DTO.ItemImage;
using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.Enum;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;
using System.Drawing.Text;

namespace SpiceCraft.Server.Repository
{
    public class ProductRepository : IProductRepository
    {
        private SpiceCraftContext _context;
        public ProductRepository(SpiceCraftContext context) 
        { 
            _context = context;
        }

        public IEnumerable<ProductCatalogItemDTO> FilterProduct(int userId, int categoryId = 0, int subCategoryId = 0, string keyword = "", ProductFilterEnum filter = ProductFilterEnum.None, ProductSortingEnum sorting = ProductSortingEnum.NameAToZ, bool includeRemovedProducts = false)
        {
            // Initial query for products
            var query = from p in _context.Items
                        join pc in _context.ItemCategories on p.CategoryId equals pc.CategoryId
                        join iv in _context.Inventories on p.ItemId equals iv.ItemId
                        join pi in _context.ItemImages.Where(i => i.IsMain == true) on p.ItemId equals pi.ItemId into imageJoin
                        from pi in imageJoin.DefaultIfEmpty()
                        join pp in _context.PromotionItems on p.ItemId equals pp.ItemId into promoProductJoin
                        from pp in promoProductJoin.DefaultIfEmpty()
                        join prc in _context.PromotionCategories on p.CategoryId equals prc.CategoryId into promoCatJoin
                        from prc in promoCatJoin.DefaultIfEmpty()
                        join pcp in _context.PromotionComboItems on p.ItemId equals pcp.ItemId into comboJoin
                        from pcp in comboJoin.DefaultIfEmpty()
                        join pbp in _context.PromotionBulkItems on p.ItemId equals pbp.ItemId into bulkPromoJoin
                        from pbp in bulkPromoJoin.DefaultIfEmpty()
                        select new ProductCatalogItemDTO
                        {
                            Price = p.Price,
                            ItemName = p.ItemName,
                            Description = p.Description,
                            CreatedDate = p.CreatedAt,
                            IsRemoved = p.IsRemoved,
                            CurrentStock = iv.CurrentStock < 0 ? 0 : iv.CurrentStock,
                            OwnProduct = p.OwnProduct,
                            CategoryId = p.CategoryId,
                            ImageCode = pi.ImageCode,
                            CategoryName = pc.CategoryName,
                            ItemId = p.ItemId,
                            DiscountRate = pp != null ? pp.DiscountRate : (prc != null ? prc.DiscountRate : 0),
                            BulkDiscountRate = pbp != null ? pbp.DiscountRate : 0,
                            BulkDiscountRequiredQuantity = pbp != null ? pbp.RequiredQuantity : 0,
                            ComboName = pcp != null ? pcp.ComboName : "",
                            DiscountPrice = p.Price * (1 - ((pp != null ? pp.DiscountRate : (prc != null ? prc.DiscountRate : 0)) / 100)),
                            IsInSale = (pp != null || prc != null) ? "Yes" : "No",
                            IsLowStock = iv.CurrentStock < iv.LowStockThreshold ? "Yes" : "No",
                            IsNoStock = iv.CurrentStock < 0 ? "Yes" : "No"
                        };

            // Filtering based on categoryId and subCategoryId
            if (categoryId > 0)
            {
                query = query.Where(p => _context.ItemCategories.Where(c => c.ParentCategoryId == categoryId).Select(c => c.CategoryId).Contains(p.CategoryId));
            }
            if (subCategoryId > 0)
            {
                query = query.Where(p => p.CategoryId == subCategoryId);
            }

            // Filtering based on keyword
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p => p.ItemName.Contains(keyword.Trim()) || p.Description.Contains(keyword.Trim()));
            }

            // Exclude removed products if specified
            if (!includeRemovedProducts)
            {
                query = query.Where(p => p.IsRemoved == false);
            }

            // Apply additional filters (InStock, OutOfStock, etc.)
            query = ApplyFilterCondition(query, filter);

            // Apply sorting
            query = ApplySortingCondition(query, sorting);

            return query.ToList();
        }

        public IQueryable<ProductCatalogItemDTO> ApplyFilterCondition(IQueryable<ProductCatalogItemDTO> query, ProductFilterEnum filter)
        {
            if (filter == ProductFilterEnum.None)
            {
                return query;
            }

            if (filter == ProductFilterEnum.InSale)
            {
                query = query.Where(p => p.IsInSale == "Yes");
            }
            else if (filter == ProductFilterEnum.NotInSale)
            {
                query = query.Where(p => p.IsInSale == "No");
            }
            else if (filter == ProductFilterEnum.InStock)
            {
                query = query.Where(p => p.CurrentStock >= 0);
            }
            else if (filter == ProductFilterEnum.OutOfStock)
            {
                query = query.Where(p => p.CurrentStock <= 0);
            }
            else if (filter == ProductFilterEnum.IsLowStock )
            {
                query = query.Where(p => p.IsLowStock == "Yes");
            }

            return query;
        }

        public IQueryable<ProductCatalogItemDTO> ApplySortingCondition(IQueryable<ProductCatalogItemDTO> query, ProductSortingEnum sorting)
        {
            // Apply sorting conditions based on your requirements
            switch (sorting)
            {
                case ProductSortingEnum.PriceLowToHigh:
                    query = query.OrderBy(p => p.Price);
                    break;
                case ProductSortingEnum.PriceHighToLow:
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case ProductSortingEnum.NameAToZ:
                    query = query.OrderBy(p => p.ItemName);
                    break;
                case ProductSortingEnum.NameZToA:
                    query = query.OrderByDescending(p => p.ItemName);
                    break;
                default:
                    query = query.OrderBy(p => p.CreatedDate);
                    break;
            }

            return query;
        }

        public ProductSummaryDTO CreateUpdateProduct(ProductSummaryDTO product)
        {
            // Check if the product already exists by ItemId
            Item item = _context.Items.FirstOrDefault(i => i.ItemId == product.ItemId);

            if (item == null)  // If the item doesn't exist, create a new one
            {
                item = new Item()
                {
                    CategoryId = product.SubCategoryId,
                    Description = product.Description,
                    Discount = product.DiscountRate,
                    ItemName = product.ItemName,
                    Price = product.Price,
                    OwnProduct = product.OwnProduct
                };

                _context.Items.Add(item);  // Add new item
            }
            else  // If the item exists, update its fields
            {
                item.CategoryId = product.SubCategoryId;
                item.Description = product.Description;
                item.Discount = product.DiscountRate;
                item.ItemName = product.ItemName;
                item.Price = product.Price;
                item.OwnProduct = product.OwnProduct;

                _context.Items.Update(item);  // Update the existing item
            }

            _context.SaveChanges();  // Save changes to the database

            product.ItemId = item.ItemId;  // Return the updated product ID
            return product;
        }

        public bool CreateUpdateItemImages(IEnumerable<ProductImageDto> productImages)
        {
            List<ItemImage> itemImages = new();
            foreach (var image in productImages)
            {
                var itemImage = new ItemImage()
                {
                    ImageCode = image.ImageCode,
                    ImageIndex = image.ImageIndex,
                    ImageName = image.ImageName,
                    IsMain = image.IsMain,
                    ItemId = image.ItemId
                };
                itemImages.Add(itemImage);
            }
            _context.ItemImages.AddRange(itemImages);
            int status = _context.SaveChanges();
            
            // now make sure there is only one main image for each product
            var mainImageItem = productImages?.FirstOrDefault(f => f.IsMain);
            if (mainImageItem != null)
            {
                UpdateMainImage(mainImageItem.ItemId, mainImageItem.ImageCode);   
            }

            return status > 0;
        }

        public void DeleteItemImage(int itemId, string imageCode)
        {
            var itemImage = _context.ItemImages.FirstOrDefault(img => img.ItemId == itemId && img.ImageCode == imageCode);
            if (itemImage != null)
            {
                _context.ItemImages.Remove(itemImage);
                _context.SaveChanges();
            }
        }

        public IEnumerable<CategoryDTO> GetSubCategories(int? parentCategoryId)
        {
            // We fetch the subcategories for the given parent category ID
            var subCategories = _context.ItemCategories
                .Where(pc => pc.ParentCategoryId == parentCategoryId)
                .Select(pc => new CategoryDTO
                {
                    CategoryId = pc.CategoryId,
                    CategoryName = pc.CategoryName
                })
                .ToList();

            return subCategories;
        }

        public int GetProductImageNextIndex(int itemId)
        {
            // Query to get the maximum image index for the given product ID
            var maxIndex = _context.ItemImages
                .Where(pi => pi.ItemId == itemId)
                .Max(pi => (int?)pi.ImageIndex) ?? 0;

            // Increment the max index by 1
            return maxIndex + 1;
        }

        public void UpdateMainImage(int ItemId, string imageCodeOrName)
        {
            // First, set `IsMain = false` for all images except the given main image
            var imagesToUpdate = _context.ItemImages
                .Where(pi => pi.ItemId == ItemId && (pi.ImageCode != imageCodeOrName || pi.ImageName == imageCodeOrName))
                .ToList();

            foreach (var image in imagesToUpdate)
            {
                image.IsMain = false;
            }

            // Save changes to update the images
            _context.SaveChanges();

            // Then, set `IsMain = true` for the given main image
            var mainImage = _context.ItemImages
                .FirstOrDefault(pi => pi.ItemId == ItemId && (pi.ImageCode == imageCodeOrName || pi.ImageName == imageCodeOrName));

            if (mainImage != null)
            {
                mainImage.IsMain = true;
                _context.SaveChanges();
            }
        }

        public IEnumerable<ItemImageDTO> GetProductImages(int? productId)
        {
            // We fetch the images for the given product ID
            var productImages = _context.ItemImages
                .Where(pi => pi.ItemId == productId)
                .Select(pi => new ItemImageDTO
                {
                    ItemId = pi.ItemId,
                    ImageCode = pi.ImageCode,
                    ImageName = pi.ImageName,
                    ImageIndex = pi.ImageIndex,
                    IsMain = pi.IsMain
                })
                .ToList();

            return productImages;
        }

        public IEnumerable<CategoryDTO> GetParentCategories()
        {
            var categories = _context.ItemCategories
                .Where(pc => pc.ParentCategoryId == null)
                .Select(pc => new CategoryDTO
                {
                    CategoryId = pc.CategoryId,
                    CategoryName = pc.CategoryName
                })
                .ToList();

            return categories;
        }

        public ProductDetailDTO GetProductDetail(int? productId)
        {
            // Get parent categories (assuming a method to retrieve them)
            var categories = GetParentCategories();

            // If productId is null, return an empty response with default values
            if (productId == null)
            {
                return new ProductDetailDTO
                {
                    ProductDetails = null,
                    Categories = categories,
                    SubCategories = null,
                    ProductImages = null
                };
            }

            // Query the product details using LINQ
            var productDetails = (from pd in _context.Items
                                  join pc in _context.ItemCategories on pd.CategoryId equals pc.CategoryId
                                  join pp in _context.PromotionItems on pd.ItemId equals pp.ItemId into productPromo
                                  from pp in productPromo.DefaultIfEmpty()
                                  join prc in _context.PromotionCategories on pd.CategoryId equals prc.CategoryId into promoCat
                                  from prc in promoCat.DefaultIfEmpty()
                                  join iv in _context.Inventories on pd.ItemId equals iv.ItemId into inventory
                                  from iv in inventory.DefaultIfEmpty()
                                  join pi in _context.ItemImages on pd.ItemId equals pi.ItemId into productImage
                                  from pi in productImage.DefaultIfEmpty()
                                  where pd.ItemId == productId
                                  select new ProductSummaryDTO
                                  {
                                      ItemId = pd.ItemId,
                                      ItemName = pd.ItemName,
                                      Description = pd.Description,
                                      CreatedAt = pd.CreatedAt,
                                      Price = pd.Price,
                                      OwnProduct = pd.OwnProduct,
                                      IsRemoved = pd.IsRemoved,
                                      CategoryName = pc.CategoryName,
                                      ParentCategoryId = _context.ItemCategories
                                      .Where(c => c.CategoryId == pc.CategoryId)
                                      .Select(c => c.ParentCategoryId)
                                      .FirstOrDefault(),
                                      SubCategoryId = pc.CategoryId,
                                      DiscountRate = (pp != null ? pp.DiscountRate : prc != null ? prc.DiscountRate : 0),
                                      CurrentStock = iv.CurrentStock,
                                      IsMain = pi.IsMain
                                  }).FirstOrDefault();

            // Get product images (assuming a method to retrieve images)
            var productImages = GetProductImages(productId);

            // Get subcategories based on the parent category ID
            var subCategories = GetSubCategories(productDetails?.ParentCategoryId);

            return new ProductDetailDTO
            {
                ProductDetails = productDetails,
                Categories = categories,
                SubCategories = subCategories,
                ProductImages = productImages
            };
        }

        public bool AddOrRemoveProductFromListing(int ItemId, bool isRemove = false)
        {
            // Find the product by productId
            var product = _context.Items.FirstOrDefault(p => p.ItemId == ItemId);

            if (product != null)
            {
                // Update the `IsRemoved` status
                product.IsRemoved = isRemove;
                _context.SaveChanges();

                return true; // Return true if the update was successful
            }

            return false; // Return false if the product was not found
        }

        public async Task<int?> SaveImageDetails(ItemImagesDTO imageDetails)
        {
            List<ItemImage> itemImages = new List<ItemImage>();
            foreach (var imageName in imageDetails.ImageNames)
            {
                int nextImageIndex = GetProductImageNextIndex(imageDetails.ItemId);
                var itemImage = new ItemImage()
                {
                    ImageName = $"{nextImageIndex}-{imageDetails.ItemId}-{imageName}",
                    ItemId = imageDetails.ItemId,
                    IsMain = imageName == imageDetails.MainImageName,
                };
                itemImages.Add(itemImage);
            }

            _context.ItemImages.AddRange(itemImages);

            return await  _context.SaveChangesAsync();
        }

    }
}
