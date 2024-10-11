using SpiceCraft.Server.DTO.Category;
using SpiceCraft.Server.DTO.ItemImage;
using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.Enum;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IProductRepository
    {
        public IEnumerable<ProductCatalogItemDTO> FilterProduct(int userId, int categoryId = 0, int subCategoryId = 0, string keyword = "", ProductFilterEnum filter = ProductFilterEnum.None, ProductSortingEnum sorting = ProductSortingEnum.NameAToZ, bool includeRemovedProducts = false);
                
        public IEnumerable<CategoryDTO> GetSubCategories(int? parentCategoryId);

        public ProductSummaryDTO CreateUpdateProduct(ProductSummaryDTO product);

        public bool CreateUpdateItemImages(IEnumerable<ProductImageDto> productImages);

        public int GetProductImageNextIndex(int productId);
        
        public void UpdateMainImage(int productId, string imageCode);
        
        public IEnumerable<ItemImageDTO> GetProductImages(int? productId);
          
        public IEnumerable<CategoryDTO> GetParentCategories();
        public ProductDetailDTO GetProductDetail(int? productId);

        public bool AddOrRemoveProductFromListing(int ItemId, bool isRemove = false);

        Task<int?> SaveImageDetails(ItemImagesDTO imageDetails);

        void DeleteItemImage(int itemId, string imageCode);
    }


}
