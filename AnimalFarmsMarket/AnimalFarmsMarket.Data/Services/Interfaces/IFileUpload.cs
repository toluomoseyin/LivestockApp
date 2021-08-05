using AnimalFarmsMarket.Data.DTOs;
using Microsoft.AspNetCore.Http;

namespace AnimalFarmsMarket.Data.Services.Interfaces
{
    public interface IFileUpload
    {
        UploadAvatarResponse UploadAvatar(IFormFile file);
        CloudinaryDotNet.Actions.DeletionResult DeleteAvatar(string publicId);
    }
}
