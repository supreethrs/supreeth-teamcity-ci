using CoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Helpers
{
    public static class Extensions
    {
        public static User UserWithoutPassword(this User user)
        {
            if (user != null)
                user.Password = null;
            return user;
        }

        public static IEnumerable<User> UsersWithoutPassword(this IEnumerable<User> users)
        {
            return users.Select(x => x.UserWithoutPassword());
        }

    }
}
