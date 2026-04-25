using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nipton.DataContext;
using Nipton.DataContext.Context;
using Nipton.DataContext.Dtos;
using Nipton.DataContext.Entities;
using System;
using System.Threading.Tasks;

namespace Nipton.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(UserRegisterDto dto);
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> UpdateAsync(int id, UserUpdateDto dto);
        Task DeactivateAsync(int id);
        Task ReactivateAsync(int id);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterAsync(UserRegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Ez az e-mail cím már foglalt!");

            var user = _mapper.Map<User>(dto);
            user.IsActive = true;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) throw new Exception("Felhasználó nem található!");
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) throw new Exception("Felhasználó nem található!");

            if (user.Email != dto.Email && await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Ez az e-mail cím már foglalt!");

            user.Username = dto.Username;
            user.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = dto.Password;

            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task DeactivateAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) throw new Exception("Felhasználó nem található!");
            user.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task ReactivateAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) throw new Exception("Felhasználó nem található!");
            user.IsActive = true;
            await _context.SaveChangesAsync();
        }
    }
}
