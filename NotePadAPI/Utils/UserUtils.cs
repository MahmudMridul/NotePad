using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NotePadAPI.Utils
{
    internal class UserUtils
    {
        internal static bool IsValidEmail(string email)
        {
            if(string.IsNullOrEmpty(email)) return false;
            if(string.IsNullOrWhiteSpace(email)) return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if(!regex.IsMatch(email)) return false;

            try
            {
                MailAddress mail = new MailAddress(email);
                // No exception thrown. So valid email.
                return true;
            }
            catch (Exception e) 
            { 
                return false;
            }
        }

        internal static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            if (string.IsNullOrWhiteSpace(password)) return false;

            // min 6 characters, min 1 upper case, min 1 lower case, min 1 number
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(password);
        }

        internal static (byte[] Salt, string Hash) GetPasswordSaltAndHash(string password)
        {
            byte[] salt = GenerateSalt();
            string hash = HashPassword(password, salt);
            return ( salt, hash );
        }

        internal static byte[] GenerateSalt(int size = 16)
        {
            byte[] salt = new byte[size];
            try
            {
                RandomNumberGenerator.Fill(salt);
                return salt;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return salt;
        }

        internal static string HashPassword(string password, byte[] salt)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];

                    Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                    Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                    byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

                    byte[] hashedPassWithSalt = new byte[hashedBytes.Length + salt.Length];
                    Buffer.BlockCopy(salt, 0, hashedPassWithSalt, 0, salt.Length);
                    Buffer.BlockCopy(hashedBytes, 0, hashedPassWithSalt, salt.Length, hashedBytes.Length);

                    return Convert.ToBase64String(hashedPassWithSalt);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }

        internal static bool IsPasswordCorrect(string password, byte[] salt, string savedHash)
        {
            string newHash = HashPassword(password, salt);
            return newHash == savedHash;
        }

        internal static string GetToken(string userName, IConfiguration config)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(config["Jwt:Key"]);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userName)
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(20),
                Issuer = config["Jwt:Issuer"],
                Audience = config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
