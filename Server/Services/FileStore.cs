using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Server.Models.API;
using Server.Services.Database;

namespace Server.Services
{
    public class FileStore : IFileStore
    {
        private FileInfo GetFile(Platforms platform, string marketplace, string branch, long version)
        {
            var fi = new FileInfo(Path.Join("uploads", marketplace, branch, $"wde-{platform}-{version}.zip"));
            if (fi.Directory == null)
                throw new ArgumentException();
            fi.Directory.Create();
            return fi;
        }
        
        public async Task<string> AddFile(Platforms platform, string marketplace, string branch, long version, IFormFile file)
        {
            var physFile = GetFile(platform, marketplace, branch, version);
            await using var stream = physFile.OpenWrite();
            await file.CopyToAsync(stream);
            return physFile.FullName;
        }

        public bool FileExists(string path)
        {
            return GetFile(path).Exists;
        }

        public FileInfo GetFile(string path)
        {
            return new FileInfo(path);
        }
    }
}