using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.Repository.Interface;
using SpiceCraft.Server.Service.Interface;

namespace SpiceCraft.Server.Service;

public class ProductImageService : IProductImageService
{
    private readonly IStorageService _storageService;
    private readonly IProductRepository _productRepository;

    public ProductImageService(IStorageService storageService, IProductRepository productRepository)
    {
        _storageService = storageService;
        _productRepository = productRepository;
    }

    public async Task SaveProductImagesAsync(int itemId, string mainImageCode, List<IFormFile> uploadedImages, List<string>? removedImages)
    {
        int nextImageIndex = _productRepository.GetProductImageNextIndex(itemId);

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

                // Upload the file to S3
                await _storageService.UploadImageAsync($"items/{imgCode}", img);

                nextImageIndex++;
            }
        }

        if (removedImages?.Count > 0)
        {
            // Remove deleted images from S3 and database
            foreach (var imageCode in removedImages)
            {
                await _storageService.DeleteImageAsync($"items/{imageCode}");

                // Delete the image record from the database
                _productRepository.DeleteItemImage(itemId, imageCode);
            }
        }

        _productRepository.CreateUpdateItemImages(imgDetails);
        _productRepository.UpdateMainImage(itemId, mainImageCode);
    }
}