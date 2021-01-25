using Microsoft.AspNetCore.Identity;

namespace MnTNotes.Core.Data.Domain.Identity
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}