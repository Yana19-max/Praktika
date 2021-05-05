using Praktikaaa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Praktikaaa
{
    public class SampleData
    {
        public static void Initialize(UsersContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Email = "Admin",
                        Password = "ADMIN",
                    },
                    new User
                    {
                        Email = "Moder",
                        Password = "MODER",
                    });
                context.SaveChanges();
            }

        }
    }
}
