using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MnTNotes.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Note> Notes { get; set; }
    }
}
