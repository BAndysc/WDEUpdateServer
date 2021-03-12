using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Models.API;
using Server.Models.Database;

namespace Server.Services.Database
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly IServiceScope scope;
        private readonly DatabaseContext databaseContext;

        public DatabaseRepository(IServiceProvider serviceProvider)
        {
            scope = serviceProvider.CreateScope();
            databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        }

        public async Task<VersionFilesModel?> GetFileForVersion(VersionEntityModel version, Platforms platform)
        {
            return await databaseContext.VersionFiles.SingleOrDefaultAsync(v =>
                v.Version == version && v.Platform == platform);
        }
        
        public async Task<List<(VersionEntityModel version, VersionFilesModel file)>> GetChangelog(string marketplace, string branch, long startVersion, Platforms platform)
        {
            var lst = await databaseContext.Versions.Where(v => v.Marketplace == marketplace &&
                                                                v.Branch == branch &&
                                                                v.Version > startVersion)
                .Join(databaseContext.VersionFiles,
                    p => new {version = p.Id, platform = platform},
                    f => new {version = f.Version.Id, platform = f.Platform},
                    (version, file) => new {version, file})
                .OrderByDescending(p => p.version.Version).ToListAsync();
            return lst.Select(a => (a.version, a.file)).ToList();
        }

        public async Task<bool> ValidateUserKey(string username, string key)
        {
            var user = await databaseContext.Users.SingleOrDefaultAsync(v => v.User == username);

            if (user == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(key, user.KeyHash);
        }
        
        public async Task<bool> ValidateMarketplace(string marketplaceName, string? key)
        {
            var marketplace = await databaseContext.Marketplaces.SingleOrDefaultAsync(v => v.Name == marketplaceName);

            if (marketplace == null)
                return false;

            return marketplace.Key == null || BCrypt.Net.BCrypt.Verify(key, marketplace.Key);
        }
        
        public async Task InsertVersionFile(VersionEntityModel updateVersion, Platforms platform, FileEntityModel file)
        {
            var previous = await databaseContext.VersionFiles.SingleOrDefaultAsync(v => v.Version == updateVersion && v.Platform == platform);
            if (previous != null)
                previous.File = file;
            else
            {
                await databaseContext.VersionFiles.AddAsync(new VersionFilesModel()
                {
                    Version = updateVersion,
                    File = file,
                    Platform = platform
                });
            }
            await databaseContext.SaveChangesAsync();
        }
        
        public async Task<VersionEntityModel> GetOrCreateVersion(string marketplace, string branch, long version, string textVersion)
        {
            var existing = await databaseContext.Versions.FirstOrDefaultAsync(v => v.Marketplace == marketplace &&
                v.Branch == branch &&
                v.Version == version);

            if (existing != null)
                return existing;

            var @new = new VersionEntityModel()
            {
                Branch = branch,
                Marketplace = marketplace,
                ReleaseDate = DateTime.Now,
                Version = version,
                TextVersion = textVersion
            };

            await databaseContext.Versions.AddAsync(@new);
            await databaseContext.SaveChangesAsync();
            
            return @new;
        }
        
        public async Task<bool> InsertFile(FileEntityModel model)
        {
            await databaseContext.Files.AddAsync(model);

            return await databaseContext.SaveChangesAsync() == 1;
        }

        public async Task<string?> GetFilePath(Guid guid)
        {
            var model = await databaseContext.Files.FindAsync(guid);
            return model?.Path;
        }
    }
}