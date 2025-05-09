using ReMarket.Models;
using ReMarket.Utilities;

using MySql.Data.MySqlClient;
using System.Data;


namespace ReMarket.Services
{
    public static class AccountRepository
    {
        private static List<Account> _accounts = new List<Account>();

        public static void Add(Account account) => _accounts.Add(account);

        public static Account? GetByEmail(string email) =>
            _accounts.FirstOrDefault(a => a.Email.Value == email);
    }
}