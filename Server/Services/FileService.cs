using System;
using System.IO;
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

        public async Task<FileEntityModel> AddFile(UserModel uploader, Platforms platform, string marketplace, string branch, long version, IFormFile file)
        {
            var path = await fileStore.AddFile(platform, marketplace, branch, version, file);
            
            var model = new FileEntityModel()
            {
                Path = path
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
    }
}