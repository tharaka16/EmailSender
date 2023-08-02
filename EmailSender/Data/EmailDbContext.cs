using System;
using EmailSender.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.Data
{
    public class EmailDbContext: DbContext
    {
        public DbSet<QueuedEmail> QueuedEmails { get; set; }

        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options)
        {
        }
    }
}

