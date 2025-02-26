using Ardalis.Result;
using BlogApp.DataApi.Entities;
using BlogApp.DataApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlogApp.DataApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EducationsController : ControllerBase
    {
        private readonly IEducationsService _educationsService;

        public EducationsController(IEducationsService educationsService)
        {
            _educationsService = educationsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _educationsService.GetAllAsync();
            if (result.Status == ResultStatus.Error)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _educationsService.GetAsync(id);
            switch (result.Status)
            {
                case ResultStatus.NotFound:
                    return NotFound();
                case ResultStatus.Error:
                    return BadRequest(result.Errors);
                default:
                    return Ok(result.Value);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Educations model)
        {
            var result = await _educationsService.CreateAsync(model);
            if (result.Status == ResultStatus.Error)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Educations model)
        {
            if (id != model.Id)
                return BadRequest("ID mismatch.");

            var result = await _educationsService.UpdateAsync(model);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Error)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _educationsService.DeleteAsync(id);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Error)
                return BadRequest(result.Errors);

            return NoContent();
        }
    }
}
