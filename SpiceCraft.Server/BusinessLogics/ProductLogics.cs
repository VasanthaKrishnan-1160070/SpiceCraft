using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Helpers.Request;
using SpiceCraft.Server.Repository.Interface;
using SpiceCraft.Server.Service.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpiceCraft.Server.BusinessLogics
{
    public class ProductLogics(
        IProductRepository productRepository,
        //  IInventoryRepository inventoryRepository,
        ICurrentUser currentUser,
        IProductImageService _productImageService
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
                return HelperFactory.Msg.SuccessList(result);                
            }
            else
            {
                // If no products are found, return an empty product list with a failure message
                return HelperFactory.Msg.Error(result);               
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

        public ResultDetail<bool> CreateUpdateProductDetails(CreateUpdateItemRequest createUpdateItem, List<IFormFile> uploadedImages)
        {
            var details = CreateUpdateProduct(createUpdateItem.ItemSummary);
            if (details.ItemId != 0)
            {
                // Now we create the product inventory
                // inventoryRepository.InsertProductToInventory(Convert.ToInt32(productDetails["product_id"]));

                // Now we save the images
                 SaveProductImages(details.ItemId, createUpdateItem.MainImageName, uploadedImages, createUpdateItem?.RemovedImages?.ToList());
                 // _productImageService.SaveProductImagesAsync(details.ItemId, createUpdateItem.MainImageName,
                 //    uploadedImages, createUpdateItem?.RemovedImages?.ToList());
                return HelperFactory.Msg.Success(true);
            }
            return HelperFactory.Msg.Error<bool>("Failed to save product images.");
        }

        public void SaveProductImages(int itemId, string mainImageCode, List<IFormFile> uploadedImages , List<string>? removedImages)
        {
            int nextImageIndex = productRepository.GetProductImageNextIndex(itemId);
            
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Items");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
           
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

                var filePath = Path.Combine(uploadFolder, imgCode);
                    
                 // Save the file to the server
                 using (var stream = new FileStream(filePath, FileMode.Create))
                 {
                     img.CopyTo(stream);
                 }

                    nextImageIndex++;
                }
            }

            if (removedImages?.Count > 0)
            {
                // Remove deleted images from file system and database
                foreach (var imageCode in removedImages)
                {
                    var filePath = Path.Combine(uploadFolder, imageCode);

                    // Delete the file from the server
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    // Delete the image record from the database
                    productRepository.DeleteItemImage(itemId, imageCode);
                }
            }

            productRepository.CreateUpdateItemImages(imgDetails);
            
            productRepository.UpdateMainImage(itemId, mainImageCode);
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

        // public async Task<ResultDetail<int?>> UploadItemImages(IFormFileCollection files, string mainImageName, int itemId)
        // {
        //     if (files == null || files.Count == 0)
        //     {
        //         return HelperFactory.Msg.Error<int?>("Files could not be uploaded");
        //     }
        //
        //     var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        //     if (!Directory.Exists(uploadFolder))
        //     {
        //         Directory.CreateDirectory(uploadFolder);
        //     }
        //
        //     var uploadedFileNames = new List<string>(); // List to store file names
        //
        //     foreach (var file in files)
        //     {
        //         var uniqueFileName = Path.GetFileNameWithoutExtension(file.FileName) 
        //                              + "_" + Path.GetRandomFileName() 
        //                              + Path.GetExtension(file.FileName); // Generate unique name for the file
        //         var filePath = Path.Combine(uploadFolder, uniqueFileName);
        //
        //         // Save the file to the server
        //         using (var stream = new FileStream(filePath, FileMode.Create))
        //         {
        //             await file.CopyToAsync(stream);
        //         }
        //
        //         // Add file name to the list (to save in the database later)
        //         uploadedFileNames.Add(uniqueFileName);
        //     }
        //
        //     // saving to the database
        //     var itemImagesDTO = new ItemImagesDTO()
        //     {
        //         ItemId = itemId, // Replace with actual ItemId
        //         ImageNames = uploadedFileNames,
        //         MainImageName = mainImageName
        //     };
        //     
        //     await productRepository.SaveImageDetails(itemImagesDTO);
        //     
        //     // Return the file count
        //     return HelperFactory.Msg.Success(uploadedFileNames?.Count());
        // }
    }

}


