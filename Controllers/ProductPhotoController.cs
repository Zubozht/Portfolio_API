using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.ProductPhoto;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("productphoto")]
    [ApiController]
    public class ProductPhotoController : ControllerBase
    {
        private readonly IProductPhotoRepository _productPhotoRepo;
        private readonly IProductRepository _productRepo;
        private readonly ILogger<ProductPhotoController> _logger;
        public ProductPhotoController(IProductPhotoRepository productPhotoRepo, IProductRepository productRepo, ILogger<ProductPhotoController> logger)
        {
            _productPhotoRepo = productPhotoRepo;
            _productRepo = productRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productphotos = await _productPhotoRepo.GetAllAsync();

            return Ok(productphotos);
        }

        /*[HttpPost("search")]
        public async Task<IActionResult> GetFiltered([FromBody] PhotoFilterDTO filterDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filteredPhotos = await _photoRepo.GetFilteredAsync(filterDTO);
            return Ok(filteredPhotos.Select(x => x.ToPhotoDTO()).ToList());
        }*/

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produtphoto = await _productPhotoRepo.GetByIdAsync(id);

            if (produtphoto == null)
            {
                return NotFound();
            }

            return Ok(produtphoto.ToProductPhotoDTO());
        }
        [HttpGet("image/{id:int}")]
        public async Task<IActionResult> GetImageById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imagepath = await _productPhotoRepo.GetImageByIdAsync(id);

            if (imagepath == null)
            {
                return NotFound();
            }

            var image = await System.IO.File.ReadAllBytesAsync(imagepath);

            return File(image, "image/jpeg");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductPhotoRequestDTO productPhotoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[]? imagebytes = null;
            string imagepath = "";

            if (productPhotoDTO.Image != null && productPhotoDTO.Image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await productPhotoDTO.Image.CopyToAsync(ms);
                    imagebytes = ms.ToArray();
                    imagepath = Path.Combine(Directory.GetCurrentDirectory(), "productphotos", $"tempphoto.jpg");
                    await System.IO.File.WriteAllBytesAsync(imagepath, imagebytes);
                };
            }

            var productPhotoModel = new ProductPhoto
            {
                Path = imagepath,
                Products = []
            };

            await _productPhotoRepo.CreateAsync(productPhotoModel);
            var savedProductPhoto = await _productPhotoRepo.GetByIdAsync(productPhotoModel.Id);

            if (savedProductPhoto == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetById), new { Id = productPhotoModel.Id }, savedProductPhoto.ToProductPhotoDTO());
        }

        [Authorize(Roles="Admin")]
        [HttpPut("{id:int}")]

        public async Task<IActionResult> Update([FromRoute(Name = "id")] int Id, [FromForm] UpdateProductPhotoRequestDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productPhotoModel = await _productPhotoRepo.UpdateAsync(Id, updateDTO);

            if (productPhotoModel == null)
            {
                return NotFound("Photo not found.");
            }

            return Ok(productPhotoModel.ToProductPhotoDTO());
        }

        [Authorize(Roles="Admin")]
        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Delete([FromRoute(Name = "id")] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productPhotoModel = await _productPhotoRepo.DeleteAsync(Id);

            if (productPhotoModel == null)
            {
                return NotFound("Photo not found.");
            }

            return NoContent();
        }
    }
}