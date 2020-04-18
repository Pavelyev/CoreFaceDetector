using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CoreFaceDetector.Services
{
    public class ImageResizer
    {
        public ResclaledStreamResult Rescale(Stream imageStream, int max)
        {
            using (Image image = Image.FromStream(imageStream))
            {
                var maxSide = Math.Max(image.Width, image.Height);

                var scaleFactor = (double)max / maxSide;

                if (scaleFactor >= 1)
                {
                    imageStream.Seek(0, SeekOrigin.Begin);
                    return new ResclaledStreamResult
                    {
                        ScaleFactor = 1,
                        Stream = imageStream
                    };
                }

                var rescaledStream = Rescale(image, scaleFactor);
                return new ResclaledStreamResult
                {
                    ScaleFactor = scaleFactor,
                    Stream = rescaledStream
                };
            }
        }

        public Stream Rescale(Stream imageStream, double scaleFactor)
        {
            using (Image image = Image.FromStream(imageStream))
            {
                return Rescale(image, scaleFactor);
            }
        }

        public Stream Rescale(Image image, double scaleFactor)
        {
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);

            using (Bitmap rescaledImage = new Bitmap(image, new Size(newWidth, newHeight)))
            {
                var resultStream = new MemoryStream();
                rescaledImage.Save(resultStream, ImageFormat.Jpeg);
                resultStream.Seek(0, SeekOrigin.Begin);
                return resultStream;
            }
        }

        public class ResclaledStreamResult
        {
            public Stream Stream { get; set; }

            public double ScaleFactor { get; set; }
        }
    }
}
