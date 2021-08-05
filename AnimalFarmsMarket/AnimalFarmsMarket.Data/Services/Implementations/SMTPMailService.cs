using AnimalFarmsMarket.Data.Services.Interfaces;
using AnimalFarmsMarket.Data.Settings;

using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

using System;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Implemantations
{
    public class SMTPMailService : INotificationService
    {
        private readonly EmailSenderConfig _emailSenderConfig;

        public SMTPMailService(IOptions<EmailSenderConfig> emailSenderConfig, ILogger<SMTPMailService> logger)
        {
            _emailSenderConfig = emailSenderConfig.Value;
            Logger = logger;
        }

        public ILogger<SMTPMailService> Logger { get; }

        public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string htmlContent, string plainText = "")
        {
            try
            {
                var message = new MimeMessage();
                var bodyBuilder = new BodyBuilder();

                // from
                message.From.Add(new MailboxAddress(_emailSenderConfig.SenderName, _emailSenderConfig.SenderEmail));

                // to
                message.To.Add(MailboxAddress.Parse(recipientEmail));

                //build the message subject and body
                message.Subject = subject;
                bodyBuilder.HtmlBody = htmlContent;
                bodyBuilder.TextBody = plainText;
                message.Body = bodyBuilder.ToMessageBody();

                //Configure smtp client for sending the email
                var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate(_emailSenderConfig.SenderEmail, _emailSenderConfig.ApiKey);
                client.Send(message);
                client.Disconnect(true);
                await Task.CompletedTask;
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
