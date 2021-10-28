using AutoMapper;
using Manager.Entities;
using Manager.Exeptions;
using Manager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Manager.Services
{
    public interface IUserService
    {
        Task<PageResult<UserDto>> GetAll(Query query);
        Task<UserDto> Get();
        Task<List<Role>> GetRoles();
        Task<UserDto> SaveProfile(UserDto dto);
        Task<UserDto> Update(UserDto dto);
        Task<UserDto> GetUser(int? id);
        Task<UserDto> EditUser(int? id);
        Task Delete(int id);
        Task<User> Create(RegisterUserDto dto);
    }
    public class UserService : IUserService
    {
        private readonly ManagerDbContext _dBContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(ManagerDbContext dBContext, IMapper mapper, 
            IUserContextService userContextService, IPasswordHasher<User> passwordHasher)
        {
            _dBContext = dBContext;
            _mapper = mapper;
            _userContextService = userContextService;
            _passwordHasher = passwordHasher;
        }
        public async Task<PageResult<UserDto>> GetAll(Query query)
        {
            var basequery = _dBContext.Users
                 .Include(u => u.Role)
                 .Where(r => query.searchPhrase == null || (r.Name.Contains(query.searchPhrase)
                                                       || r.Surname.Contains(query.searchPhrase)
                                                       || r.Email.Contains(query.searchPhrase)
                                                       || r.Role.Name.Contains(query.searchPhrase)
                                                        ));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<User, object>>>
                {
                    { nameof(User.Id), r => r.Id },
                    { nameof(User.Name), r => r.Name },
                    { nameof(User.Surname), r => r.Surname },
                    { nameof(User.Email), r => r.Email },
                    { nameof(User.Role), r => r.Role },
                    { nameof(User.IsActive), r => r.IsActive },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                basequery = query.SortDirection == SortDirection.ASC
                    ? basequery.OrderBy(selectedColumn)
                    : basequery.OrderByDescending(selectedColumn);

            }

            var users = await basequery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToListAsync();

            if (users is null)
                throw new NotFoundExeption("Users not found");

            var usersDtos = _mapper.Map<List<UserDto>>(users);
            var totaItemsCount = basequery.Count();
            var result = new PageResult<UserDto>(usersDtos, totaItemsCount, query.PageSize, query.PageNumber);

            return result;
        }
        public async Task<List<Role>> GetRoles()
        {
            var roles = await _dBContext.Roles
                 .ToListAsync();

            if (roles is null)
                throw new NotFoundExeption("Roles not found");

            return roles;
        }
        

        public async Task<UserDto> Get()
        {
            var user = await _dBContext.Users
                 .Where(u => u.Id == (int)_userContextService.GetUserId)
                 .Include(u => u.Role)
                 .FirstOrDefaultAsync();

            if (user is null)
                throw new NotFoundExeption("User not found");

            var usersDto = _mapper.Map<UserDto>(user);
            return usersDto;
        }

        public async Task<UserDto> GetUser(int? id)
        {
            if (id is null)
                throw new NotFoundExeption("User not found");

            var user = await _dBContext.Users
                 .Where(u => u.Id == id)
                 .Include(u => u.Role)
                 .FirstOrDefaultAsync();

            if(user is null)
                throw new NotFoundExeption("User not found");

            var usersDto = _mapper.Map<UserDto>(user);
            return usersDto;
        }
        public async Task<UserDto> EditUser(int? id)
        {
            if (id is null)
                throw new NotFoundExeption("User not found");

            var user = await _dBContext.Users
                 .Where(u => u.Id == id)
                 .Include(u => u.Role)
                 .FirstOrDefaultAsync();

            if (user is null)
                throw new NotFoundExeption("User not found");

            var usersDto = _mapper.Map<UserDto>(user);
            return usersDto;
        }

        public async Task<UserDto> Update(UserDto dto)
        {
            var user = await _dBContext.Users
                 .Where(u => u.Id == dto.Id)
                 .FirstOrDefaultAsync();

            if (user is null)
                throw new NotFoundExeption("User not found");

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;
            user.RoleId = dto.RoleId;
            user.Phone = dto.Phone;
            user.IsActive = dto.IsActive;

            await _dBContext.SaveChangesAsync();

            return dto;
        }
        public async Task<User> Create(RegisterUserDto dto)
        {
            Random rnd = new();
            dto.VerifcationCode = rnd.Next(10000, 99999);
            var newUser = new User()
            {
                Email = dto.Email,
                Name = dto.Name,
                Surname = dto.Surname,
                Phone = dto.Phone,
                RoleId = dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;
            _dBContext.Users.Add(newUser);
            await _dBContext.SaveChangesAsync();
            return newUser;
        }

            
        public async Task Delete(int id)
        {
            var user = await _dBContext.Users
                 .Where(u => u.Id == id)
                 .FirstOrDefaultAsync();

            if (user is null)
                throw new NotFoundExeption("User not found");

            _dBContext.Remove(user);
            await _dBContext.SaveChangesAsync();
        }

        

        public async Task<UserDto> SaveProfile(UserDto dto)
        {
            var user = await _dBContext.Users
                 .Where(u => u.Id == (int)_userContextService.GetUserId)
                 .FirstOrDefaultAsync();

            if (user is null)
                throw new NotFoundExeption("User not found");

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;
            user.Phone = dto.Phone;

            await _dBContext.SaveChangesAsync();
           
            return dto;
        }

    }
}
