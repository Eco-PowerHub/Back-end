using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;

namespace EcoPowerHub.Repositories.Interfaces
{
    public interface IUserFeedbackRepository : IGenericRepository<UserFeedBack>
    {
        Task<ResponseDto> GetAllFeedbacksAsync();
        Task<ResponseDto> GetFeedbackByIdAsync(int id);
        Task<ResponseDto> AddFeedbackAsync(UserFeedBack feedback);
        Task<ResponseDto> DeleteFeedbackAsync(int feedbackId);
    }
}
