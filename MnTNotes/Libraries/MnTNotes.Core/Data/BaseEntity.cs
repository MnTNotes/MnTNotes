using System;

namespace MnTNotes.Core.Data
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}