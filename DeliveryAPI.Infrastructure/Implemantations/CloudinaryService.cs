using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DeliveryAPI.Application.Abstractions;
using DeliveryAPi.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace DeliveryAPI.Persistence.Implementations.Services;

public class CloudinaryService(Cloudinary cloudinary) : ICloudinaryService
{
    public async Task<string> UploadFileAsync(IFormFile file, FileTypeEnum type,CancellationToken cancellationToken, params string[] folders)
        {

            if (file == null || file.Length == 0)
                throw new Exception("File is empty");

            var filePath = string.Join("/", folders);

            RawUploadParams? uploadParams;
            RawUploadResult uploadResult;
            using (var stream = file.OpenReadStream())
            {
                var fileDescription = new FileDescription(file.FileName, stream);
                uploadParams = type switch
                {
                    FileTypeEnum.Image => new ImageUploadParams { File = fileDescription, Folder = filePath },
                    FileTypeEnum.Video => new VideoUploadParams { File = fileDescription, Folder = filePath },
                    FileTypeEnum.Audio => new RawUploadParams { File = fileDescription, Folder = filePath },
                    _ =>   throw new Exception("Can't upload this type file")
                };
                if (uploadParams == null)
                    return string.Empty;
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }


            return uploadResult.Error != null ? throw new Exception("UploadResult") : uploadResult.SecureUrl.ToString();
        }

    public Task<string> UploadFileeAsync(string path, FileTypeEnum type, CancellationToken cancellationToken, params string[] folders)
    {
        throw new NotImplementedException();
    }


    private string _extractPublicId(string secureUrl)
        {
            if (string.IsNullOrEmpty(secureUrl))
                throw new ArgumentException("Link boş ola bilməz!");

            
            var uri = new Uri(secureUrl);
            var path = uri.AbsolutePath.TrimStart('/'); 

            var segments = path.Split('/');

            StringBuilder sb = new StringBuilder(segments[4]);
            for (int i = 5; i < segments.Length; i++)
            {
                sb.Append("/");
                sb.Append(segments[i]);
            }
            string publicId = sb.ToString();
            publicId = publicId.Substring(0, publicId.LastIndexOf("."));
            return publicId;
        }



        public async Task DeleteFileAsync(string url, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(url))
                throw new Exception("File is empty");

            string publicId = _extractPublicId(url);
            var destroyParams = new DeletionParams(publicId);

            
            var result = await cloudinary.DestroyAsync(destroyParams);
            
            if (result.Error != null)
            {
                Console.WriteLine($"Fayl silinə bilmədi: {result.Error.Message}");
            }

        }
        public async Task<string> UploadFileAsync(string url, FileTypeEnum type,CancellationToken cancellationToken, params string[] folders)
        {
            if (url == null || url.Length == 0)
                throw new Exception("File is empty");

            var filePath = Path.Combine(folders);

            RawUploadParams? uploadParams;
            RawUploadResult uploadResult;
            var fileDescription = new FileDescription(url);
            uploadParams = type switch
            {
                FileTypeEnum.Image => new ImageUploadParams { File = fileDescription, Folder = filePath },
                FileTypeEnum.Video => new VideoUploadParams { File = fileDescription, Folder = filePath },
                FileTypeEnum.Audio => new RawUploadParams { File = fileDescription, Folder = filePath },
                _ =>  throw new Exception("Can't upload this type file")
            };

            uploadResult = await cloudinary.UploadAsync(uploadParams);
            return uploadResult.Error != null ? throw new Exception("exception upload") : uploadResult.SecureUrl.ToString();
        }
}