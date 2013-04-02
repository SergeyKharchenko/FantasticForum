namespace Mvc.StructModels
{
    public class GetAvatarSM    
    {
        public bool HasAvatar { get; private set; }
        public byte[] AvatarData { get; private set; }
        public string ImageMimeType { get; private set; }

        public GetAvatarSM(bool hasAvatar, byte[] avatarData = null, string imageMimeType = "")
        {
            HasAvatar = hasAvatar;
            AvatarData = avatarData;
            ImageMimeType = imageMimeType;
        }
    }
}