using MnTNotes.Core.Data.Domain;
using MnTNotes.Data;
using System.Collections.Generic;

namespace MnTNotes.Services.Notes
{
    public class NoteService : INoteService
    {
        private readonly IRepository<Note> _noteRepository;

        public NoteService(IRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public Note GetById(int noteId)
        {
            return _noteRepository.Get(x => x.Id == noteId);
        }

        public List<Note> GetByUserId(string userId)
        {
            return _noteRepository.GetList(x => x.UserId == userId);
        }

        public List<Note> GetAll()
        {
            return _noteRepository.GetList();
        }

        public void Add(Note note)
        {
            _noteRepository.Insert(note);
        }

        public void Update(Note note)
        {
            _noteRepository.Update(note);
        }

        public void Delete(Note note)
        {
            _noteRepository.Delete(note);
        }
    }
}