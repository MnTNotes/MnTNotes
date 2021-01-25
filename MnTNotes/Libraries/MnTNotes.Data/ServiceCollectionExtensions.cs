using Microsoft.Extensions.DependencyInjection;
using MnTNotes.Core.Data.Domain;
using MnTNotes.Data.Abstract;
using MnTNotes.Data.Concrete;

namespace MnTNotes.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataDependencies(this IServiceCollection services)
        {
            services.AddDbContext<MnTNotesDataDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<,>));
            services.AddScoped(typeof(IRepository<Note>), typeof(EntityRepository<Note, MnTNotesDataDbContext>));
            services.AddScoped<INoteRepository, NoteRepository>();
        }
    }
}