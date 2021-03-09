using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Server.Models;

namespace Server.Services
{
    public class FileStore : IFileStore
    {
        private Dictionary<Guid, FileInfo> uploaded = new();
        
        private FileInfo GetFile(Platforms platform, string marketplace, string branch, long version)
        {
            var fi = new FileInfo(Path.Join("uploads", marketplace, branch, $"wde-{platform}-{version}.zip"));
            if (fi.Directory == null)
                throw new ArgumentException();
            fi.Directory.Create();
            return fi;
        }
        
        public async Task<Guid> AddFile(Platforms platform, string marketplace, string branch, long version, IFormFile file)
        {
            var physFile = GetFile(platform, marketplace, branch, version);
            await using var stream = physFile.OpenWrite();
            await file.CopyToAsync(stream);

            var id = Guid.NewGuid();
            uploaded.Add(id, physFile);
            return id;
        }

        public bool FileExists(string id)
        {
            return uploaded.ContainsKey(Guid.Parse(id));
        }

        public FileInfo GetFile(Guid id)
        {
            return uploaded[id];
        }
    }

    public interface IFileStore
    {
        Task<Guid> AddFile(Platforms platform, string marketplace, string branch, long version, IFormFile file);
        FileInfo GetFile(Guid id);
        bool FileExists(string id);
    }
}