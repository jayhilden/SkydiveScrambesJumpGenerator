using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Data.Sql;
using Data.Sql.Models;
using Data.Sql.Services;

namespace PasswordHasher
{
    internal class Program
    {
        private static readonly PasswordService PasswordService = new PasswordService(new PiiaDb());
        private static void Main(string[] args)
        {
            Console.WriteLine("This program is used to set the admin password");
            Console.WriteLine("Enter in the new password now");
            var password = GetPasswordLoop();
            PasswordService.SavePassword(password);
            Console.WriteLine("Password successfully hashed and saved.  Press any key to exit");
            Console.ReadKey();
        }



        /// <summary>
        /// loop through until they put in the same password twice
        /// </summary>
        /// <returns></returns>
        private static string GetPasswordLoop()
        {
            while (true)
            {
                var password1 = GetPassword();
                Console.WriteLine("\nVerify Password");
                var password2 = GetPassword();
                if (password1 == password2)
                {
                    return password1;
                }
                Console.WriteLine("\nPasswords did not match");
            }
        }

        public static string GetPassword()
        {
            var password = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Remove(password.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password.Append(i.KeyChar);
                    Console.Write("*");
                }
            }
            return password.ToString();
        }
    }
}
