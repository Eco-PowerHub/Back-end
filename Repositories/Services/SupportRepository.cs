using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserSupportDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace EcoPowerHub.Repositories.Services
{
    public class SupportRepository : GenericRepository<UserSupport>, ISupportRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region Constructor
        public SupportRepository(EcoPowerDbContext context, IMapper mapper,UserManager<ApplicationUser> userManager,IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion


        #region Service Implementation
        public async Task<ResponseDto> AddSupportAsync(CreateUserSupportDto supportDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (supportDto == null || string.IsNullOrWhiteSpace(userId))
            {
                return new ResponseDto
                {
                    Message = "Invalid support data.",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto
                {
                    Message = "User not found.",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                };
            }

            var support = _mapper.Map<UserSupport>(supportDto);
            support.UserId = userId;
            support.Response = "No Response yet";
            support.CreatedAt = DateTime.UtcNow;


            await _context.UserSupport.AddAsync(support);
            await _context.SaveChangesAsync();

            var resultDto = new GetUserSupportDto
            {
                Subject = support.Subject,
                Response = support.Response,
                CreatedAt = support.CreatedAt,
                UserId = user.Id,
                UserName = user.UserName! ,
                Email = user.Email! ,
                PhoneNumber = user.PhoneNumber!
            };

            return new ResponseDto
            {
                Message = "Support request submitted successfully.",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = resultDto
            };
        }

        public async Task<ResponseDto> DeleteSupportAsync(int id)
        {
            if (id < 1)
            {
                return new ResponseDto
                {
                    Message = "Invalid support Id! ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            var support = await _context.UserSupport.FindAsync(id);
            if (support == null)
            {
                return new ResponseDto
                {
                    Message = "Support request not found! ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            _context.UserSupport.Remove(support);
            await _context.SaveChangesAsync();

            return new ResponseDto
            {
                Message = "Support request deleted successfully! ",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public async Task<ResponseDto> GetAllSupportsAsync()
        {
            var supports = await _context.UserSupport
                .Include(s => s.User)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            if (supports.Count == 0)
            {
                return new ResponseDto
                {
                    Message = "No support requests found!",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Data = new List<GetUserSupportDto>()
                };
            }

            var responseDto = supports.Select(s => new GetUserSupportDto
            {
                Subject = s.Subject,
                Response = s.Response,
                CreatedAt = s.CreatedAt,
                UserId = s.UserId,
                UserName = s.User.UserName!,
                Email = s.User.Email!,
                PhoneNumber = s.User.PhoneNumber!
            }).ToList();

            return new ResponseDto
            {
                Message = "Support requests fetched successfully!",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = responseDto
            };
        }

        //public async Task<ResponseDto> GetSupportByIdAsync(int id)
        //{
        //    if (id < 1)
        //    {
        //        return new ResponseDto
        //        {
        //            Message = "Invalid support Id! ",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.BadRequest,
        //        };
        //    }
        //    var support= await _context.UserSupport.FindAsync(id);
        //    if (support == null)
        //    {
        //        return new ResponseDto
        //        {
        //            Message = "Support request not found! ",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.NotFound
        //        };
        //    }
        //    var responseDto = _mapper.Map<GetUserSupportDto>(support);
        //    return new ResponseDto
        //    {
        //        Message = "Support request retrieved successfully! ",
        //        IsSucceeded = true,
        //        StatusCode = (int)HttpStatusCode.OK,
        //        Data= responseDto
        //    };
        //}

        //public async Task<ResponseDto> AddResponseAsync(int id, AddResponseDto responseDto)
        //{
        //    if (id < 1 || string.IsNullOrWhiteSpace(responseDto.Response))
        //    {
        //        return new ResponseDto
        //        {
        //            Message = "Invalid data! ",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.BadRequest,
        //        };
        //    }
        //    var support = await _context.UserSupport.FindAsync(id);
        //    if (support == null)
        //    {
        //        return new ResponseDto
        //        {
        //            Message = "Support request not found! ",
        //            IsSucceeded = false,
        //            StatusCode = (int)HttpStatusCode.NotFound
        //        };
        //    }
        //    support.Response= responseDto.Response;
        //    await _context.SaveChangesAsync();

        //    var updatedSupportDto = _mapper.Map<GetUserSupportDto>(support);
        //    return new ResponseDto
        //    {
        //        Message = "Response added successfully!",
        //        IsSucceeded = true,
        //        StatusCode = (int)HttpStatusCode.OK,
        //        Data = updatedSupportDto
        //    };
        //}
    }
    #endregion
}
