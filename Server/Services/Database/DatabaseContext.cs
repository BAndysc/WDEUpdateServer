using Microsoft.EntityFrameworkCore;
using Server.Models.Database;

namespace Server.Services.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions) {}

        public DbSet<FileEntityModel> Files => Set<FileEntityModel>();
        public DbSet<VersionEntityModel> Versions => Set<VersionEntityModel>();
        public DbSet<VersionFilesModel> VersionFiles => Set<VersionFilesModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VersionEntityModel>()
                .HasIndex(nameof(VersionEntityModel.Marketplace));
            modelBuilder.Entity<VersionEntityModel>()
                .HasIndex(nameof(VersionEntityModel.Branch));
            modelBuilder.Entity<VersionEntityModel>()
                .HasIndex(nameof(VersionEntityModel.Version));
            
            
            //modelBuilder.Entity<VersionFilesModel>()
            //     .HasKey(nameof(VersionFilesModel.Version), nameof(VersionFilesModel.File));
        }
    }
}