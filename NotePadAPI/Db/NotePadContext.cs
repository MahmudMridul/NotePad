using Microsoft.EntityFrameworkCore;
using NotePadAPI.Db.IDb;
using NotePadAPI.Models;

namespace NotePadAPI.Db
{
    public class NotePadContext : DbContext, IDbContext
    {
        public NotePadContext(DbContextOptions<NotePadContext> options) : base(options) { }
        
        public DbSet<User> Users {  get; set; } 
        public DbSet<Note> Notes { get; set; }
    }
}
