using Microsoft.AspNetCore.Http;
using BlogApp.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using BlogApp.FileApi.Services;
using System;
using System.Threading.Tasks;
using System.IO;

namespace BlogApp.FileApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new Response(false, "Dosya yüklenmemiş"));
            }

            try
            {
                // Servis çağrısı sırasında dosyanın yeni mi yoksa var olan mi olduğunu anlamak için tuple döndürelim
                var (fileName, isDuplicate) = await _fileService.UploadFileAsync(file);

                if (isDuplicate)
                {
                    // Dosya zaten var, yeni yüklenmedi
                    return Ok(new Response<string>(true, "Bu dosya zaten mevcut, tekrar yüklenmedi", fileName));
                }
                else
                {
                    // Yeni dosya başarıyla yüklendi
                    return Ok(new Response<string>(true, "Dosya başarıyla yüklendi", fileName));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response(false, $"Dosya yükleme hatası: {ex.Message}"));
            }
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            try
            {
                var fileResult = await _fileService.DownloadFileAsync(fileName);

                if (fileResult == null)
                {
                    return NotFound(new Response(false, "Dosya bulunamadı"));
                }

                return File(fileResult.FileStream, fileResult.ContentType, fileResult.FileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response(false, $"Dosya indirme hatası: {ex.Message}"));
            }
        }

        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            try
            {
                var result = await _fileService.DeleteFileAsync(fileName);

                if (!result)
                {
                    return NotFound(new Response(false, "Dosya bulunamadı"));
                }

                return Ok(new Response(true, "Dosya başarıyla silindi"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response(false, $"Dosya silme hatası: {ex.Message}"));
            }
        }
    }
}