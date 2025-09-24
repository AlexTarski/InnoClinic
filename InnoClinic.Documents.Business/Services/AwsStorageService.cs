using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

using InnoClinic.Documents.Business.Interfaces;
using InnoClinic.Documents.Infrastructure;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InnoClinic.Documents.Business.Services
{
    public class AwsStorageService : IStorageService
    {
        private readonly ILogger<AwsStorageService> _logger;
        private readonly IAmazonS3 _s3;
        private readonly IOptions<AwsSettings> _awsSettings;

        public AwsStorageService(ILogger<AwsStorageService> logger, IAmazonS3 s3, IOptions<AwsSettings> options)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _s3 = s3 ?? throw new DiNullReferenceException(nameof(s3));
            _awsSettings = options ?? throw new DiNullReferenceException(nameof(options));
        }

        public async Task<string> GenerateLinkAsync(string objectPath, TimeSpan lifetime)
        {
            try
            {
                Logger.DebugStartProcessingMethod(_logger, nameof(GenerateLinkAsync));
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _awsSettings.Value.BucketName,
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

        public async Task<string> AddFileAsync(Guid fileId, IFormFile file, UploadFileType uploadFileType)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(AddFileAsync));
            var objectKey = GetObjectKey(fileId, uploadFileType, Path.GetFileName(file.FileName));
            try
            {
                await UploadFileAsync(file, objectKey);
                Logger.DebugExitingMethod(_logger, nameof(AddFileAsync));

                return objectKey;
            }
            catch (Exception ex)
            {
                Logger.Error(_logger, ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateFileAsync(IFormFile file, string objectKey)
        {
            try
            {
                Logger.DebugStartProcessingMethod(_logger, nameof(UpdateFileAsync));
                await UploadFileAsync(file, objectKey);
                Logger.DebugExitingMethod(_logger, nameof(AddFileAsync));
            }
            catch (Exception ex)
            {
                Logger.Error(_logger, ex, ex.Message);
                throw;
            }
        }

        private string GetObjectKey(Guid fileId, UploadFileType uploadFileType, string fileName)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetObjectKey));

            return uploadFileType switch
            {
                UploadFileType.PhotoDoctor => GenerateObjectKey(_awsSettings.Value.PhotoDoctorPath, fileId, fileName),
                UploadFileType.PhotoReceptionist => GenerateObjectKey(_awsSettings.Value.PhotoReceptionistPath, fileId, fileName),
                UploadFileType.PhotoPatient => GenerateObjectKey(_awsSettings.Value.PhotoPatientPath, fileId, fileName),
                UploadFileType.PhotoOffice => GenerateObjectKey(_awsSettings.Value.PhotoOfficePath, fileId, fileName),
                _ => throw new InvalidEnumArgumentException($"{nameof(UploadFileType)} has invalid type"),
            };
        }

        private string GenerateObjectKey(string fileDirectory, Guid fileId, string fileName)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GenerateObjectKey));
            return $"{fileDirectory}/{fileId}_{fileName}";
        }

        private async Task UploadFileAsync(IFormFile file, string objectKey)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(UploadFileAsync));
            using (var stream = file.OpenReadStream())
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = objectKey,
                    BucketName = _awsSettings.Value.BucketName,
                    ContentType = file.ContentType
                };

                var transfer = new TransferUtility(_s3);
                Logger.DebugPrepareToEnter(_logger, nameof(transfer.UploadAsync));
                var uploadTask = transfer.UploadAsync(uploadRequest);
                await uploadTask;

                if (!uploadTask.IsCompletedSuccessfully)
                {
                    Logger.WarningFailedDoAction(_logger, nameof(transfer.UploadAsync));
                    throw new UploadFailedException();
                }

                Logger.DebugExitingMethod(_logger, nameof(UploadFileAsync));
            }
        }
    }
}