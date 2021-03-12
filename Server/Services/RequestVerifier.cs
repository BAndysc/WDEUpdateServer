using System.Threading.Tasks;
using Server.Models;
using Server.Models.API;
using Server.Services.Database;

namespace Server.Services
{
    public class RequestVerifier : IRequestVerifier
    {
        private readonly IDatabaseRepository databaseRepository;

        public RequestVerifier(IDatabaseRepository databaseRepository)
        {
            this.databaseRepository = databaseRepository;
        }
        
        public async Task<bool> VerifyDownloadRequest(string id, string key)
        {
            return true;
        }

        public async Task<bool> VerifyCheckVersionRequest(CheckVersionRequest request, string key)
        {
            return await databaseRepository.ValidateMarketplace(request.Marketplace, key);
        }

        public async Task<bool> VerifyUploadRequest(UploadVersionRequest request, string user, string key)
        {
            return await databaseRepository.ValidateUserKey(user, key);
        }
    }
}