using System;
using System.Net;
using EmailSender.Data;
using EmailSender.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EmailSender.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailDbContext _dbContext;
        private readonly string _sendGridApiKey;
        private readonly ILogger<EmailSenderService> _logger;

        public EmailSenderService(EmailDbContext dbContext, string sendGridApiKey, ILogger<EmailSenderService> logger)
        {
            _dbContext = dbContext;
            _sendGridApiKey = sendGridApiKey;
            _logger = logger;
        }

        public async Task<int> QueueEmail(QueuedEmail email)
        {
            try
            {
                _dbContext.QueuedEmails.Add(email);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to store email in database: ", ex.Message);
                return 0;
            }
        }

        public async Task SendQueuedEmails()
        {
            var queuedEmails = _dbContext.QueuedEmails.Where(e => !e.IsSent).ToList();

            foreach(var email in queuedEmails)
            {
                var client = new SendGridClient(_sendGridApiKey);
                var message = new SendGridMessage
                {
                    From = new EmailAddress("tharaka.lkm@gmail.com"),
                    Subject = email.Subject,
                    PlainTextContent = email.Body,
                    HtmlContent = email.Body
                };

                message.AddTo(new EmailAddress(email.ToEmail));

                try
                {
                    var response = await client.SendEmailAsync(message);
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        email.IsSent = true;
                        _dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    // log the exception or retry sending email
                    _logger.LogError("Email sending failed: ", ex.Message);
                }
            }    
        }
    }
}

