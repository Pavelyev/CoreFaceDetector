using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoreFaceDetector.Services
{
    public class UrlToStreamService
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
