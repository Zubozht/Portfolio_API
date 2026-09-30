using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Tag;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("tag")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _tagRepo;
        private readonly IPhotoRepository _photoRepo;
        public TagController(ITagRepository tagRepo, IPhotoRepository photoRepo)
        {
            _tagRepo = tagRepo;
            _photoRepo = photoRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tags = await _tagRepo.GetFilteredAsync(new TagFilterDTO { sortBy = "tag", isDescending = false });
            var tagDTO = tags.Select(x => x.ToTagDTO());
            
            return Ok(tagDTO);
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetFiltered([FromBody] TagFilterDTO filterDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filteredTags = await _tagRepo.GetFilteredAsync(filterDTO);

            return Ok(filteredTags.Select(x => x.ToTagDTO()).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tag = await _tagRepo.GetByIdAsync(id);
            
            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag.ToTagDTO());
        }
        [Authorize(Roles="Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagRequestDTO tagDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tagModel = tagDTO.ToTagFromCreateDTO();

            foreach (int photoid in tagModel.photos.Select(x => x.Photoid))
            {
                if (!await _photoRepo.PhotoExists(photoid))
                {
                    return BadRequest("Some of the specified photos do not exist.");
                }
            }

            await _tagRepo.CreateAsync(tagModel);
            var savedTag = await _tagRepo.GetByIdAsync(tagModel.id);
            if (savedTag == null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetById), new{id = tagModel.id}, savedTag.ToTagDTO());
        }
        
        [HttpGet("previewimage/{id:int}")]
        public async Task<IActionResult> GetPreviewImageById(int id)
        {
            var previewimagepath = await _tagRepo.GetPreviewImageByIdAsync(id);

            if (previewimagepath == null)
            {
                return NotFound();
            }

            var previewimage = System.IO.File.ReadAllBytes(previewimagepath);

            return File(previewimage, "image/jpeg");
        }

        [Authorize(Roles="Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTagRequestDTO updateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tagModel = await _tagRepo.UpdateAsync(id, updateDTO);

            if (tagModel == null)
            {
                return NotFound("Tag not found.");
            }

            return Ok(tagModel.ToTagDTO());
        }
        [Authorize(Roles="Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tagModel = await _tagRepo.DeleteAsync(id);

            if (tagModel == null)
            {
                return NotFound("Tag not found.");
            }

            return NoContent();
        }
    }
}