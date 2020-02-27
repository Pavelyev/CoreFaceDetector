using System.Linq;
using CoreFaceDetector.Models;
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

        public FaceController(IWebHostEnvironment webHostEnvironment)
        {
            var path = webHostEnvironment.ContentRootFileProvider.GetFileInfo("Cascades/haarcascade_frontalface_default.xml").PhysicalPath;
            this.cascadeClassifier = new CascadeClassifier(path);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Detect(string uri)
        {
            var mat = new Mat(uri, ImreadModes.Grayscale);

            var faces = this.cascadeClassifier.DetectMultiScale(mat, 1.1, 3, HaarDetectionType.ScaleImage);

            if (faces.Length == 0)
            {
                return NotFound();
            }

            var face = faces.OrderByDescending(x => x.Width * x.Height).First();

            var result = new Face
            {
                Rectangle = $"{face.X},{face.Y},{face.Width},{face.Height}"
            };

            return Ok(result);
        }
    }
}
