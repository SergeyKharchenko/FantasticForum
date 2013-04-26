namespace Mvc.UtilityModels
{
    public class AvatarUtilityModel    
    {
        public bool HasAvatar { get; private set; }
        public byte[] AvatarData { get; private set; }
        public string ImageMimeType { get; private set; }

        public AvatarUtilityModel(bool hasAvatar, byte[] avatarData = null, string imageMimeType = "")
        {
            HasAvatar = hasAvatar;
            AvatarData = avatarData;
            ImageMimeType = imageMimeType;
        }
    }
}