using System.Linq;
using Data.Sql.Models;
using static BCrypt.Net.BCrypt;

namespace Data.Sql.Services
{
    public class PasswordService
    {
        public const string SessionKeyName = "IsAdmin";
        private readonly PiiaDb _db;

        public PasswordService(PiiaDb db)
        {
            _db = db;
        }

        public void SavePassword(string password)
        {
            var loopCount = GetLoopCount();
            var hash = HashPassword(password, loopCount);

            var dbRow = _db.Configurations.Single(x => x.ConfigurationID == ConfigurationKeys.AdminPassword);
            dbRow.ConfigurationValue = hash;
            _db.SaveChanges();
        }

        public bool VerifyPassword(string passwordToVerify)
        {
            var current = GetCurrentPasswordHash();
            return Verify(passwordToVerify, current);
        }

        private string GetCurrentPasswordHash()
        {
            return _db.Configurations
                .Where(x => x.ConfigurationID == ConfigurationKeys.AdminPassword)
                .Select(x => x.ConfigurationValue)
                .Single();
        }

        private int GetLoopCount()
        {
            var loopCountString = _db.Configurations
                .Where(x => x.ConfigurationID == ConfigurationKeys.BcryptLoopCount)
                .Select(x => x.ConfigurationValue)
                .Single();
            return int.Parse(loopCountString);
        }
    }
}
