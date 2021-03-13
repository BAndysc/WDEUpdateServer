using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Server.Models.API;

namespace Server.Services
{
    public class FileStore : IFileStore
    {
        private FileInfo uploadDictionary;
        
        public FileStore(IConfiguration configuration)
        {
            uploadDictionary = new FileInfo(configuration["UploadPath"]);
            if (uploadDictionary == null)
                throw new Exception();
            uploadDictionary.Directory!.Create();
        }
        
        private (FileInfo, string) GetFile(Platforms platform, string marketplace, string branch, long version)
        {
            var filePath = Path.Join(marketplace, branch, $"wde-{platform}-{version}.zip");
            var fi = new FileInfo(Path.Join(uploadDictionary.FullName, filePath));
            if (fi.Directory == null)
                throw new ArgumentException();
            fi.Directory.Create();
            return (fi, filePath);
        }
        
        public async Task<string> AddFile(Platforms platform, string marketplace, string branch, long version, IFormFile file)
        {
            var (physFile, filePath) = GetFile(platform, marketplace, branch, version);
            await using var stream = physFile.OpenWrite();
            await file.CopyToAsync(stream);
            return filePath;
        }

        public bool FileExists(string path)
        {
            return GetFile(path).Exists;
        }

        public Task RemoveFile(string path)
        {
            var file = GetFile(path);
            if (file.Exists)
                file.Delete();
            return Task.CompletedTask;
        }
        
        public FileInfo GetFile(string path)
        {
            return new FileInfo(Path.Join(uploadDictionary.FullName, path));
        }
    }
}