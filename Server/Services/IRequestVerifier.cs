using Server.Models;

namespace Server.Services
{
    public interface IRequestVerifier
    {
        bool VerifyUploadRequest(UploadVersionRequest request, string key);

        bool VerifyDownloadRequest(string id, string key);
    }
}