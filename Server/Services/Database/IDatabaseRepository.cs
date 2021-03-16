using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models.API;
using Server.Models.Database;

namespace Server.Services.Database
{
    public interface IDatabaseRepository
    {
        Task<bool> InsertFile(UserModel uploader, FileEntityModel model);
        Task<string?> GetFilePath(Guid guid);
        Task<VersionEntityModel?> GetLatestVersionAnyBranch(string marketplace);
        Task<List<(VersionEntityModel version, FileEntityModel file)>> GetLatestVersion(string marketplace, string branch, long startVersion, Platforms platform);
        Task<List<VersionEntityModel>> GetChangelog(string marketplace, string branch, long startVersion, Platforms platform);
        Task<VersionFilesModel?> GetFileForVersion(VersionEntityModel version, Platforms platform);
        Task<VersionEntityModel?> GetVersion(string marketplace, string branch, long version);
        Task<VersionEntityModel> GetOrCreateVersion(string marketplace, string branch, long version,
            string textVersion);

        Task<UserModel> GetUserByName(string user);
        Task<bool> ValidateUserKey(string user, string key);
        Task<bool> ValidateMarketplace(string marketplace, string? key);
        
        Task<FileEntityModel?> InsertVersionFile(VersionEntityModel updateVersion, Platforms requestPlatform, FileEntityModel file);
        Task<List<VersionFilesModel>> GetOldFiles(string marketplace, string branch, long requestVersion, Platforms platform);
        Task RemoveFile(FileEntityModel file);
        Task AddChangelogEntry(VersionEntityModel version, string entry);
    }
}