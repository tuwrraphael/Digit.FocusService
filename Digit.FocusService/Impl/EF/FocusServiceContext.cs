using Microsoft.EntityFrameworkCore;

namespace Digit.FocusService.Impl.EF
{
    public class FocusServiceContext : DbContext
    {
        public FocusServiceContext(DbContextOptions<FocusServiceContext> contextOptions) : base(contextOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
