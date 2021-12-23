using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Entities;
using TrickingLibrary.WebApi.BackgroundServices;
using TrickingLibrary.WebApi.Exceptions;
using TrickingLibrary.WebApi.RequestModels;

namespace TrickingLibrary.WebApi.Services
{
    public class SubmissionService
    {
        private readonly VideoManager _videoManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly Channel<ProcessVideoMessage> _channel;

        public SubmissionService(VideoManager videoManager, ApplicationDbContext dbContext, Channel<ProcessVideoMessage> channel)
        {
            _videoManager = videoManager;
            _dbContext = dbContext;
            _channel = channel;
        }
        
        public async Task CreateSubmissionAsync(SubmissionFormModel submission)
        {
            if (!await _dbContext.Tricks.AnyAsync(x => x.Id == submission.TrickId))
            {
                throw new TlNotFoundException("Category not found!");
            }
            
            var temporaryFileName =  await _videoManager.SaveTemporaryVideoAsync(submission.Video);
            
            var newSubmission = new Submission
            {
                Name = submission.Name,
                Description = submission.Description,
                TrickId = submission.TrickId,
            };

            await _dbContext.AddAsync(newSubmission);
            await _dbContext.SaveChangesAsync();
            await _channel.Writer.WriteAsync(new ProcessVideoMessage
            {
                SubmissionId = newSubmission.Id,
                TemporaryVideoName = temporaryFileName,
            });
            
        }
        
        public async Task SetSubmissionVideoAsync(int submissionId, string fileName, CancellationToken stoppingToken)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name can not be null or empty.");
            }

            var submission = await _dbContext.Submissions.FirstOrDefaultAsync(
                x => x.Id == submissionId, stoppingToken);

            if (submission == null)
            {
                throw new InvalidOperationException(
                    $"Can not set video to submission with id {submissionId}. Submission not found");
            }
            submission.Video = fileName;
            submission.IsVideoProcessed = true;
                    
            await _dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}