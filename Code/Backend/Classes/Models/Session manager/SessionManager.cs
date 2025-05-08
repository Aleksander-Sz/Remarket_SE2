using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReMarket.Models
{
    public static class SessionManager
    {
        private static Account? _currentUser;

        public static bool IsLoggedIn => _currentUser != null;

        public static Account? CurrentUser => _currentUser;

        public static void Login(Account account)
        {
            _currentUser = account;
        }

        public static void Logout()
        {
            _currentUser = null;
        }
    }

}
//!!!!!!! To be finsiehd after database