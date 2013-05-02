using Models;

namespace Mvc.UtilityModels
{
    public class AuthorizeUtilityModel
    {
        public bool IsAuthorized { get; set; }
        public User User { get; set; }
    }
}