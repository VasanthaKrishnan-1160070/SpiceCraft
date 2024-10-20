using Amazon.S3.Transfer;
using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.Repository.Interface;
using SpiceCraft.Server.Service.Interface;

namespace SpiceCraft.Server.Service;

using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public StorageService(IAmazonS3 s3Client, string bucketName)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
    }

    // Constructor to use AWS Profile for local development
    public StorageService(IConfiguration configuration, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            var awsOptions = configuration.GetAWSOptions();
            awsOptions.Profile = configuration["AWS:Profile"];
            _s3Client = awsOptions.CreateServiceClient<IAmazonS3>();
        }
        else
        {
            _s3Client = new AmazonS3Client();
        }
        _bucketName = configuration["AWS:BucketName"];
    }

    
    
    // public async Task UploadImageAsync(string key, IFormFile image)
    // {
    //     byte[] imageBytes = File.ReadAllBytes("C:/vasanth/IndustryProject/new/SpiceCraft/SpiceCraft.Server/uploads/items/10_6.jpg");  // Load the test image from disk
    //     using (var stream = new MemoryStream(imageBytes))
    //     {
    //         Console.WriteLine($"Stream length: {stream.Length} bytes");  // Log the length of the stream
    //
    //         var putRequest = new PutObjectRequest
    //         {
    //             BucketName = _bucketName,
    //             Key = key,
    //             InputStream = stream,
    //             ContentType = "image/jpeg"  // Hardcoded for testing
    //         };
    //
    //         await _s3Client.PutObjectAsync(putRequest);
    //     }
    // }
    
    // public async Task UploadImageAsync(string key, IFormFile image)
    // {
    //     // Step 1: Save the image locally for testing (Optional)
    //     var localPath = Path.Combine("C:/vasanth/IndustryProject/new/SpiceCraft/SpiceCraft.Server/uploads/items", image.FileName);
    //     using (var localStream = new FileStream(localPath, FileMode.Create))
    //     {
    //         await image.CopyToAsync(localStream);
    //     }
    //
    //     // Step 2: Re-open the saved file and upload it to S3
    //     using (var stream = new FileStream(localPath, FileMode.Open, FileAccess.Read))
    //     {
    //         Console.WriteLine($"Stream length for upload: {stream.Length} bytes");
    //
    //         var putRequest = new PutObjectRequest
    //         {
    //             BucketName = _bucketName,
    //             Key = key,
    //             InputStream = stream,
    //             ContentType = image.ContentType
    //         };
    //
    //         await _s3Client.PutObjectAsync(putRequest);
    //     }
    // }
    
    public async Task UploadImageAsync(string key, IFormFile image, bool useMemoryStream)
    {
        var tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_{image.FileName}");

        try
        {
            // Step 1: Save the image locally
            using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
            {
                await image.CopyToAsync(fileStream);  // Copy the file to the local disk
            }

            // Step 2: Read the local file and upload it to S3
            using (var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key,
                    InputStream = fileStream,  // Use the file stream from the local disk
                    ContentType = image.ContentType  // Set the content type from IFormFile
                };

                await _s3Client.PutObjectAsync(putRequest);  // Upload the file to S3
            }
        }
        catch (AmazonS3Exception s3Ex)
        {
            Console.WriteLine($"Error uploading to S3: {s3Ex.Message}");
            throw;  // Optionally rethrow or handle the S3 exception
        }
        catch (IOException ioEx)
        {
            Console.WriteLine($"File I/O error: {ioEx.Message}");
            throw;  // Optionally rethrow or handle the file I/O exception
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error uploading image: {ex.Message}");
            throw;  // Optionally rethrow or handle the general exception
        }
        finally
        {
            // Step 3: Delete the local file after upload
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }



    public async Task UploadImageAsync(string key, IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            throw new ArgumentException("Invalid file or empty file.");
        }

        try
        {
            var fileTransferUtility = new TransferUtility(_s3Client);
            // Upload the file using TransferUtility
            await fileTransferUtility.UploadAsync(image.OpenReadStream(), _bucketName, key);
        }

        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"AWS error: {e.Message}");
            throw;
        }
        catch (IOException ioEx)
        {
            Console.WriteLine($"IO error: {ioEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
            throw;
        }
    }


    public async Task DeleteImageAsync(string key)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };
        await _s3Client.DeleteObjectAsync(deleteRequest);
    }
}

