using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using EcoPowerHub.UOW;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EcoPowerHub.Repositories.Services
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly EcoPowerDbContext _context;

        public ProductRepository(  EcoPowerDbContext context, IMapper mapper) :base(context)
        {
            _mapper = mapper;
            _context= context;
        }
        public async Task<ResponseDto> GetAllProductsAsync()
        {
            var products = await _context.Products.AsNoTracking().ToListAsync();
            return products.Count == 0
                ? new ResponseDto
                {
                    Message = "Products retrieved successfully", 
                    IsSucceeded = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = _mapper.Map<IEnumerable<ProductDto>>(products) 
                }
                :new ResponseDto
                {
                    Message = "No Product Found",
                    IsSucceeded = true,
                    StatusCode = (int)HttpStatusCode.OK, 
                    Data = new List<ProductDto>()
                };

        }

        public async Task<ResponseDto> GetProductById(int id)
        {
            if (id <= 0)
            {
                return new ResponseDto
                {
                    Message = "Invalid Product Id ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return new ResponseDto
                {
                    Message = "Product not found",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            } 
            var productDto =  _mapper.Map<ProductDto>(product);
            return new ResponseDto
            {
                Message = "Product retrieved successfully!",
                IsSucceeded = true,
                StatusCode = 200,
                Data = productDto
            };
        }
        public async Task<ResponseDto> GetProductByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new ResponseDto
                {
                    Message = "Invalid Product name",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            var product = await _context.Products
                .Where(p => p.Name == name)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return new ResponseDto
                {
                    Message = "Product not found ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return new ResponseDto
            {
                Message = "Product retrieved successfully! ",
                IsSucceeded = true,
                StatusCode = 200,
                Data = productDto
            };
        }

        public async Task<ResponseDto> GetProductByCategory(string categoryName)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c=>c.Name == categoryName);
            if (category == null)
                return new ResponseDto
                {
                    Message = "No category found with given name!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            var products = await _context.Products.Where(p => p.CategoryId == category.Id).ToListAsync();
                
            if(products.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No products found for the given category",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return new ResponseDto
            {
                Message = "Products retrieved successfully! ",
                IsSucceeded = true,
                StatusCode = 200,
                Data = productDtos
            };
        }

        public async Task<ResponseDto> GetProductByCompany(string companyName)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == companyName);
            if (company == null)
                return new ResponseDto
                {
                    Message = "No company found with that name!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            var products = await _context.Products
                .Where(p=>p.CompanyId== company.Id)
                .AsNoTracking()
                .ToListAsync();
            if(products.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No products found for the given company",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return new ResponseDto
            {
                Message = "Products retrieved successfully! ",
                IsSucceeded = true,
                StatusCode = 200,
                Data = productDtos
            };
        }

     

        public async Task<ResponseDto> GetProductsSortedByPrice(string categoryName)
        {
             var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
            if (category == null)
                return new ResponseDto
                {
                    Message = "No category found with given name!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };

            var products = await _context.Products
                .Where(p => p.CategoryId == category.Id)
                .OrderBy(p => p.Price)
                .AsNoTracking()
                .ToListAsync();
            if (products.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No products found for this category ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return new ResponseDto
            {
                Message = "Product sorted by price ",
                IsSucceeded = true,
                StatusCode = 200,
                Data = productDtos
            };
        }

        public async Task<ResponseDto> AddProductAsync(ProductDto productDto)
        {
            if(productDto == null)
            {
                return new ResponseDto
                {
                    Message = "Invalid product data! ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            var existingProduct = await _context.Products.AnyAsync(p => p.Name == productDto.Name);
            if (existingProduct)
            {
                return new ResponseDto
                {
                    Message = "Product with the same name already exists!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.Conflict
                };
            }

            var product = _mapper.Map<Product>(productDto);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<ProductDto>(product);
            return new ResponseDto
            {
                Message = "Product added successfully! ",
                IsSucceeded = true,
                StatusCode = 200,
                Data = resultDto
            };
        }

        public async Task<ResponseDto> UpdateProductAsync(int id, ProductDto productDto)
        {
            if(productDto == null)
            {
                return new ResponseDto
                {
                    Message = "Invalid product data! ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            var existingProduct = await _context.Products.FindAsync(id);
            if(existingProduct == null)
            {
                return new ResponseDto
                {
                    Message = "Product not found! ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            _mapper.Map(productDto, existingProduct);
             _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Product Updated successfully! ",
                IsSucceeded = true,
                StatusCode = 200,
                Data = existingProduct
            };
        }

        public async Task<ResponseDto> DeleteProductAsync(int id)
        {
            if(id < 1)
            {
                return new ResponseDto
                {
                    Message = "Invalid product Id",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return new ResponseDto
                {
                    Message = "Product not found! ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
             _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Product deleted successfully! ",
                IsSucceeded = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> SearchProductAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new ResponseDto
                {
                    Message = "Invalid search term ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            var products = await _context.Products
                .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()))
                .AsNoTracking()
                .ToListAsync();
            if (products == null || !products.Any())
            {
                return new ResponseDto
                {
                    Message = "Product not found ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            var productDtos= _mapper.Map<IEnumerable<ProductDto>>(products);
            return new ResponseDto
            {
                Message = "Products retrieved successfully! ",
                IsSucceeded = true,
                StatusCode = 200,
                Data = productDtos
            };
        }
    }
}
