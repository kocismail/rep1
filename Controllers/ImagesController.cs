using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DeepHist.Business.Abstract;
using DeepHist.Entity.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeepHist.Controllers
{
    [Route("api/images")]

    public class ImagesController : Controller
    {
        private IImageService _imageService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImagesController(IImageService imageService, IWebHostEnvironment hostEnvironment)
        {
            _imageService = imageService;
            webHostEnvironment = hostEnvironment;
        }

        [HttpPost("Upload1"), DisableRequestSizeLimit]
        public IActionResult UploadFile(IFormFile file)
        {
            try
            {
                //   var file = Request.Form.Files[0];

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("Upload")]
        public IActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        private string UploadedFile()
        {
            string uniqueFileName = null;

            Image image = new Image();

            string uploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FormFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.FormFile.CopyTo(fileStream);
            }


            return uniqueFileName;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> GetValues()
        {
            var values = await _imageService.GetList();
            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{projectId}")]
        public async Task<ActionResult> GetByCategory(int projectId)
        {
            var values = await _imageService.GetByProject(projectId);
            return Ok(values);
        }

        [HttpPost("Save")]
        public async Task<ActionResult> SaveImage([FromRoute] IFormFile file)
        {
            UploadedFile();
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(file.OpenReadStream()))
            {
                bytes = br.ReadBytes((int)file.Length);
            }

            Image image = new Image();
            image.DataContent = bytes;
            image.ImageName = image.UserId.ToString() + "_" + DateTime.Now.ToLongDateString() + "_" + DateTime.Now.ToLongTimeString();
            await _imageService.Add(image);

            return Ok(image);

        }


        [HttpPost("Save1")]
        public async Task<ActionResult> SaveImage1([FromServices] IFormFile file)
        {
            UploadedFile();
            byte[] bytes;

            using (BinaryReader br = new BinaryReader(file.OpenReadStream()))
            {
                bytes = br.ReadBytes((int)file.Length);
            }

            Image image = new Image();
            image.DataContent = bytes;
            image.ImageName = image.UserId.ToString() + "_" + DateTime.Now.ToLongDateString() + "_" + DateTime.Now.ToLongTimeString();
            await _imageService.Add(image);

            return Ok(image);

        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
