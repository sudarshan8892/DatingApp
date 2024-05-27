using DatingApp.Controllers;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extension;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIDatingAPP.Extension;
using WebAPIDatingAPP.Interfaces;

namespace DatingApp.APIControllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IRepository _userRepo;
        private readonly ILikedRepository _likedRepo;

        public LikesController(IRepository userRepository, ILikedRepository repository)
        {
            _userRepo = userRepository;
            _likedRepo = repository;
        }

        [HttpPost("{UserName}")]
        public async Task<ActionResult> AddLike(string UserName)
        {
            var sourceId = (User.GetUserId());
            var likedUser = await _userRepo.GetUserByNameAsync(UserName);// case sensitive
            var sourceUser = await _likedRepo.GetUserWithLike(sourceId);

            if (likedUser == null) return NotFound();
            if (sourceUser.UserName == UserName) return BadRequest("you cant like yourself");

            var userlike = await _likedRepo.GetUserLike(sourceId, likedUser.Id);

            if (userlike != null) return BadRequest(" you alredy liked this User");


            userlike = new UserLike
            {
                SourceUserId = sourceId,
                TargetUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userlike);
            if (await _userRepo.SaveAllAsysc()) return Ok();

            return BadRequest("Failed like to User");
        }

        public async Task<ActionResult<PageList<LikedDto>>> GetUserLikess([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var user = await _likedRepo.GetUserLike(likesParams);
            Response.AddPaginationHeader(new PaginationHeader(user.CurrentPage, user.PageSize, user.TotalCount, user.TotalPage));
            return Ok(user);
        }
    }
}
