using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.DTOs;
using DatingApp.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WebAPIDatingAPP.Entities;
using WebAPIDatingAPP.Interfaces;

namespace WebAPIDatingAPP.DATA
{
    public class UserRepository : IRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppUsers> GetUserByIdAsync(int Id)
        {
            return await _context.AppUsers.FindAsync(Id);
        }

        public async Task<AppUsers> GetUserByNameAsync(string Name)
        {
            return await _context.AppUsers
                .Include(photo => photo.Photos)
                .SingleOrDefaultAsync(x => x.UserName == Name);
        }

        public async Task<IEnumerable<AppUsers>> GetUsersAsysc()
        {
            return await _context.AppUsers
                .Include(photo => photo.Photos).
                ToListAsync();
        }

        public async Task<bool> SaveAllAsysc()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUsers users)
        {
            _context.Entry(users).State = EntityState.Modified;

        }

        //member

        public async Task<MemberDTo> GetMemberByNameAsync(string Name)
        {
            //      return await _context.AppUsers
            //.Where(x => x.UserName == Name)
            //.Select(a => new MemberDTo
            //{

            //})
            //.SingleOrDefaultAsync();

            return await _context.AppUsers.Where(a => a.UserName == Name)
                .ProjectTo<MemberDTo>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();



        }

        public async Task<PageList<MemberDTo>> GetMemberAsync(UserParams userParams)
        {
            var query = _context.AppUsers.AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUserName);
            query = query.Where(u => u.Gender != userParams.Gender);


            return await PageList<MemberDTo>.CreateAsync(query.AsNoTracking()
            .ProjectTo<MemberDTo>(_mapper.ConfigurationProvider),
            userParams.PageNumber, userParams.PageSize);
        }
    }
}
