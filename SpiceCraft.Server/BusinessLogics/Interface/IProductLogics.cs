    using SpiceCraft.Server.DTO.Product;
    using SpiceCraft.Server.Helpers;
    using SpiceCraft.Server.Helpers.Request;

    namespace SpiceCraft.Server.BusinessLogics.Interface;

    public interface IProductLogics
    {
        public ResultDetail<IEnumerable<ProductCatalogItemDTO>> FilterProduct(ProductFilterRequest filter);

        public ResultDetail<ProductDetailDTO> GetProductDetail(int itemId);

        public ResultDetail<bool> AddProductToListing(int itemId);

        public ResultDetail<bool> RemoveProductFromListing(int itemId);

        public ResultDetail<bool> CreateUpdateProductDetails(ProductSummaryDTO productDetails, string mainImageCode, List<IFormFile> uploadedImages);

        public ProductSummaryDTO CreateUpdateProduct(ProductSummaryDTO productDetails);
}

