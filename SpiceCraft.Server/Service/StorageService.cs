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

    public async Task UploadImageAsync(string key, IFormFile image)
    {
        using (var stream = image.OpenReadStream())
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                ContentType = image.ContentType
            };
            await _s3Client.PutObjectAsync(putRequest);
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

