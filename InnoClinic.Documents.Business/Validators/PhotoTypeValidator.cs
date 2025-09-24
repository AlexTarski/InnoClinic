using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace InnoClinic.Documents.Business.Validators
{
    /// <summary>
    /// Provides validation utilities for uploaded <see cref="IFormFile"/> photos.
    /// Ensures that files have an allowed extension, a valid MIME content type, 
    /// and a correct binary signature for supported image formats (.png, .jpg, .jpeg, .gif).
    /// </summary>
    public static class PhotoTypeValidator
    {
        private static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg", ".gif" };
        private static readonly string[] AllowedContentTypes =
        {
        "image/png",
        "image/jpeg",
        "image/gif"
        };

        /// <summary>
        /// Checks whether the uploaded <see cref="IFormFile"/> has a valid file extension.
        /// </summary>
        /// <param name="photo">The uploaded photo file to validate.</param>
        /// <returns><c>true</c> if the file extension is allowed; otherwise, <c>false</c>.</returns>
        public static bool IsValidExtension(IFormFile photo)
        {
            var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return false;

            return true;
        }

        /// <summary>
        /// Validates the MIME content type and binary signature of the uploaded <see cref="IFormFile"/>.
        /// </summary>
        /// <param name="photo">The uploaded photo file to validate.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  
        /// The task result is <c>true</c> if the file has a valid content type and signature; otherwise, <c>false</c>.
        /// </returns>
        public static async Task<bool> IsValidContentTypeAsync(IFormFile photo)
        {
            if (!AllowedContentTypes.Contains(photo.ContentType))
                return false;

            if (!await IsValidImageSignatureAsync(photo))
                return false;

            return true;
        }

        /// <summary>
        /// Verifies the binary signature of the uploaded <see cref="IFormFile"/> 
        /// against known signatures for PNG, JPEG, and GIF formats.
        /// </summary>
        /// <param name="file">The uploaded photo file to inspect.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  
        /// The task result is <c>true</c> if the file signature matches a supported image format; otherwise, <c>false</c>.
        /// </returns>
        private static async Task<bool> IsValidImageSignatureAsync(IFormFile file)
        {
            byte[] pngSignature = [0x89, 0x50, 0x4E, 0x47];
            byte[] jpgSignature = [0xFF, 0xD8];
            byte[] gif87a = Encoding.ASCII.GetBytes("GIF87a");
            byte[] gif89a = Encoding.ASCII.GetBytes("GIF89a");

            using (var stream = file.OpenReadStream())
            {
                byte[] header = new byte[8];
                int bytesRead = await stream.ReadAsync(header, 0, header.Length);

                if (bytesRead < 4) return false;

                if (header.Take(4).SequenceEqual(pngSignature))
                    return true;

                if (header.Take(2).SequenceEqual(jpgSignature))
                    return true;

                if (header.Take(6).SequenceEqual(gif87a) || header.Take(6).SequenceEqual(gif89a))
                    return true;
            }

            return false;
        }
    }
}