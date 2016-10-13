using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrambles.Services
{
    public static class UserService
    {
        public const string SessionKeyName = "IsAdmin";

        public static bool IsAdmin()
        {
            return (bool?) HttpContext.Current.Session[SessionKeyName] == true;
        } 
    }
}