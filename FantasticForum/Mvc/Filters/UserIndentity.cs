using System.Security.Principal;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.DAL.Abstract;

namespace Mvc.Filters
{
    public class UserIndentity : IIdentity, IPrincipal
    {
        public User User { get; set; }

        public string Name
        {
            get
            {
                return User != null ? User.Email : "anonym";
            }
        }

        public string AuthenticationType
        {
            get { return typeof (User).ToString(); }
        }

        public bool IsAuthenticated
        {
            get { return User != null; }
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        public IIdentity Identity { get { return this; } }
    }
}