using Microsoft.AspNetCore.Identity;

namespace MnTNotes.Core.Data.Domain.Identity
{
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}