using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public class MainUser : IdentityUser
    {
        public string Name { get; set; }
        public string FamilyName { get; set; }

        public byte[] ProfilePicture { get; set; }
    }
}
