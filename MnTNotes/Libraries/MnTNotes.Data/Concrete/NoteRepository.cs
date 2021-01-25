using MnTNotes.Core.Data.Domain;
using MnTNotes.Data.Abstract;

namespace MnTNotes.Data.Concrete
{
    public class NoteRepository : EntityRepository<Note, MnTNotesDataDbContext>, INoteRepository
    {
        private MnTNotesDataDbContext _dbContext;

        public NoteRepository(MnTNotesDataDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}