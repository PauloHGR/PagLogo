using Microsoft.EntityFrameworkCore;
using PagLogo.Models;

namespace PagLogo
{
    public class AppDbContext : DbContext, IAppDbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<Transaction>? Transactions { get; set; }
    }
}
