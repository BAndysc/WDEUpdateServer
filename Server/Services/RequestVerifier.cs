using Server.Models;
using Server.Models.API;

namespace Server.Services
{
    public class RequestVerifier : IRequestVerifier
    {
        public bool VerifyDownloadRequest(string id, string key)
        {
            return true;
        }

        public bool VerifyCheckVersionRequest(CheckVersionRequest request, string key)
        {
            return true;
        }

        public bool VerifyUploadRequest(UploadVersionRequest request, string key)
        {
            return true;
        }

    }
}