using Microsoft.EntityFrameworkCore;
using PagLogo.Models;

namespace PagLogo
{
    public interface IAppDbContext : IDisposable
    {
        DbSet<User>? Users { get; set; }
        DbSet<Transaction>? Transactions { get; set; }
        int SaveChanges();
    }
}
