using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Examination.BLL.General
{
    public class General
    {
 
    }

    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);

        }
        public static bool VerifyPassword(string password, string hash) { 
        
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }

}
