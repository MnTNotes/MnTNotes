using MnTNotes.Core.Data.Domain.Identity;

namespace MnTNotes.Core.Data.Domain
{
    public class Note : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}