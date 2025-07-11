﻿using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.Helpers;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcoPowerHub.Repositories.Services
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly IMapper _mapper;

        #region Constructor
        public CategoryRepository(EcoPowerDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Service Implementation
        public async Task<ResponseDto> GetAllCategories()
        {

            var categories = await _context.Categories.AsNoTracking().Include(p => p.Products).ToListAsync();
            if (categories.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No Categories Found",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Data = new List<Category>()
                };
            }
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return new ResponseDto
            {
                Message = "Categories retrieved successfully",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = categoryDtos
            };
        }

        public async Task<ResponseDto> GetCategoryById(int id)
        {
            if (id <= 0)
                return new ResponseDto
                {
                    Message = "Invalid Category id",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return new ResponseDto
                {
                    Message = "Category not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new ResponseDto
            {
                Message = "Category retrieved successfully",
                IsSucceeded = true,
                StatusCode = 200,
                Data = categoryDto
            };
        }


        public async Task<ResponseDto> GetCategoryByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new ResponseDto
                {
                    Message = "Invalid category name.",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
            if (category == null)
            {
                return new ResponseDto
                {
                    Message = "Category not found",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new ResponseDto
            {
                Message = "Category retrieved successfully.",
                IsSucceeded = true,
                StatusCode = 200,
                Data = categoryDto
            };
        }

        public async Task<ResponseDto> AddCategoryAsync(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return new ResponseDto
                {
                    Message = " Category Data is required",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            var existingCategory = await _context.Categories.AnyAsync(c => c.Name == categoryDto.Name);
            if (existingCategory)
            {
                return new ResponseDto
                {
                    Message = "Category already exists!",
                    IsSucceeded = false,
                    StatusCode = 409
                };
            }

            var category = _mapper.Map<Category>(categoryDto);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Category added successfully!",
                IsSucceeded = true,
                StatusCode = 200,
                Data = category
            };
        }

        public async Task<ResponseDto> DeleteCategoryAsync(int id)
        {
            if (id <= 0)
            {
                return new ResponseDto
                {
                    Message = "Invalid Category id",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return new ResponseDto
                {
                    Message = "Category not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }

            if (await _context.Products.AnyAsync(p => p.CategoryId == id))
            {
                return new ResponseDto
                {
                    Message = "Cannot delete category with associated products.",
                    IsSucceeded = false,
                    StatusCode = 409
                };
            }


            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return new ResponseDto
            {
                Message = "Category deleted successfully! ",
                IsSucceeded = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            if (categoryDto == null || id <= 0)
            {
                return new ResponseDto
                {
                    Message = "Invalid Category data or ID!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c=>c.Id == id);
                //.Include(c => c.Products)
               
            if (existingCategory == null)
            {
                return new ResponseDto
                {
                    Message = "Category not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            _mapper.Map(categoryDto, existingCategory);
            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();

            return new ResponseDto
            {
                Message = "Category updated successfully with products!",
                IsSucceeded = true,
                StatusCode = 200,
                Data = existingCategory
            };
        } 
        #endregion

    }
}
