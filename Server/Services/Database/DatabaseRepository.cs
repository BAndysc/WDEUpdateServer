using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Models.API;
using Server.Models.Database;
using Z.EntityFramework.Plus;

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
            
            databaseContext.Database.Migrate();
        }

        public async Task<VersionFilesModel?> GetFileForVersion(VersionEntityModel version, Platforms platform)
        {
            return await databaseContext.VersionFiles.SingleOrDefaultAsync(v =>
                v.Version == version && v.Platform == platform);
        }
        
        public async Task<List<(VersionEntityModel, FileEntityModel)>> GetLatestVersion(string marketplace, string branch, long startVersion, Platforms platform)
        {
            var lst = await databaseContext.Versions
                .Join(databaseContext.VersionFiles,
                    v => new {id = v, platform = platform},
                    vf => new {id = vf.Version, platform = vf.Platform},
                    (v, vf) => new {version = v, versionFile = vf})
                .Join(databaseContext.Files,
                    vvf => vvf.versionFile.File,
                    f => f,
                    (vvf, f) => new {version = vvf.version, versionFile = vvf.versionFile, file = f})
                .Where(v => v.version.Marketplace == marketplace &&
                            v.version.Branch == branch &&
                            v.version.Version > startVersion)
                .OrderByDescending(p => p.version.Version)
                .ToListAsync();
            return lst.Select(tuple => (tuple.version, tuple.file)).ToList();
        }
        
        public async Task<List<VersionEntityModel>> GetChangelog(string marketplace, string branch, long startVersion, Platforms platform)
        {
            return await databaseContext.Versions.
                Where(v => v.Marketplace == marketplace &&
                                        v.Branch == branch &&
                                        v.Version > startVersion)
                .Include(v => v.Changes)
                .OrderByDescending(p => p.Version)
                .ToListAsync();
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
        
        public async Task<FileEntityModel?> InsertVersionFile(VersionEntityModel updateVersion, Platforms platform, FileEntityModel file)
        {
            FileEntityModel? old = null;
            var previous = await databaseContext.VersionFiles.SingleOrDefaultAsync(v => v.Version.Id == updateVersion.Id && v.Platform == platform);
            if (previous != null)
            {
                old = previous.File;
                previous.File = file;
            }
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
            return old;
        }

        public async Task<List<VersionFilesModel>> GetOldFiles(string marketplace, string branch, long version, Platforms platform)
        {
            return await databaseContext.VersionFiles
                .Where(v => v.Version.Marketplace == marketplace &&
                            v.Version.Branch == branch &&
                            v.Platform == platform &&
                            v.Version.Version <= version)
                .Include(e => e.File)
                .Include(e => e.Version)
                .ToListAsync();
        }

        public async Task<VersionEntityModel?> GetVersion(string marketplace, string branch, long version)
        {
            return await databaseContext.Versions.FirstOrDefaultAsync(v => v.Marketplace == marketplace &&
                v.Branch == branch &&
                v.Version == version);
        }
        
        public async Task<VersionEntityModel> GetOrCreateVersion(string marketplace, string branch, long version, string textVersion)
        {
            var existing = await GetVersion(marketplace, branch, version);

            if (existing != null)
                return existing;
            
            var @new = new VersionEntityModel()
            {
                Branch = branch,
                Marketplace = marketplace,
                ReleaseDate = DateTime.UtcNow,
                Version = version,
                TextVersion = textVersion
            };

            await databaseContext.Versions.AddAsync(@new);
            await databaseContext.SaveChangesAsync();
            
            return @new;
        }
        
        public async Task<bool> InsertFile(UserModel uploader, FileEntityModel model)
        {
            model.Uploader = uploader;
            model.UploadDate = DateTime.UtcNow;
            await databaseContext.Files.AddAsync(model);

            return await databaseContext.SaveChangesAsync() == 1;
        }

        public async Task<UserModel> GetUserByName(string user)
        {
            return await databaseContext.Users.SingleAsync(u => u.User == user);
        }

        public async Task RemoveFile(FileEntityModel file)
        {
            databaseContext.Files.Remove(file);
            await databaseContext.SaveChangesAsync();
        }
        
        public async Task<string?> GetFilePath(Guid guid)
        {
            var model = await databaseContext.Files.FindAsync(guid);
            return model?.Path;
        }

        public async Task AddChangelogEntry(VersionEntityModel version, string entry)
        {
            await databaseContext.ChangelogEntries.AddAsync(new ChangeLogEntryModel() {Version = version, Change = entry});
            await databaseContext.SaveChangesAsync();
        }

        public async Task<VersionEntityModel?> GetLatestVersionAnyBranch(string marketplace)
        {
            var list = await databaseContext.Versions
                .Where(v => v.Marketplace == marketplace)
                .OrderByDescending(v => v.Version)
                .Take(1)
                .ToListAsync();
            return list.Count == 0 ? null : list[0];
        }

        public async Task<StaticFileModel?> GetStaticFile(string name)
        {
            return await databaseContext.StaticFiles
                .Where(v => v.FileName == name)
                .FirstOrDefaultAsync();
        }

        public async Task AddComment(string ip, string username, string text, string userAgent)
        {
            await databaseContext.Comments.AddAsync(new CommentModel(Guid.NewGuid(), ip, username, text, userAgent, DateTime.UtcNow));
            await databaseContext.SaveChangesAsync();
        }

        private async Task Log(IPAddress? ip, string method, string userAgent, string text)
        {
            await databaseContext.Logs.AddAsync(new LogModel(ip?.ToString(), method, text, userAgent));
            await databaseContext.SaveChangesAsync();
        }

        public async Task Log(HttpRequest request, string text)
        {
            await Log(request.HttpContext.Connection.RemoteIpAddress, request.Method,request.Headers.UserAgent, text);
        }
    }
}