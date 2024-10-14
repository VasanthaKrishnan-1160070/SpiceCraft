namespace SpiceCraft.Server.Service.Interface;

public interface IStorageService
{
    Task UploadImageAsync(string key, IFormFile image);
    
    Task DeleteImageAsync(string key);
}