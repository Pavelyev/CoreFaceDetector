using System;
using System.IO;
using System.Net;

namespace CoreFaceDetector.Services
{
    public class UriToStreamService
    {
        public Stream GetStream(string url)
        {
            var uri = new Uri(url);

            if (uri.Scheme == Uri.UriSchemeFile)
            {
                return File.OpenRead(uri.LocalPath);
            }

            if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
            {
                using (var client = new WebClient())
                {
                    return new MemoryStream(client.DownloadData(uri));
                }
            }

            throw new ArgumentException($"url scheme isn't supported");
        }
    }
}
