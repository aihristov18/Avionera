using Avionera.Interfaces;
using System.Security.Cryptography;

namespace Avionera.Services
{
    public class ImageConverter : IImageConverter
    {
        public async Task<byte[]> ToByteArrayAsync(IFormFile image)
        {
            using (var stream = new MemoryStream())
            {
                await image.CopyToAsync(stream);
                return stream.ToArray();
            }
        }
        public byte[] ToByteArray(IFormFile image)
        {
            using (var stream = new MemoryStream())
            {
                image.CopyTo(stream);
                return stream.ToArray();
            }
        }
        public string ByteArrayToImgUrl(byte[] bytes)
        {
            string b64Data = Convert.ToBase64String(bytes);
            string imgUrl = string.Format("data:image/png;base64,{0}", b64Data);
            return imgUrl;
        }
        public IFormFile ByteArrayToFormFile(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, null, "image");
        }
    }
}
