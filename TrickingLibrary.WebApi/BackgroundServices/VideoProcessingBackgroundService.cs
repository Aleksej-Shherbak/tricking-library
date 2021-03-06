using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrickingLibrary.WebApi.Services;

namespace TrickingLibrary.WebApi.BackgroundServices
{
    public class VideoProcessingBackgroundService : BackgroundService
    {
        private readonly ILogger<VideoProcessingBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly VideoManager _videoManager;
        private readonly ChannelReader<ProcessVideoMessage> _channelReadr;

        public VideoProcessingBackgroundService(
            Channel<ProcessVideoMessage> channel,
            ILogger<VideoProcessingBackgroundService> logger,
            IServiceProvider serviceProvider, 
            VideoManager videoManager)
        {
            _logger = logger;
            // We are using ServiceProvider here because this is the Singleton. The constructor will not called again.
            _serviceProvider = serviceProvider;
            _videoManager = videoManager;
            _channelReadr = channel.Reader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _channelReadr.WaitToReadAsync(stoppingToken))
            {
                var message = await _channelReadr.ReadAsync(stoppingToken);
                var convertedVideoName = _videoManager.GetConvertedVideoName;
                var thumbnailFileName = _videoManager.GetThumbnailFileName;
                var inputPath = _videoManager.GetFilePath(message.TemporaryVideoName);
                var outputConvertedVideoPath =  _videoManager.GetFilePath(convertedVideoName);
                var outputThumbnailPath =  _videoManager.GetFilePath(thumbnailFileName);
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = _videoManager.FfmpegPath,
                        Arguments = 
                            $"-y -i {inputPath} -an -vf scale=540x380 {outputConvertedVideoPath} -ss 00:00:00 -vframes 1 -vf scale=540x380 {outputThumbnailPath}",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };

                    using var process = new Process {StartInfo = startInfo};
                    process.Start();
                    await process.WaitForExitAsync(stoppingToken);

                    if (!_videoManager.IsFileExists(convertedVideoName))
                    {
                        throw new Exception(
                            $"FFMPEG failed to generate converted video with given params: input path {inputPath}, output path: {outputConvertedVideoPath}");
                    }
                    
                    using var scope = _serviceProvider.CreateScope();
                    var submissionService = scope.ServiceProvider.GetRequiredService<SubmissionService>();

                    await submissionService.SetSubmissionVideoAsync(message.SubmissionId, convertedVideoName,
                        thumbnailFileName, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Video processing failed. Input message: {0}", message);
                }
                finally
                {
                    _videoManager.DeleteVideo(message.TemporaryVideoName);
                }
            }
        }
    }
}