using AnimalFarmsMarket.Data.Services.Interfaces;
using AnimalFarmsMarket.Data.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SendGrid;
using SendGrid.Helpers.Mail;

using System;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Implemantations
{
    public class NotificationService : INotificationService
    {
        public NotificationService(IOptions<EmailSenderConfig> emailConfig, ILogger<NotificationService> logger)
        {
            Configuration = emailConfig.Value;
            Logger = logger;
        }

        public ILogger<NotificationService> Logger { get; }

        private EmailSenderConfig Configuration { get; set; }

        public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string htmlContent, string plainText = "")
        {
            try
            {
                var client = new SendGridClient(Configuration.ApiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(Configuration.SenderEmail, Configuration.SenderName),
                    Subject = subject,
                    PlainTextContent = plainText,
                    HtmlContent = htmlContent
                };
                msg.AddTo(new EmailAddress(recipientEmail));

                //disable tracking by sendgrid
                msg.SetOpenTracking(false);
                msg.SetClickTracking(false, false);
                msg.SetSubscriptionTracking(false);

                //disable google analytics
                msg.SetGoogleAnalytics(false);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted) return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, new string[] { recipientEmail });
                return false;
            }
            return false;
        }
    }
}
