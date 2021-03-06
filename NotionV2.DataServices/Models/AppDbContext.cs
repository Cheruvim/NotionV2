using Microsoft.EntityFrameworkCore;

namespace NotionV2.DataServices.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated(); // создаем базу данных при первом обращении
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SectionOnUserLinker> LinkerSectionsOnUsers { get; set; }
    }
}