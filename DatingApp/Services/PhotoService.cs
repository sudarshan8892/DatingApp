using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using WebAPIDatingAPP.Helpers;
using WebAPIDatingAPP.Interfaces;

namespace WebAPIDatingAPP.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinaryDot;

        public PhotoService(IOptions<CloudinarySetting> config)
        {
            var acc = new Account
                (

                config.Value.CouldName,
                config.Value.ApiKey,
                config.Value.ApiSecret

                );

            _cloudinaryDot = new Cloudinary(acc);

        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadresult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var Stream = file.OpenReadStream();
                var uploadPram = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, Stream),
                    Transformation = new Transformation().Height("500").Width("500").Crop("fill").Gravity("face"),
                    Folder = "dot-net6"
                };
                uploadresult = await _cloudinaryDot.UploadAsync(uploadPram);
            }
            return uploadresult;
        }


        public async Task<DeletionResult>DeletePhotoAsync(string publicid)
        {
            var deletionParams = new DeletionParams(publicid);
            return await _cloudinaryDot.DestroyAsync(deletionParams);

        }
    }
}
