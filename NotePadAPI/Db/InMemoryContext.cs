using Microsoft.EntityFrameworkCore;
using NotePadAPI.Db.IDb;
using NotePadAPI.Models;

namespace NotePadAPI.Db
{
    public class InMemoryContext : DbContext, IDbContext
    {
        public InMemoryContext(DbContextOptions<InMemoryContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}
