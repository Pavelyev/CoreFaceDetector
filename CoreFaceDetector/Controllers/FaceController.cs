using System;
using System.IO;
using System.Linq;
using CoreFaceDetector.Models;
using CoreFaceDetector.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using OpenCvSharp;

namespace CoreFaceDetector.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaceController : ControllerBase
    {
        private readonly CascadeClassifier cascadeClassifier;
        private readonly UrlToStreamService urlToStreamService;
        private readonly ImageResizer imageResizer;

        public FaceController(IWebHostEnvironment webHostEnvironment, UrlToStreamService urlToStreamService)
        {
            var path = webHostEnvironment.ContentRootFileProvider.GetFileInfo("Cascades/haarcascade_frontalface_default.xml").PhysicalPath;
            this.cascadeClassifier = new CascadeClassifier(path);
            this.urlToStreamService = urlToStreamService;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Detect(string url)
        {
            var stream = this.urlToStreamService.GetStream(url);

            var resclaledStreamResult = this.imageResizer.Rescale(stream, 500);

            var mat = Mat.FromStream(resclaledStreamResult.Stream, ImreadModes.Grayscale);

            var faces = this.cascadeClassifier.DetectMultiScale(mat, 1.1, 3, HaarDetectionType.ScaleImage);

            if (faces.Length == 0)
            {
                return NotFound();
            }

            var face = faces.OrderByDescending(x => x.Width * x.Height).First();

            var resultVector = new[] { face.X, face.Y, face.Width, face.Height };

            resultVector = resultVector.Select(x => (int)(x / resclaledStreamResult.ScaleFactor)).ToArray();

            var result = new Face
            {
                Rectangle = $"{resultVector[0]},{resultVector[1]},{resultVector[2]},{resultVector[3]}"
            };

            return Ok(result);
        }
    }
}
