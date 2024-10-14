namespace SpiceCraft.Server.Service.Interface;

public interface IProductImageService
{
    Task SaveProductImagesAsync(int itemId, string mainImageCode, List<IFormFile> uploadedImages,
        List<string>? removedImages);
}