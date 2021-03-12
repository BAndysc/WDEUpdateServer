using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models.API;
using Server.Models.Database;

namespace Server.Services.Database
{
    public interface IDatabaseRepository
    {
        Task<bool> InsertFile(FileEntityModel model);
        Task<string?> GetFilePath(Guid guid);
        Task<List<(VersionEntityModel version, VersionFilesModel file)>> GetChangelog(string marketplace, string branch, long startVersion, Platforms platform);
        Task<VersionFilesModel?> GetFileForVersion(VersionEntityModel version, Platforms platform);
        Task<VersionEntityModel> GetOrCreateVersion(string marketplace, string branch, long version,
            string textVersion);

        Task<bool> ValidateUserKey(string user, string key);
        Task<bool> ValidateMarketplace(string marketplace, string? key);
        
        Task InsertVersionFile(VersionEntityModel updateVersion, Platforms requestPlatform, FileEntityModel file);
    }
}