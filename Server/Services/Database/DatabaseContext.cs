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
        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<MarketplaceModel> Marketplaces => Set<MarketplaceModel>();
        public DbSet<ChangeLogEntryModel> ChangelogEntries => Set<ChangeLogEntryModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VersionEntityModel>()
                .HasIndex(nameof(VersionEntityModel.Marketplace));
            modelBuilder.Entity<VersionEntityModel>()
                .HasIndex(nameof(VersionEntityModel.Branch));
            modelBuilder.Entity<VersionEntityModel>()
                .HasIndex(nameof(VersionEntityModel.Version));
            
            modelBuilder
                .Entity<VersionFilesModel>()
                .HasOne(e => e.Version)
                .WithMany(e => e.Files)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<VersionEntityModel>()
                .HasMany<ChangeLogEntryModel>(e => e.Changes)
                .WithOne(e => e.Version)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder
                .Entity<VersionFilesModel>()
                .HasOne(e => e.File)
                .WithMany(e => e.ReferencedVersions)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}