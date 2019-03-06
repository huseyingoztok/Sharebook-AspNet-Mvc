using ShareBook.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.DAL.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public DbSet<AdminLogs> AdminLogs { get; set; }
        public DbSet<Category> Category { get; set; }

        public DbSet<Comment> Comment { get; set; }
        public DbSet<Liked> Liked { get; set; }
        public DbSet<ShareBookUser> ShareBookUser { get; set; }
        public DbSet<Sharing> Sharing { get; set; }
        public DbSet<SiteLog> SiteLog { get; set; }

        public DbSet<Slider> Slider { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new myInitializer());
        }
    }
}
