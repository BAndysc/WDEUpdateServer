using Server.Models;
using Server.Models.API;

namespace Server.Services
{
    public interface IRequestVerifier
    {
        bool VerifyUploadRequest(UploadVersionRequest request, string key);
        bool VerifyDownloadRequest(string id, string key);
        bool VerifyCheckVersionRequest(CheckVersionRequest request, string key);
    }
}