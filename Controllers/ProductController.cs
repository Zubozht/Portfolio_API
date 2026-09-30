using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Product;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductPhotoRepository _productPhotoRepo;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductRepository productRepo, IProductPhotoRepository productPhotoRepo, ILogger<ProductController> logger)
        {
            _productRepo = productRepo;
            _productPhotoRepo = productPhotoRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = (await _productRepo.GetFilteredAsync(new ProductFilterDTO { SortBy = "Id", IsDescending = true })).Select(x => x.ToProductDTO());

            return Ok(products);
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetFiltered([FromBody] ProductFilterDTO filterDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filteredPhotos = await _productRepo.GetFilteredAsync(filterDTO);
            return Ok(filteredPhotos.Select(x => x.ToProductDTO()).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product.ToProductDTO());
        }
        [HttpGet("productpreviewimage/{id:int}")]
        public async Task<IActionResult> GetPreviewImageById([FromRoute(Name = "id")] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var previewimagepath = await _productRepo.GetPreviewImageByIdAsync(Id);

            if (previewimagepath == null)
            {
                return NotFound();
            }

            var previewimage = await System.IO.File.ReadAllBytesAsync(previewimagepath);

            return File(previewimage, "image/jpeg");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequestDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productModel = productDTO.ToProductFromCreateDTO();

            await _productRepo.CreateAsync(productModel);
            var savedProduct = await _productRepo.GetByIdAsync(productModel.Id);

            if (savedProduct == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetById), new { Id = productModel.Id }, savedProduct.ToProductDTO());
        }

        [Authorize(Roles="Admin")]
        [HttpPut("{id:int}")]

        public async Task<IActionResult> Update([FromRoute(Name = "id")] int Id, [FromBody] UpdateProductRequestDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productModel = await _productRepo.UpdateAsync(Id, updateDTO);

            if (productModel == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(productModel.ToProductDTO());
        }

        [Authorize(Roles="Admin")]
        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var photoModel = await _productRepo.DeleteAsync(id);

            if (photoModel == null)
            {
                return NotFound("Product not found.");
            }

            return NoContent();
        }
    }
}