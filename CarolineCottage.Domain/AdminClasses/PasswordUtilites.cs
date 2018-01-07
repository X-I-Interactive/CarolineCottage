using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CarolineCottage.Repository.CarolineCottageDatabase;

namespace CarolineCottage.Domain
{
    public class PasswordUtilityFunctions
    {
        public static PasswordStatus GetPasswordStatus(int userID, string password2Check, string connectionString)
        {
            using (CarolineCottageDbContext dbContext = new CarolineCottageDbContext(connectionString))
            {
                var user = dbContext.Users.FirstOrDefault(x => x.UserID == userID) ?? new Repository.CarolineCottageClasses.User();
                if (PasswordUtilityFunctions.CreatePasswordHashSHA1(password2Check, user.Salt) != user.PasswordEnc)
                {
                    return PasswordStatus.Valid;
                }
            }
            return PasswordStatus.Invalid;
        }

        public static PasswordStatus GetPasswordStatus(string name, string password2Check, string connectionString)
        {
            using (CarolineCottageDbContext dbContext = new CarolineCottageDbContext(connectionString))
            {
                var user = dbContext.Users.FirstOrDefault(x => x.Name == name) ?? new Repository.CarolineCottageClasses.User();
                if (PasswordUtilityFunctions.CreatePasswordHashSHA1(password2Check, user.Salt) == user.PasswordEnc)
                {
                    return PasswordStatus.Valid;
                }
            }
            return PasswordStatus.Invalid;
        }

        public static string CreatePasswordHashSHA1(string password, string salt)
        {
            string passwordAndSalt = string.Concat(password, salt);
            SHA1CryptoServiceProvider objSHA1 = new SHA1CryptoServiceProvider();
            byte[] byteBuffer = System.Text.Encoding.ASCII.GetBytes(passwordAndSalt);

            return Convert.ToBase64String(objSHA1.ComputeHash(byteBuffer));
        }

        public static string CreateSalt(int saltSize)
        {
            RNGCryptoServiceProvider objRng = new RNGCryptoServiceProvider();
            Byte[] buffer = new Byte[saltSize];

            objRng.GetBytes(buffer);

            return Convert.ToBase64String(buffer);
        }
    }

    public enum PasswordStatus
    {
        Invalid = 0,
        Valid = 1,
        Temporary = 2,
        Expired = 3
    }
}
