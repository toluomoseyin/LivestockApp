using System;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendEmailAsync(string recipientEmail, string subject, string htmlContent, string plainText = "");
    }
}
