using Models.Abstract;

namespace Models
{
    public class Image : MongoEntity
    {
        public string ImageMimeType { get; set; }
        public byte[] Data { get; set; }
    }
}