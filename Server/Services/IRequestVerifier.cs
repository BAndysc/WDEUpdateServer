using System.Threading.Tasks;
using Server.Models.API;

namespace Server.Services
{
    public interface IRequestVerifier
    {
        Task<bool> VerifyUploadRequest(UploadVersionRequest request, string user, string key);
        Task<bool> VerifyDownloadRequest(string id, string? key);
        Task<bool> VerifyCheckVersionRequest(CheckVersionRequest request, string? key);
    }
}