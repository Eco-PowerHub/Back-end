using AutoMapper;
using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.DTO.UserSupportDto;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EcoPowerHub.Repositories.Services
{
    public class SupportRepository : GenericRepository<UserSupport>, ISupportRepository
    {
        private readonly EcoPowerDbContext _context;
        private readonly IMapper _mapper;

        #region Constructor
        public SupportRepository(EcoPowerDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        } 
        #endregion


        #region Service Implementation
        public async Task<ResponseDto> AddSupportAsync(CreateUserSupportDto supportDto)
        {
            if (supportDto == null)
            {
                return new ResponseDto
                {
                    Message = "Invalid support data! ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            var support = _mapper.Map<UserSupport>(supportDto);
            support.CreatedAt = DateTime.UtcNow;
            await _context.UserSupport.AddAsync(support);
            support.Response ??= "No Response yet"; // تعيين قيمة افتراضية

            await _context.SaveChangesAsync();
            var dto = _mapper.Map<CreateUserSupportDto>(support);
            return new ResponseDto
            {
                Message = "Support request send successfully! ",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = dto
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
            var supports = await _context.UserSupport.AsNoTracking().ToListAsync();
            if (!supports.Any())
            {
                return new ResponseDto
                {
                    Message = "No support requests found! ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Data = new List<GetUserSupportDto>()
                };
            }
            var responseDto = _mapper.Map<List<CreateUserSupportDto>>(supports);
            return new ResponseDto
            {
                Message = "Support requests retrieved successfully! ",
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
