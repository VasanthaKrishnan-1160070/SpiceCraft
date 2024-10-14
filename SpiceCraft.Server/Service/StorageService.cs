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

    public StorageService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        var awsOptions = configuration.GetSection("AWS");
        var region = awsOptions["Region"];
        var accessKey = awsOptions["AccessKey"];
        var secretKey = awsOptions["SecretKey"];

        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
        };

        _s3Client = new AmazonS3Client(accessKey, secretKey, config);
        _bucketName = awsOptions["BucketName"];
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

