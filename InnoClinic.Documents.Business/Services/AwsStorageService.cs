using System;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;

using InnoClinic.Documents.Business.Interfaces;
using InnoClinic.Documents.Infrastructure;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InnoClinic.Documents.Business.Services
{
    public class AwsStorageService : IStorageService
    {
        private readonly ILogger<AwsStorageService> _logger;
        private readonly IAmazonS3 _s3;
        private readonly string _bucket;

        public AwsStorageService(ILogger<AwsStorageService> logger, IAmazonS3 s3, IOptions<AwsSettings> options)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _s3 = s3 ?? throw new DiNullReferenceException(nameof(s3));
            _bucket = options.Value.BucketName ?? throw new DiNullReferenceException(nameof(options));
        }

        public async Task<string> GenerateLinkAsync(string objectPath, TimeSpan lifetime)
        {
            try
            {
                Logger.DebugStartProcessingMethod(_logger, nameof(GenerateLinkAsync));
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _bucket,
                    Key = objectPath,
                    Expires = DateTime.UtcNow.Add(lifetime)
                };

                return await _s3.GetPreSignedURLAsync(request);
            }
            catch (Exception ex)
            {
                Logger.Warning(_logger, ex, ex.Message);
                throw;
            }
        }
    }
}