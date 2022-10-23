using Microsoft.EntityFrameworkCore;

namespace task.Models
{
    public class FolderContext : DbContext
    {
        //public FolderContext() => Database.EnsureCreated();
        public DbSet<FolderModel> Folders { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }
    }
}
