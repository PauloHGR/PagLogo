using Microsoft.EntityFrameworkCore;
using PagLogo.Models;

namespace PagLogo
{
    public interface IAppDbContext : IDisposable
    {
        DbSet<User>? Users { get; set; }
        int SaveChanges();
    }
}
