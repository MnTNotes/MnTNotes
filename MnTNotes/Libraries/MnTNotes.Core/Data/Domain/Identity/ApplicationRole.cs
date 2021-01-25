using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MnTNotes.Core.Data.Domain.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
}