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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _projectsService.GetAllAsync();
            if (result.Status == ResultStatus.Error)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _projectsService.GetAsync(id);
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
        public async Task<IActionResult> Create([FromBody] Projects model)
        {
            var result = await _projectsService.CreateAsync(model);
            if (result.Status == ResultStatus.Error)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Projects model)
        {
            if (id != model.Id)
                return BadRequest("ID mismatch.");

            var result = await _projectsService.UpdateAsync(model);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Error)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _projectsService.DeleteAsync(id);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Error)
                return BadRequest(result.Errors);

            return NoContent();
        }
    }
}
