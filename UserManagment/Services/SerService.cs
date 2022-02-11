using AutoMapper;
using Manager.Entities;
using Manager.Exeptions;
using Manager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Manager.Services
{
    public interface ISerService
    {
        Task<PageResult<ServiceDto>> GetAll(Query query);
        Task<ServiceDto> Get(int id);
        Task<PriceDto> GetPirce(int id);
        Task<ServiceDto> Update(ServiceDto dto);
        Task Delete(int id);
        Task<Service> Create(ServiceDto dto);
    }

    public class SerService : ISerService
    {
        private readonly ManagerDbContext _dBContext;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public SerService(ManagerDbContext dBContext, IMapper mapper, IFileService fileService)
        {
            _dBContext = dBContext;
            _mapper = mapper;
            _fileService = fileService;
        }


        public async Task<PageResult<ServiceDto>> GetAll(Query query)
        {
            var basequery = _dBContext.Services
                            .Where(r => query.searchPhrase == null || (r.Name.Contains(query.searchPhrase)));


            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Service, object>>>
                {
                    { nameof(User.Id), r => r.Id },
                    { nameof(User.Name), r => r.Name },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                basequery = query.SortDirection == SortDirection.ASC
                    ? basequery.OrderBy(selectedColumn)
                    : basequery.OrderByDescending(selectedColumn);
            }

            var services = await basequery

                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToListAsync();

            if (services is null)
                throw new NotFoundExeption("Services not found");

            var serviceDtos = _mapper.Map<List<ServiceDto>>(services);
            var totaItemsCount = basequery.Count();
            var result = new PageResult<ServiceDto>(serviceDtos, totaItemsCount, query.PageSize, query.PageNumber);

            return result;

        }

        public async Task<ServiceDto> Get(int id)
        {
            var service = await _dBContext.Services
                 .Where(c => c.Id == id)
                 .FirstOrDefaultAsync();

            if (service is null)
                throw new NotFoundExeption("Service not found");



            var serviceDto =   _mapper.Map<ServiceDto>(service);
            return serviceDto;
        }
        public async Task<PriceDto> GetPirce(int id)
        {
            var price = await _dBContext.Prices
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (price is null)
                throw new NotFoundExeption("Price not found");


            var PriceDto = _mapper.Map<PriceDto>(price);
            return PriceDto;

        }


        public async Task<ServiceDto> Update(ServiceDto dto)
        {

            var service = await _dBContext.Services
                             .Where(s => s.Id == dto.Id)
                             .FirstOrDefaultAsync();

            if (service is null)
                throw new NotFoundExeption("User not found");

            if (dto.ImageFile is not null && dto.ImageFile.Length > 0)
            {
                service.Image = $"/images/service/{dto.ImageFile.FileName}";
                await _fileService.UploadAsync(dto.ImageFile, "images/service");
            }
            else if (String.IsNullOrEmpty(dto.Image))
            {
                service.Image = "";
            }

            service.Name = dto.Name;
            service.Description = dto.Description;
            service.Slug = dto.Slug;
            service.IsActive = dto.IsActive;
            service.Tags = dto.Tags;
            service.SKU = dto.SKU;
            service.CategoryId = dto.CategoryId;


            await _dBContext.SaveChangesAsync();

            return dto;
        }


        public async Task Delete(int id)
        {
            var service = await _dBContext.Services
                 .Where(s => s.Id == id)
                 .FirstOrDefaultAsync();

            if (service is null)
                throw new NotFoundExeption("Category not found");

            _dBContext.Remove(service);
            await _dBContext.SaveChangesAsync();
        }


        public async Task<Service> Create(ServiceDto dto)
        {
            var service = new Service();
            if (dto.ImageFile is not null && dto.ImageFile.Length > 0)
            {
                service.Image = $"/images/service/{dto.ImageFile.FileName}";
                await _fileService.UploadAsync(dto.ImageFile, "images/service");

            }

            service.Name = dto.Name;
            service.Description = dto.Description;
            service.Slug = dto.Slug;
            service.SKU = dto.SKU;
            service.Tags = dto.Tags;
            service.IsActive = dto.IsActive;
            service.CategoryId = dto.CategoryId;

            _dBContext.Services.Add(service);

            await _dBContext.SaveChangesAsync();
            return service;
        }


    } 
}
