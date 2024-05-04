using CloudinaryDotNet.Actions;

namespace WebAPIDatingAPP.Interfaces
{
    public interface IPhotoService
    {
       public  Task<ImageUploadResult>AddPhotoAsync(IFormFile file);
       public Task<DeletionResult> DeletePhotoAsync(string publicid);
    }
}
