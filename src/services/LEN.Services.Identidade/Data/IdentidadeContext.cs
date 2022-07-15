using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LEN.Services.Identidade.Data
{
    public class IdentidadeContext : IdentityDbContext
    {
        public IdentidadeContext(DbContextOptions<IdentidadeContext> options) : base(options) { }
    }
}
