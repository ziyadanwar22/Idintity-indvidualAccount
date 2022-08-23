using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_IndvidualAccount.Models
{
    public class UserRolesVM
    {
        public UserRolesVM()
        {
            userRoles = new List<string>();
        }
        public IdentityUser user { get; set; }

        public List<string> userRoles { get; set; }
    }
}
