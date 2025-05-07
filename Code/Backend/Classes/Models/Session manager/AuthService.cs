/*using ReMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReMarket.Models
{
    public class AuthService
    {
        private readonly Database _database;

        public AuthService(Database database)
        {
            _database = database;
        }

        public bool Register(string username, string password)
        {
            if (_database.UsernameExists(username))
                return false;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var account = new Account { Username = username, Password = hashedPassword };
            return _database.InsertAccount(account);
        }

        public bool Login(string username, string password)
        {
            var account = _database.GetAccountByUsername(username);
            if (account == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(password, account.Password))
                return false;

            SessionManager.Login(account);
            return true;
        }
    }

}*/


//!!!!!!! To be finsiehd after database