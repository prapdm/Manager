﻿using AutoMapper;
using Manager.Entities;
using Manager.Models;

namespace Manager
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Service, ServiceDto>();
            CreateMap<Price, PriceDto>();

 
 

        }  
    }
}
