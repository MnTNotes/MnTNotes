using Microsoft.AspNetCore.Identity;

namespace MnTNotes.Core.Data.Domain.Identity
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public string Discriminator { get; set; }

        //public virtual ApplicationUser User { get; set; }
    }
}