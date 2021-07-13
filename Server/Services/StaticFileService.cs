using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Server.Services.Database;

namespace Server.Services
{
    public class StaticFileService : IStaticFileService
    {
        private readonly IDatabaseRepository databaseRepository;
        private FileInfo staticDirectory;
        
        public StaticFileService(IConfiguration configuration, IDatabaseRepository databaseRepository)
        {
            staticDirectory = new FileInfo(configuration["StaticFilesPath"]);
            if (staticDirectory == null)
                throw new Exception();
            staticDirectory.Directory!.Create();
            
            this.databaseRepository = databaseRepository;
        }
        
        public async Task<FileInfo?> GetFilePath(string name)
        {
            var model = await databaseRepository.GetStaticFile(name);
            if (model == null)
                return null;
            
            var fi = new FileInfo(Path.Join(staticDirectory.FullName, model.Path));
            if (fi.Directory == null)
                throw new ArgumentException();

            return fi;
        }
    }

    public interface IStaticFileService
    {
        Task<FileInfo?> GetFilePath(string name);
    }
}