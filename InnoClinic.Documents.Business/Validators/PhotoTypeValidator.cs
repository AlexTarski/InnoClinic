using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace InnoClinic.Documents.Business.Validators
{
    public static class PhotoTypeValidator
    {
        private static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg", ".gif" };
        private static readonly string[] AllowedContentTypes =
        {
        "image/png",
        "image/jpeg",
        "image/gif"
        };

        public static bool IsValidExtension(IFormFile photo)
        {
            var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return false;

            return true;
        }

        public static async Task<bool> IsValidContentTypeAsync(IFormFile photo)
        {
            if (!AllowedContentTypes.Contains(photo.ContentType))
                return false;

            if (!await IsValidImageSignatureAsync(photo))
                return false;

            return true;
        }

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