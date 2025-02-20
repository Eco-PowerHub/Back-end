using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.Helpers;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.UOW;
using Microsoft.EntityFrameworkCore;

namespace EcoPowerHub.Repositories.Services
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly EcoPowerDbContext _context;
        public CategoryRepository(EcoPowerDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _context = context;
        }

        public async Task<ResponseDto> GetAllAsync()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync(); 
            if (categories == null || !categories.Any())
            {
                return new ResponseDto
                {
                    Message = "No Categories Found",
                    IsSucceeded = true,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Data = new List<Category>()
                };
            }
            return new ResponseDto
            {
                Message = "Categories retrieved successfully!",
                IsSucceeded = true,
                StatusCode=200,
                Data = categories
            };
        }

        public async Task<ResponseDto> GetById(int id)
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
            return new ResponseDto
            {
                Message = "Category retrieved successfully",
                IsSucceeded = true,
                StatusCode = 200,
                Data = category
            };
        }


        public async Task<ResponseDto> GetByName(string name)
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

            return new ResponseDto
            {
                Message = "Category retrieved successfully.",
                IsSucceeded = true,
                StatusCode = 200,
                Data = category  
            };
        }

        public async Task<ResponseDto> AddAsync(Category category)
        {
            if (category == null)
            {
                return new ResponseDto
                {
                    Message = "Invalid Category Data",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

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

        public async Task<ResponseDto> DeleteAsync(int id)
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
            await _context.Categories.FindAsync(id);
            await _context.SaveChangesAsync();

            return new ResponseDto
            {
                Message = "Category deleted successfully! ",
                IsSucceeded = true,
                StatusCode = 200
            };
        }
       
        public async Task<ResponseDto> UpdateAsync(int id, Category category)
        {
            if (category == null)
            {
                return new ResponseDto
                {
                    Message = "Invalid Category data!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            var existingCategory = await _context.Categories.FindAsync(category.Id);
            if (existingCategory == null)
            {
                return new ResponseDto
                {
                    Message = "Category not found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            _context.Categories.Update(existingCategory);
            _context.SaveChangesAsync();

            return new ResponseDto
            {
                Message = "Category updated successfully! ",
                IsSucceeded = true,
                StatusCode = 200,
                Data = existingCategory
            };
        }
    }
}
