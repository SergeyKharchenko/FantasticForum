namespace Mvc.UtilityModels
{
    public class ImageUtilityModel    
    {
        public bool HasImage { get; private set; }
        public byte[] Data { get; private set; }
        public string ImageMimeType { get; private set; }

        public ImageUtilityModel(bool hasAvatar, byte[] avatarData = null, string imageMimeType = "")
        {
            HasImage = hasAvatar;
            Data = avatarData;
            ImageMimeType = imageMimeType;
        }
    }
}