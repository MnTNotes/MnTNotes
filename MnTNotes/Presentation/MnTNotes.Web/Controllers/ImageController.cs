using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Claims;

namespace MnTNotes.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;

        public ImageController(ILogger<ImageController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetImage()
        {
            return new JsonResult(new
            {
                uploaded = true,
                fileName = "",
                url = "",
                data = new { link = "" },
                error = new { message = "test data" }
            });
        }

        [HttpPost]
        public IActionResult PostImage(IFormFile image)
        {
            if (image.Length <= 0) return null;

            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var formattedUserId = userId.Replace("-", string.Empty);

                    string imageBaseFolder = "./ClientApp/build/images/";
                    if (!Directory.Exists(imageBaseFolder))
                        Directory.CreateDirectory(imageBaseFolder);

                    string imageUserFolder = "./ClientApp/build/images/" + formattedUserId;
                    if (!Directory.Exists(imageUserFolder))
                        Directory.CreateDirectory(imageUserFolder);

                    var imageFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName).ToLower();
                    var imagePath = imageUserFolder + "/" + imageFileName;
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }

                    var imageUrl = $"{this.Request.Scheme}://{this.Request.Host}/{this.Request.PathBase}images/{formattedUserId}/{imageFileName}";

                    _logger.LogDebug("POST: Image");

                    return new JsonResult(new
                    {
                        uploaded = true,
                        fileName = imageFileName,
                        url = imageUrl,
                        data = new { link = imageUrl },
                        error = new { message = "image is uploaded" }
                    });
                }
            }

            return new JsonResult(new { uploaded = false, error = new { message = "image is not uploaded" } });
        }
    }
}