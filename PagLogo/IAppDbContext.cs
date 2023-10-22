using Microsoft.EntityFrameworkCore;
using PagLogo.Models;

namespace PagLogo
{
    public interface IAppDbContext : IDisposable
    {
        DbSet<Generic>? Generics { get; set; }
        DbSet<Tradesman>? Tradesmans { get; set; }
    }
}
