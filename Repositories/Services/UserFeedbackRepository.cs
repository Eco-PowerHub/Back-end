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
    public class UserFeedbackRepository : GenericRepository<UserFeedBack>, IUserFeedbackRepository
    {
        private readonly EcoPowerDbContext _context;

        public UserFeedbackRepository(EcoPowerDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<ResponseDto> GetAllFeedbacksAsync()
        {
            var feedbacks = await _context.UserFeedBacks.AsNoTracking().ToListAsync();
            if(!feedbacks.Any() || feedbacks==null)
            {
                return new ResponseDto
                {
                    Message= "No Feedbacks found!",
                    IsSucceeded=false,
                    StatusCode=(int)HttpStatusCode.NotFound,
                    Data= new List<UserFeedBack>()
                };
            }
            return new ResponseDto
            {
                Message = "Feedbacks retrieved successfully! ",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = feedbacks
            };
        }
        public async Task<ResponseDto> GetFeedbackByIdAsync(int id)
        {
            if (id <= 0)
            {
                return new ResponseDto
                {
                    Message = "Invalid feedback Id",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            var feedback = await _context.UserFeedBacks.FindAsync(id);
            if (feedback == null)
            {
                return new ResponseDto
                {
                    Message = "Feedback not found",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            return new ResponseDto
            {
                Message = "Feedback retrieved successfully! ",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = feedback
            };
        }
        public async Task<ResponseDto> AddFeedbackAsync(UserFeedBack feedback)
        {
            if (feedback == null || string.IsNullOrWhiteSpace(feedback.Content))
            {
                return new ResponseDto
                {
                    Message = "Invalid feedback Data! ",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            feedback.CreatedAt = DateTime.UtcNow;
            await _context.UserFeedBacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Feedback added successfully! ",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.Created,
                Data = feedback
            };
        }
      
        public async Task<ResponseDto> DeleteFeedbackAsync(int feedbackId)
        {

            if (feedbackId <= 0)
            {
                return new ResponseDto
                {
                    Message = "Invalid feedback Id",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            var feedback = await _context.UserFeedBacks.FindAsync(feedbackId);
            if (feedback == null)
            {
                return new ResponseDto
                {
                    Message = "Feedback not found",
                    IsSucceeded = false,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            _context.UserFeedBacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Feedback deleted successfully! ",
                IsSucceeded = true,
                StatusCode = (int)HttpStatusCode.OK
            };
        }

    }
}
