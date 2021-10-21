using AutoMapper;
using Manager.Entities;
using Manager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Manager.Services
{
    public interface IUserService
    {
        public List<UserDto> GetAll();
     
    }
    public class UserService : IUserService
    {
        private readonly ManagerDbContext _dBContext;
        private readonly IMapper _mapper;

        public UserService(ManagerDbContext dBContext, IMapper mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }
        public List<UserDto> GetAll()
        {
            var users = _dBContext.Users
                 .Include(u => u.Role).ToList();

            var usersDtos = _mapper.Map<List<UserDto>>(users);

            return usersDtos;
        }

 
    }
}
