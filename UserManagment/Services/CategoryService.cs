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

    public interface ICategoryService
    {
        Task<PageResult<CategoryDto>> GetAll(Query query);
        Task<List<Category>> GetAll();
        Task<List<Category>> GetSubCategories(int cid);
        Task<Category> Create(CategoryDto dto);
        Task Delete(int id);
        Task<CategoryDto> Get(int id);
        Task<CategoryDto> Update(CategoryDto dto);

    }

    public class CategoryService : ICategoryService
    {
        private readonly ManagerDbContext _dBContext;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CategoryService(ManagerDbContext dBContext, IMapper mapper, IFileService fileService)
        {
            _dBContext = dBContext;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<PageResult<CategoryDto>> GetAll(Query query)
        {
            var basequery = _dBContext.Categories
                            .Include(u => u.Children)
                            .Where(r => query.searchPhrase == null || (r.Name.Contains(query.searchPhrase)
                                                                 
                                                                   ));
                            



            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Category, object>>>
                {
                    { nameof(User.Id), r => r.Id },
                    { nameof(User.Name), r => r.Name },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                basequery = query.SortDirection == SortDirection.ASC
                    ? basequery.OrderBy(selectedColumn)
                    : basequery.OrderByDescending(selectedColumn);
            }

            basequery = basequery.Where(x => x.Parent == null).Include(x => x.Children).ThenInclude(x => x.Children);

            var categories = await basequery
                
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToListAsync();

            if (categories is null)
                throw new NotFoundExeption("Categories not found");

            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
            var totaItemsCount = basequery.Count();
            var result = new PageResult<CategoryDto>(categoryDtos, totaItemsCount, query.PageSize, query.PageNumber);

            return result;
        }

        public async Task<List<Category>> GetSubCategories(int cid)
        {
            var subcategories = await _dBContext.Categories
                            .Where(r => r.ParentId == cid).ToListAsync();

            return subcategories;
        }

        public async Task<List<Category>> GetAll()
        {
            var categories = await _dBContext.Categories
                 .Where(r => r.IsActive == true)
                 .ToListAsync();

            if (categories is null)
                throw new NotFoundExeption("Categories not found");

            return categories;
        }

        public async Task<Category> Create(CategoryDto dto)
        {
            var category = new Category();
            if (dto.ImageFile is not null && dto.ImageFile.Length > 0)
            {
                category.Image = $"/images/category/{dto.ImageFile.FileName}";
                await _fileService.UploadAsync(dto.ImageFile, "images/category");

            }

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.Icon = dto.Icon;
            category.Slug = dto.Slug;
            category.IsActive = dto.IsActive;
            category.ParentId = dto.ParentId;

            _dBContext.Categories.Add(category);

            await _dBContext.SaveChangesAsync();
            return category;
        }

        public async Task Delete(int id)
        {
            var category = await _dBContext.Categories
                 .Where(u => u.Id == id)
                 .FirstOrDefaultAsync();

            if (category is null)
                throw new NotFoundExeption("Category not found");

            _dBContext.Remove(category);
            await _dBContext.SaveChangesAsync();
        }

        public async Task<CategoryDto> Get(int id)
        {
            var category = await _dBContext.Categories
                 .Where(c => c.Id == id)
                 .FirstOrDefaultAsync();

            if (category is null)
                throw new NotFoundExeption("Category not found");

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task<CategoryDto> Update(CategoryDto dto)
        {
            var category = await _dBContext.Categories
                 .Where(c => c.Id == dto.Id)
                 .FirstOrDefaultAsync();

            if (category is null)
                throw new NotFoundExeption("User not found");

            if (dto.ImageFile is not null && dto.ImageFile.Length > 0)
            {
                category.Image = $"/images/category/{dto.ImageFile.FileName}";
                await _fileService.UploadAsync(dto.ImageFile, "images/category");
            } else if (String.IsNullOrEmpty(dto.Image))
            {
                category.Image = "";
            }
        
            category.Name = dto.Name;
            category.Description = dto.Description;
            category.Icon = dto.Icon;
            category.Slug = dto.Slug;
            category.IsActive = dto.IsActive;
            category.ParentId = dto.ParentId;

   

            await _dBContext.SaveChangesAsync();

            return dto;
        }

      
    }
}