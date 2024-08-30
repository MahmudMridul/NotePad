using System.Net.Mail;
using System.Text.RegularExpressions;

namespace NotePadAPI.Utils
{
    internal class UserUtils
    {
        internal static bool ValidEmail(string email)
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
    }
}
