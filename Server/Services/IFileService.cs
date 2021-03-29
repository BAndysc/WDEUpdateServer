using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Server.Models.API;
using Server.Models.Database;

namespace Server.Services
{
    public interface IFileService
    {
        Task<FileEntityModel> AddFile(UserModel uploader, Platforms platform, string marketplace, string branch, long version, IFormFile file, string[]? pathsToMakeExecutable);
        Task<FileInfo?> GetFile(Guid guid);
        Task<bool> FileExists(Guid guid);
        Task RemoveFile(FileEntityModel oldFile);
    }
}