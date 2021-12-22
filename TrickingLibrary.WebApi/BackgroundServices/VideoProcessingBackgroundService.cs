using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrickingLibrary.Data;

namespace TrickingLibrary.WebApi.BackgroundServices
{
    public class VideoProcessingBackgroundService : BackgroundService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<VideoProcessingBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ChannelReader<ProcessVideoMessage> _channelReadr;

        public VideoProcessingBackgroundService(
            IWebHostEnvironment env, 
            Channel<ProcessVideoMessage> channel,
            ILogger<VideoProcessingBackgroundService> logger,
            IServiceProvider serviceProvider)
        {
            _env = env;
            _logger = logger;
            // We are using ServiceProvider here because this is the Singleton. The constructor will not called again.
            _serviceProvider = serviceProvider;
            _channelReadr = channel.Reader;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _channelReadr.WaitToReadAsync(stoppingToken))
            {
                var message = await _channelReadr.ReadAsync(stoppingToken);
                try
                {
                    var inputPath = Path.Combine(_env.WebRootPath, message.Input);
                    var outputPath = Path.Combine(_env.WebRootPath, message.Output);
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(_env.ContentRootPath, "Ffmpeg", "ffmpeg.exe"),
                        Arguments = $"-y -i {inputPath} -an -vf scale=540x380 {outputPath}",
                        WorkingDirectory = _env.WebRootPath,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };

                    using var process = new Process{ StartInfo = startInfo };
                    process.Start();
                    await process.WaitForExitAsync(stoppingToken);

                    using var scope = _serviceProvider.CreateScope();
                    var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var submission = await ctx.Submissions.FirstOrDefaultAsync(
                        x => x.Id == message.SubmissionId, stoppingToken);

                    submission.Video = message.Output;
                    submission.IsVideoProcessed = true;
                    
                    await ctx.SaveChangesAsync(stoppingToken);

                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Video processing failed. Input message: {0}", message);
                }
            }
        }
    }
}