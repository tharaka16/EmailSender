using System;
using EmailSender.Models;

namespace EmailSender.Services
{
    public interface IEmailSenderService
    {
        public Task<int> QueueEmail(QueuedEmail email);
        public Task SendQueuedEmails();
    }
}

