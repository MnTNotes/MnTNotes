using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MnTNotes.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
