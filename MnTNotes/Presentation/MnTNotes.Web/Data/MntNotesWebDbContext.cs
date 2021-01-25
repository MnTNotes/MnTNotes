using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MnTNotes.Core.Data;

namespace MnTNotes.Web.Data
{
    public class MntNotesWebDbContext : BaseApiAuthorizationDbContext
    {
        public MntNotesWebDbContext(
           DbContextOptions options,
           IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}