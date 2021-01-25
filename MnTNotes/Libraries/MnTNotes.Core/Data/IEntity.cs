using System;

namespace MnTNotes.Core.Data
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}