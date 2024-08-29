using Microsoft.EntityFrameworkCore;
using NotePadAPI.Models;

namespace NotePadAPI.Db.IDb
{
    public interface IDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Note> Notes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
