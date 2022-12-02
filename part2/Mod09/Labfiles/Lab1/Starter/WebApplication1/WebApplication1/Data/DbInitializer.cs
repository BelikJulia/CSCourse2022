using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace WebApplication1.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Database.EnsureCreated())
                Seed(context);
        }

        private static void Seed(ApplicationDbContext context)
        {
            List<IdentityUser> userList = new List<IdentityUser>
            {
                new IdentityUser("JonDue") { Email = "jond@outlook.com", PasswordHash = Convert.ToBase64String(Encoding.ASCII.GetBytes("password1234"))},
            };
            context.Users.AddRange(userList);
            context.SaveChanges();
        }
    }
}
