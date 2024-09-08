using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Helpers.Request;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics
{
    public class ProductLogics(
        IProductRepository productRepository,
        //  IInventoryRepository inventoryRepository,
        ICurrentUser currentUser
        ) : IProductLogics
    {
        public ResultDetail<IEnumerable<ProductCatalogItemDTO>> FilterProduct(ProductFilterRequest filter)
        {
            // Get the current user ID from the session helper
            var userId = currentUser.UserId;

            // Call the data access layer to filter products based on the provided criteria
            var result = productRepository.FilterProduct(userId, filter.CategoryId, filter.SubCategoryId, filter.Keyword, filter.Filter, filter.Sorting, filter.IncludeRemovedProducts);

            // If products are found, return them with a success message
            if (result != null && result.Any())
            {
                return new ResultDetail<IEnumerable<ProductCatalogItemDTO>>() { Message = "", IsSuccess = true, Data = result, Notify = false };
            }
            else
            {
                // If no products are found, return an empty product list with a failure message

                return new ResultDetail<IEnumerable<ProductCatalogItemDTO>>() { Message = "", IsSuccess = false, Data = result, Notify = false };
            }
        }

        public ResultDetail<ProductDetailDTO> GetInitialProductData()
        {
            var data = productRepository.GetProductDetail(null);

            if (data?.ProductDetails != null)
            {
                return HelperFactory.Msg.Success(data);
            }
            return HelperFactory.Msg.Error<ProductDetailDTO>("No product data found.");
        }

        public ResultDetail<ProductDetailDTO> GetProductDetail(int itemId)
        {
            var result = productRepository.GetProductDetail(itemId);
            if (result != null)
            {
                return HelperFactory.Msg.Success(result);
            }
            return HelperFactory.Msg.Error<ProductDetailDTO>("No product data found.");
        }

        public ProductSummaryDTO CreateUpdateProduct(ProductSummaryDTO productDetails)
        {
            if (productDetails is ProductSummaryDTO)
            {
                productRepository.CreateUpdateProduct(productDetails);
            }

            return productDetails;
        }

        public ResultDetail<bool> CreateUpdateProductDetails(ProductSummaryDTO productDetails, string mainImageCode, List<IFormFile> uploadedImages)
        {
            CreateUpdateProduct(productDetails);
            if (productDetails.ItemId != 0)
            {
                // Now we create the product inventory
                // inventoryRepository.InsertProductToInventory(Convert.ToInt32(productDetails["product_id"]));

                // Now we save the images
                SaveProductImages(productDetails.ItemId, mainImageCode, uploadedImages);
            }
            return HelperFactory.Msg.Success(true);
        }

        public void SaveProductImages(int itemId, string mainImageCode, List<IFormFile> uploadedImages)
        {
            int nextImageIndex = productRepository.GetProductImageNextIndex(itemId);

            string staticImgFolderPath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "product_images");
            List<ProductImageDto> imgDetails = new();

            foreach (var img in uploadedImages)
            {
                string baseFileName = Path.GetFileNameWithoutExtension(img.FileName);
                string ext = Path.GetExtension(img.FileName);
                if (!string.IsNullOrEmpty(ext))
                {
                    string imgCode = $"{itemId}_{nextImageIndex}{ext}";
                    string imgName = baseFileName;
                    imgDetails.Add(new ProductImageDto()
                    {
                        ImageCode = imgCode,
                        ImageName = imgName,
                        ImageIndex = nextImageIndex,
                        IsMain = img.FileName == mainImageCode,
                        ItemId = itemId
                    });

                    Directory.CreateDirectory(staticImgFolderPath);
                    // Assuming `img` is a path, use File.Copy to save
                    File.Copy(img.FileName, Path.Combine(staticImgFolderPath, imgCode));

                    nextImageIndex++;
                }
            }

            productRepository.CreateUpdateItemImages(imgDetails);
            // productRepository.UpdateMainImage(itemId, mainImageCode);
        }

        public ResultDetail<bool> RemoveProductFromListing(int itemId)
        {
            var isSuccess = productRepository.AddOrRemoveProductFromListing(itemId, true);
            if (isSuccess)
            {
                return HelperFactory.Msg.Success(true, "Product removed from the listing successfully", true);
            }
            return HelperFactory.Msg.Error<bool>("Could not remove product from the listing");
        }

        public ResultDetail<bool> AddProductToListing(int itemId)
        {
            var isSuccess = productRepository.AddOrRemoveProductFromListing(itemId);

            if (isSuccess)
            {
                return HelperFactory.Msg.Success(true, "Product added to the listing successfully", true);
            }
            return HelperFactory.Msg.Error<bool>("Could not add product to the listing");
        }
    }

}


