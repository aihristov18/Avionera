namespace Avionera.Interfaces
{
    public interface IImageConverter
    {
        public Task<byte[]> ToByteArrayAsync(IFormFile image);
        public byte[] ToByteArray(IFormFile image);
        public string ByteArrayToImgUrl(byte[] bytes);
        public IFormFile ByteArrayToFormFile(byte[] bytes);
    }
}
