using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TrickingLibrary.WebApi.Services
{
    public class VideoManager
    {
        private readonly IWebHostEnvironment _env;
        public const string TempPrefix = "temp_";
        public const string ConvertedVideoPrefix = "converted_";
        public const string ConvertVideoFormat = "mp4";

        public VideoManager(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GetConvertedVideoName => $"{ConvertedVideoPrefix}{DateTime.Now.Ticks}.{ConvertVideoFormat}";
        public string WorkingDirectory => _env.WebRootPath;
        
        public string GetVideoPath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Incorrect source file name.");
            }
            
            return Path.Combine(WorkingDirectory, fileName);
        }

        public async Task<string> SaveTemporaryVideoAsync(IFormFile video)
        {
            var temporaryFileName = GetTemporaryFileName(video.FileName);
            var savePath = GetVideoPath(temporaryFileName);
            await using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            await video.CopyToAsync(fileStream);
            return temporaryFileName;
        }

        public void DeleteTemporaryVideo(string fileName)
        {
            var path = GetVideoPath(fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public bool IsVideoExists(string fileName) => File.Exists(GetVideoPath(fileName));

        private string GetTemporaryFileName(string sourceFileName)
        {
            if (string.IsNullOrWhiteSpace(sourceFileName) || string.IsNullOrWhiteSpace(Path.GetExtension(sourceFileName)))
            {
                throw new ArgumentException("Incorrect source file name.");
            }

            return string.Concat(TempPrefix, DateTime.Now.Ticks, Path.GetExtension(sourceFileName));
        }

    }
}