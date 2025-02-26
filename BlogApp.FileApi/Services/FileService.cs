using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace BlogApp.FileApi.Services
{
    // Kendi özel dosya sonuç sınıfımızı tanımlayalım (FileResult isim çakışmasını önlemek için)
    public class FileDownloadResult
    {
        public Stream FileStream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }

    public interface IFileService
    {
        Task<(string fileName, bool isDuplicate)> UploadFileAsync(IFormFile file);
        Task<FileDownloadResult> DownloadFileAsync(string fileName);
        Task<bool> DeleteFileAsync(string fileName);
    }

    public class FileService : IFileService
    {
        private readonly string _fileStoragePath;
        private readonly Dictionary<string, string> _fileHashes = new Dictionary<string, string>();

        public FileService(IConfiguration configuration)
        {
            _fileStoragePath = configuration["FileStorage:Path"] ?? "Uploads";
            if (!Directory.Exists(_fileStoragePath))
            {
                Directory.CreateDirectory(_fileStoragePath);
            }

            // Mevcut dosyaların hash değerlerini ön yükleme
            PreloadFileHashes();
        }

        private void PreloadFileHashes()
        {
            // Performans için, uygulama başlatıldığında mevcut dosyaların hash değerlerini hesapla
            try
            {
                foreach (var filePath in Directory.GetFiles(_fileStoragePath))
                {
                    var fileHash = CalculateFileHash(filePath);
                    var fileName = Path.GetFileName(filePath);
                    _fileHashes[fileHash] = fileName;
                }
            }
            catch (Exception)
            {
                // Hash yükleme hatalarını yok say - gerekirse loglama yapılabilir
            }
        }

        public async Task<(string fileName, bool isDuplicate)> UploadFileAsync(IFormFile file)
        {
            // Önce dosya hash değerini hesapla
            string fileHash = await CalculateFileHashAsync(file);

            // Aynı hash değerine sahip dosya var mı kontrol et
            if (_fileHashes.TryGetValue(fileHash, out string existingFileName))
            {
                // Aynı içeriğe sahip dosya zaten var, mevcut dosya adını ve duplicate=true döndür
                return (existingFileName, true);
            }

            // Daha anlamlı dosya isimlendirmesi
            string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string safeFileName = Path.GetFileNameWithoutExtension(file.FileName)
                .Replace(" ", "_")
                .Replace("-", "_");
            string fileExtension = Path.GetExtension(file.FileName);

            string newFileName = $"{timestamp}_{safeFileName}{fileExtension}";
            string filePath = Path.Combine(_fileStoragePath, newFileName);

            // Dosya adı çakışma kontrolü
            if (System.IO.File.Exists(filePath))
            {
                // Eğer aynı adda dosya varsa, adın sonuna sayı ekleyerek benzersiz yap
                int counter = 1;
                string fileNameWithoutExt = $"{timestamp}_{safeFileName}";

                while (System.IO.File.Exists(filePath))
                {
                    newFileName = $"{fileNameWithoutExt}({counter}){fileExtension}";
                    filePath = Path.Combine(_fileStoragePath, newFileName);
                    counter++;
                }
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Yeni dosya hash değerini kaydet
            _fileHashes[fileHash] = newFileName;

            // Yeni dosya adını ve duplicate=false döndür
            return (newFileName, false);
        }

        private async Task<string> CalculateFileHashAsync(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                ms.Position = 0;
                using (var sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(ms);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private string CalculateFileHash(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public async Task<FileDownloadResult> DownloadFileAsync(string fileName)
        {
            var filePath = Path.Combine(_fileStoragePath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return new FileDownloadResult
            {
                FileStream = memory,
                ContentType = GetContentType(filePath),
                FileName = Path.GetFileName(filePath)
            };
        }

        public Task<bool> DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_fileStoragePath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return Task.FromResult(false);
            }

            try
            {
                // Dosya hash değerini hesapla
                string fileHash = CalculateFileHash(filePath);

                // Dosyayı sil
                System.IO.File.Delete(filePath);

                // Hash tablosundan da kaldır
                if (_fileHashes.ContainsKey(fileHash))
                {
                    _fileHashes.Remove(fileHash);
                }

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        private string GetContentType(string path)
        {
            var extension = Path.GetExtension(path).ToLowerInvariant();

            return extension switch
            {
                ".txt" => "text/plain",
                ".pdf" => "application/pdf",
                ".doc" => "application/vnd.ms-word",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}