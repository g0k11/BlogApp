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
    public class PersonalInfoController : ControllerBase
    {
        private readonly IPersonalInfoService _personalInfoService;

        public PersonalInfoController(IPersonalInfoService personalInfoService)
        {
            _personalInfoService = personalInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _personalInfoService.GetAllAsync();
            if (result.Status == ResultStatus.Error)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _personalInfoService.GetAsync(id);
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
        public async Task<IActionResult> Create([FromBody] PersonalInfo model)
        {
            var result = await _personalInfoService.CreateAsync(model);
            if (result.Status == ResultStatus.Error)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PersonalInfo model)
        {
            if (id != model.Id)
                return BadRequest("ID mismatch.");

            var result = await _personalInfoService.UpdateAsync(model);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Error)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _personalInfoService.DeleteAsync(id);
            if (result.Status == ResultStatus.NotFound)
                return NotFound();
            if (result.Status == ResultStatus.Error)
                return BadRequest(result.Errors);

            return NoContent();
        }
    }
}
