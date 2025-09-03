using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Photo;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Esf;

namespace api.Controllers
{   [Route("photo")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoRepository _photoRepo;
        private readonly ITagRepository _tagRepo;
        private readonly ILogger<PhotoController> _logger;
        public PhotoController(IPhotoRepository photoRepo, ITagRepository tagRepo, ILogger<PhotoController> logger)
        {
            _photoRepo = photoRepo;
            _tagRepo = tagRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var photos = (await _photoRepo.GetFilteredAsync(new PhotoFilterDTO { sortBy = "id", isDescending = true })).Select(x => x.ToPhotoDTO());

            return Ok(photos);
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetFiltered([FromBody] PhotoFilterDTO filterDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filteredPhotos = await _photoRepo.GetFilteredAsync(filterDTO);
            return Ok(filteredPhotos.Select(x => x.ToPhotoDTO()).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var photo = await _photoRepo.GetByIdAsync(id);

            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo.ToPhotoDTO());
        }
        [HttpGet("image/{id:int}")]
        public async Task<IActionResult> GetImageById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imagepath = await _photoRepo.GetImageByIdAsync(id);

            if (imagepath == null)
            {
                return NotFound();
            }

            var image = await System.IO.File.ReadAllBytesAsync(imagepath);

            return File(image, "image/jpeg");
        }
        [HttpGet("previewimage/{id:int}")]
        public async Task<IActionResult> GetPreviewImageById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var previewimagepath = await _photoRepo.GetPreviewImageByIdAsync(id);

            if (previewimagepath == null)
            {
                return NotFound();
            }

            var image = await System.IO.File.ReadAllBytesAsync(previewimagepath);

            return File(image, "image/jpeg");
        }
        [HttpGet("atuimage/{name}")]
        public async Task<IActionResult> GetATUImage([FromRoute] string name)
        {
            var safeName = Path.GetFileName(name);

            if (!safeName.EndsWith(".jpg") && !safeName.EndsWith(".jpeg"))
            {
                _logger.LogInformation("not found!!!");
                return NotFound();
            }

            string atuImagePath = Path.Combine(Directory.GetCurrentDirectory(), "atuphotos", $"{safeName}");

            _logger.LogInformation("path: " + atuImagePath);

            if (!System.IO.File.Exists(atuImagePath))
            {
                _logger.LogInformation("not found!!!");
                return NotFound();
            }

            return File(await System.IO.File.ReadAllBytesAsync(atuImagePath), "image/jpeg");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePhotoReqestDTO photoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[]? imagebytes = null;
            string imagepath = "";

            if (photoDTO.image != null && photoDTO.image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await photoDTO.image.CopyToAsync(ms);
                    imagebytes = ms.ToArray();
                    imagepath = Path.Combine(Directory.GetCurrentDirectory(), "photos", $"tempphoto.jpg");
                    await System.IO.File.WriteAllBytesAsync(imagepath, imagebytes);
                };
            }

            var photoModel = new Photo
            {
                path = imagepath,
                caption = photoDTO.caption,
                description = photoDTO.description ?? "",
                tags = photoDTO.tags.Select(x => new PhotoTag { Tagid = x }).ToList()
            };

            foreach (int tagid in photoModel.tags.Select(x => x.Tagid))
            {
                if (!await _tagRepo.TagExists(tagid))
                {
                    return BadRequest("Some of the specified tags do not exist.");
                }
            }

            await _photoRepo.CreateAsync(photoModel);
            var savedPhoto = await _photoRepo.GetByIdAsync(photoModel.id);

            if (savedPhoto == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetById), new { id = photoModel.id }, savedPhoto.ToPhotoDTO());
        }

        [Authorize(Roles="Admin")]
        [HttpPut("{id:int}")]

        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdatePhotoRequestDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var photoModel = await _photoRepo.UpdateAsync(id, updateDTO);

            if (photoModel == null)
            {
                return NotFound("Photo not found.");
            }

            return Ok(photoModel.ToPhotoDTO());
        }

        [Authorize(Roles="Admin")]
        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var photoModel = await _photoRepo.DeleteAsync(id);

            if (photoModel == null)
            {
                return NotFound("Photo not found.");
            }

            return NoContent();
        }
    }
}