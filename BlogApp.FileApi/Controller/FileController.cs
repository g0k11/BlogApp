using Microsoft.AspNetCore.Http;
using BlogApp.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using BlogApp.FileApi.Services;
using System;
using System.Threading.Tasks;
using System.IO;
using Ardalis.Result;

namespace BlogApp.FileApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        [HttpPost("upload")]
        public async Task<ActionResult<Response<string>>> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Ok(new Response<string>(false, "Dosya yüklenmemiş"));
            }

            try
            {
                var (fileName, isDuplicate) = await _fileService.UploadFileAsync(file);

                if (isDuplicate)
                {
                    return Ok(new Response<string>(true, "Bu dosya zaten mevcut, tekrar yüklenmedi", fileName));
                }
                
                return Ok(new Response<string>(true, "Dosya başarıyla yüklendi", fileName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>(false, $"Dosya yükleme hatası: {ex.Message}"));
            }
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest(new Response<object>(false, "Dosya adı belirtilmedi"));
            }

            try
            {
                var fileResult = await _fileService.DownloadFileAsync(fileName);

                if (fileResult == null)
                {
                    return NotFound(new Response<object>(false, "Dosya bulunamadı"));
                }

                return File(fileResult.FileStream, fileResult.ContentType, fileResult.FileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<object>(false, $"Dosya indirme hatası: {ex.Message}"));
            }
        }

        [HttpDelete("delete/{fileName}")]
        public async Task<ActionResult<Response<object>>> DeleteFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest(new Response<object>(false, "Dosya adı belirtilmedi"));
            }

            try
            {
                var result = await _fileService.DeleteFileAsync(fileName);

                if (!result)
                {
                    return NotFound(new Response<object>(false, "Dosya bulunamadı"));
                }

                return Ok(new Response<object>(true, "Dosya başarıyla silindi"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<object>(false, $"Dosya silme hatası: {ex.Message}"));
            }
        }
    }
}