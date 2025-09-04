using DeliveryAPi.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace DeliveryAPI.Application.Abstractions;

public interface ICloudinaryService
{
    Task<string> UploadFileAsync(IFormFile file,FileTypeEnum type,CancellationToken cancellationToken, params string[] folders);
    Task<string> UploadFileAsync(string path, FileTypeEnum type ,CancellationToken cancellationToken, params string[] folders);
    Task DeleteFileAsync(string path, CancellationToken cancellationToken);
}