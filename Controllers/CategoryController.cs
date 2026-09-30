using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Category;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace api.Controllers
{
    [Route("category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryRepository categoryRepo, ILogger<CategoryController> logger)
        {
            _categoryRepo = categoryRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categories = await _categoryRepo.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute(Name = "id")] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _categoryRepo.GetByIdAsync(Id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category.ToCategoryDTO());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequestDTO createDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryModel = createDTO.ToCategoryFromCreateDTO();
            await _categoryRepo.CreateAsync(categoryModel);
            var savedCategory = await _categoryRepo.GetByIdAsync(categoryModel.Id);
            if (savedCategory == null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetById), new{id = categoryModel.Id}, savedCategory.ToCategoryDTO());
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute(Name = "id")] int Id, [FromBody] UpdateCategoryRequestDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryModel = await _categoryRepo.UpdateAsync(Id, updateDTO);

            if (categoryModel == null)
            {
                return NotFound("Tag not found.");
            }

            return Ok(categoryModel.ToCategoryDTO());
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute(Name = "id")] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryModel = await _categoryRepo.DeleteAsync(Id);

            if (categoryModel == null)
            {
                return NotFound("Tag not found.");
            }

            return NoContent();
        }
    }
}