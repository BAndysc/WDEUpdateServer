using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Server.Models.API;
using Server.Models.Database;
using Server.Services.Database;

namespace Server.Services
{
    public class FileService : IFileService
    {
        private readonly IDatabaseRepository databaseRepository;
        private readonly IFileStore fileStore;

        public FileService(IDatabaseRepository databaseRepository, IFileStore fileStore)
        {
            this.databaseRepository = databaseRepository;
            this.fileStore = fileStore;
        }

        public async Task<FileEntityModel> AddFile(UserModel uploader, Platforms platform, string marketplace, string branch, long version, IFormFile file, string[]? pathsToMakeExecutable)
        {
            var path = await fileStore.AddFile(platform, marketplace, branch, version, file, pathsToMakeExecutable);
            var physicalFile = fileStore.GetFile(path);
            var hash = await CalculateMd5(physicalFile).ConfigureAwait(false);

            var model = new FileEntityModel()
            {
                Path = path,
                HashMd5 = hash
            };
            
            await databaseRepository.InsertFile(uploader, model);
            
            return model;
        }

        public async Task<FileInfo?> GetFile(Guid guid)
        {
            var path= await GetPath(guid);
            if (path == null)
                return null;

            return fileStore.GetFile(path);
        }

        public async Task<bool> FileExists(Guid guid)
        {
            var path= await GetPath(guid);
            if (path == null)
                return false;

            return fileStore.FileExists(path);
        }

        private async Task<string?> GetPath(Guid guid)
        {
            return await databaseRepository.GetFilePath(guid);
        }

        public async Task RemoveFile(FileEntityModel oldFile)
        {
            await fileStore.RemoveFile(oldFile.Path);
            await databaseRepository.RemoveFile(oldFile);
        }

        public Task<string> CalculateMd5(FileInfo file)
        {
            return Task.Run(() =>
            {
                using var md5 = HashAlgorithm.Create("MD5")!;
                using var stream = File.OpenRead(file.FullName);
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-","").ToLower();
            });
        }
    }
}