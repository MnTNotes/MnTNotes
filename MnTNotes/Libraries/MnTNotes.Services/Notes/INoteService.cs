using MnTNotes.Core.Data.Domain;
using System.Collections.Generic;

namespace MnTNotes.Services.Notes
{
    public interface INoteService
    {
        Note GetById(int noteId);

        List<Note> GetByUserId(string userId);

        List<Note> GetAll();

        void Add(Note note);

        void Update(Note note);

        void Delete(Note note);
    }
}