namespace SpiceCraft.Server.Service.Interface;

public interface IStorageService
{
    Task UploadImageAsync(string key, IFormFile image);
    Task UploadImageAsync(string key, IFormFile image, bool useMemoryStream);
    Task DeleteImageAsync(string key);
}