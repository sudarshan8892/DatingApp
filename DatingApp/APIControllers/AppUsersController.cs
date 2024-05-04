using AutoMapper;
using DatingApp.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIDatingAPP.Entities;
using WebAPIDatingAPP.Extension;
using WebAPIDatingAPP.Interfaces;

namespace DatingApp.Controllers
{
    [Authorize]
    public class AppUsersController : BaseApiController
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public AppUsersController(IRepository repository, IMapper mapper, IPhotoService photoService)
        {
            _repository = repository;
            _mapper = mapper;
            _photoService = photoService;
        }
        [HttpGet]
        //api/AppUsers
        public async Task<ActionResult<IEnumerable<MemberDTo>>> GetUsers()
        {

            var user = (await _repository.GetMemberAsync());
            return (Ok(user));

        }
        [HttpGet("{id:int}")]
        //api/AppUsers/1
        public async Task<ActionResult<MemberDTo>> GetUserbyId(int id)
        {

            var user = (await _repository.GetUserByIdAsync(id));
            var returnuser = _mapper.Map<MemberDTo>(user);
            return (Ok(returnuser));

        }
        [HttpGet("{name}")]
        //api/AppUsers/UserName
        public async Task<ActionResult<MemberDTo>> GetUserByUserName(string name)
        {
            return (await _repository.GetMemberByNameAsync(name));


        }
        [HttpPut]
        public async Task<IActionResult> Memberupdate(MemberUpdateDTo updateDTo)
        {

            var user = await _repository.GetUserByNameAsync(User.GetUserName());
            if (user == null) return NotFound();
            _mapper.Map(updateDTo, user);
            if (await _repository.SaveAllAsysc()) return Ok(user.UserName);
            // sudarsshan
            // return NoContent();
            return BadRequest("failed to update user");
        }

        [HttpPost("Add-photo")]
        public async Task<ActionResult<PhotoDTo>> AddPhoto(IFormFile file)
        {
            var user = await _repository.GetUserByNameAsync(User.GetUserName());

            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUri.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (user.Photos.Count == 0) photo.IsMain = true;
            user.Photos.Add(photo);
            if (await _repository.SaveAllAsysc())
            {
                return CreatedAtAction(nameof(GetUserByUserName), new { name = user.UserName },
                    _mapper.Map<PhotoDTo>(photo));
            }
            return BadRequest("probalem  uploading photo ");
        }
        [HttpPut("set-Main-photo/{PhotoId}")]
        public async Task<IActionResult> SetMainPhoto(int PhotoId)
        {
            var user = await _repository.GetUserByNameAsync(User.GetUserName());

            if (user == null) return NotFound();
            var photo = user.Photos.FirstOrDefault(p => p.Id == PhotoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest(" this is already your Main photo");

            var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;
            if (await _repository.SaveAllAsysc()) return NoContent();
            return BadRequest("Problem setting the Main photo");
        }

    }
}

