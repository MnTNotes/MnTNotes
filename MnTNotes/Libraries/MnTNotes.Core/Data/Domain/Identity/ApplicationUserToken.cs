using Microsoft.AspNetCore.Identity;

namespace MnTNotes.Core.Data.Domain.Identity
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}