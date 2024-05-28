using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Entities;
using WebAPIDatingAPP.Entities;
using WebAPIDatingAPP.Extension;

namespace WebAPIDatingAPP.Helpers
{
    public class AutoMapperUsers : Profile
    {

        public AutoMapperUsers()
        {
            CreateMap<AppUsers, MemberDTo>()
                .ForMember(destinationMember => destinationMember.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            
                .ForMember(destinationMember => destinationMember.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalcuateAge()));
            CreateMap<Photo, PhotoDTo>();
            CreateMap<MemberUpdateDTo, AppUsers>();
            CreateMap<RegisterDto, AppUsers>();
            CreateMap<Message, MessageDto>()
             .ForMember(d => d.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
             .ForMember(d => d.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url));



        }
    }
}
