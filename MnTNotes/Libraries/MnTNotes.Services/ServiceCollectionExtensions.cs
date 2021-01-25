using Microsoft.Extensions.DependencyInjection;
using MnTNotes.Data;
using MnTNotes.Services.Notes;

namespace MnTNotes.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServicesDependencies(this IServiceCollection services)
        {
            services.AddDataDependencies();
            services.AddScoped<INoteService, NoteService>();
        }
    }
}