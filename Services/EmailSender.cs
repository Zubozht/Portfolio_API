using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using api.Interfaces;
using Google.Protobuf;

namespace api.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            string smtphost = _config["SMTP:Host"] ?? "";
            int smtpport = int.Parse(_config["SMTP:Port"] ?? "587");
            bool enablessl = bool.Parse(_config["SMTP:EnableSsl"] ?? "true");
            string mymail = _config["SMTP:Username"] ?? "";
            string mypw = _config["SMTP:Password"] ?? "";
            
            var client = new SmtpClient(smtphost, smtpport)
            {
                EnableSsl = enablessl,
                Credentials = new NetworkCredential(mymail, mypw)
            };

            return client.SendMailAsync(new MailMessage(from:mymail, to:email, subject, message));
        }
    }
}