using System.Threading.Tasks;
using Server.Models.API;

namespace Server.Services
{
    public interface IRequestVerifier
    {
        Task<bool> VerifyUploadRequest(UploadVersionRequest request, Authentication user);
        Task<bool> VerifyDownloadRequest(string id, string? key);
        Task<bool> VerifyCheckVersionRequest(CheckVersionRequest request, string? key);
        Task<bool> VerifyModifyChangelogRequest(Authentication user);
    }
}