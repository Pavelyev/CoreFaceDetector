using System.IO;

namespace CoreFaceDetector.Services
{
    public class ResclaledStreamResult
    {
        public Stream Stream { get; set; }

        public double ScaleFactor { get; set; }
    }
}