using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.Enum;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Helpers.Request;
using SpiceCraft.Server.Repository;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics
{
    public class ProductLogics(
        IProductRepository productRepository,
        IInventoryRepository inventoryRepository,
        ICurrentUser currentUser
        ) : IProductLogics
    {
        public ResultDetail<IEnumerable<ProductCatalogItemDTO>> FilterProduct(ProductFilterRequest filter)
     {
        // Get the current user ID from the session helper
        var userId = currentUser.UserId;

        // Call the data access layer to filter products based on the provided criteria
        var result = productRepository.FilterProduct(userId, filter.CategoryId, filter.SubCategoryId, filter.Keyword, filter.Filter, filter.Sorting, filter.IncludeRemovedProducts);

        // Prepare the data dictionary to be returned
        var data = new Dictionary<string, object>
        {
            ["filter"] = new
            {
                category_id = categoryId,
                keyword = keyword ?? string.Empty,
                user_id = userId,
                sub_category_id = subCategoryId,
                filter,
                sorting
            }
        };

        // If products are found, return them with a success message
        if (result != null && result.Any())
        {
            data["products"] = result;
            return new ResultDetail(message: "", isSuccess: true, data: data, notify: false);
        }
        else
        {
            // If no products are found, return an empty product list with a failure message
            data["products"] = new List<object> { new { } };
            return new ResultDetail(message: "", isSuccess: false, data: data, notify: false);
        }
    }

    public ResultDetail GetInitialProductData()
    {
        var data = productRepository.GetProductDetail();
        if (data != null)
        {
            return new ResultDetail(message: "", isSuccess: true, data: data, notify: false);
        }
        return new ResultDetail(message: "", isSuccess: false, data: data, notify: true);
    }

    public ResultDetail GetProductDetail(int productId)
    {
        var result = productRepository.GetProductDetail(productId);
        if (result != null)
        {
            return new ResultDetail(message: "", isSuccess: true, data: result, notify: false);
        }
        return new ResultDetail(message: "", isSuccess: false, data: result, notify: true);
    }

    public ResultDetail CreateUpdateProductDetails(Dictionary<string, object> productDetails, Dictionary<string, object> productImageDetails)
    {
        var resultDetail = CreateUpdateProduct(productDetails);
        if (resultDetail.IsSuccess)
        {
            // Now we create the product inventory
            inventoryRepository.InsertProductToInventory(Convert.ToInt32(productDetails["product_id"]));

            // Now we save the images
            SaveProductImages(resultDetail.Data, productImageDetails);
        }
        return resultDetail;
    }

    public ResultDetail CreateUpdateProduct(Dictionary<string, object> productDetails)
    {
        if (!productDetails.ContainsKey("product_id"))
        {
            productDetails["created_date"] = DateTime.Now;
        }
        return InsertOrUpdateReturnResultDetail(Tables.Products, productDetails);
    }

    public void SaveProductImages(Dictionary<string, object> productDetails, Dictionary<string, object> productImageDetails)
    {
        int productId = Convert.ToInt32(productDetails["product_id"]);
        var productImages = productImageDetails["uploaded_images"] as List<string>;
        var mainImageCode = productImageDetails["main_image"] as string;
        int nextImageIndex = productRepository.GetNextImageIndex(new { product_id = productId });

        string staticImgFolderPath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "product_images");
        List<(string imgCode, int imgIndex, string imgName)> imgDetails = new List<(string, int, string)>();

        foreach (var img in productImages)
        {
            string baseFileName = Path.GetFileNameWithoutExtension(img);
            string ext = Path.GetExtension(img);
            if (!string.IsNullOrEmpty(ext))
            {
                string imgCode = $"{productId}_{nextImageIndex}{ext}";
                string imgName = baseFileName;
                imgDetails.Add((imgCode, nextImageIndex, imgName));

                if (img == mainImageCode)
                {
                    mainImageCode = imgCode;  // Set the main image
                }

                Directory.CreateDirectory(staticImgFolderPath);
                // Assuming `img` is a path, use File.Copy to save
                File.Copy(img, Path.Combine(staticImgFolderPath, imgCode));

                nextImageIndex++;
            }
        }

        foreach (var item in imgDetails)
        {
            var param = new Dictionary<string, object>
            {
                { "product_id", productId },
                { "image_code", item.imgCode },
                { "image_index", item.imgIndex },
                { "image_name", item.imgName },
                { "is_main", (item.imgCode == mainImageCode) }
            };
            InsertOrUpdateReturnResultDetail(Tables.ProductImages, param);
        }

        productRepository.UpdateMainImage(new { product_id = productId, image_code = mainImageCode });
    }

    public ResultDetail RemoveProductFromListing(int productId)
    {
        var result = ProductRepository.AddOrRemoveProductFromListing(productId, 1);
        if (result != null)
        {
            return new ResultDetail(message: "Product removed from the listing successfully", isSuccess: true, data: result, notify: true);
        }
        return new ResultDetail(message: "Could not remove product from the listing", isSuccess: false, data: result, notify: true);
    }

    public ResultDetail AddProductToListing(int productId)
    {
        var result = productRepository.AddOrRemoveProductFromListing(productId, 0);
        if (result != null)
        {
            return new ResultDetail(message: "Product added to the listing successfully", isSuccess: true, data: result, notify: true);
        }
        return new ResultDetail(message: "Could not add product to the listing", isSuccess: false, data: result, notify: true);
    }
    }

}


