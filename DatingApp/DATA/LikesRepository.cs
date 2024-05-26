using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebAPIDatingAPP.DATA;
using WebAPIDatingAPP.Entities;
using WebAPIDatingAPP.Extension;

namespace DatingApp.DATA
{
    public class LikesRepository : ILikedRepository
    {
        private readonly DataContext _context;

        public LikesRepository(DataContext dbContext)
        {
            _context = dbContext;
        }
        public  async Task<UserLike> GetUserLike(int SourceUserId, int TragetUserId)
        {
            return await _context.Likes.FindAsync(SourceUserId, TragetUserId);
        }

        public async Task<IEnumerable<LikedDto>> GetUserLike(string Predicate, int UserId)
        {
            var user = _context.AppUsers.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();
            if(Predicate=="liked")
            {
                likes= likes.Where(l=>l.SourceUserId==UserId);
                user = likes.Select(like => like.TargetUser);
            }
            if (Predicate == "likedBy")
            {
                likes = likes.Where(l => l.TargetUserId == UserId);
                user = likes.Select(like => like.SourceUser);
            }
            return await user.Select(user =>new LikedDto
            {
                UserName=user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalcuateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(u=>u.IsMain).Url,
                City = user.City,
                Id = user.Id

            }).ToListAsync();
        }
        public async Task<AppUsers> GetUserWithLike(int UserId)
        {
            return await _context.AppUsers
                 .Include(x => x.LikedUsers)
                 .FirstOrDefaultAsync(x => x.Id == UserId);
        }
    }
}
