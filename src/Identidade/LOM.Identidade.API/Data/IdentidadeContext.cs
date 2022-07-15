using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LOM.Identidade.API.Data
{
    public class IdentidadeContext : IdentityDbContext
    {
        public IdentidadeContext(DbContextOptions<IdentidadeContext> options) : base(options) { }
    }
}
