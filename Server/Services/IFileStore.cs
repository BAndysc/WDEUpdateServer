using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Server.Models.API;

namespace Server.Services
{
    public interface IFileStore
    {
        Task<string> AddFile(Platforms platform, string marketplace, string branch, long version, IFormFile file, string[]? pathsToMakeExecutable);
        FileInfo GetFile(string id);
        bool FileExists(string id);
        Task RemoveFile(string path);
    }
}