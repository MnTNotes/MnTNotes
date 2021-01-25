using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MnTNotes.Core.Data.Domain.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}