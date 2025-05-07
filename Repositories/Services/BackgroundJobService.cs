using EcoPowerHub.Models;
using Hangfire;
using Microsoft.AspNetCore.Identity;

namespace EcoPowerHub.Repositories.Services
{
    public class BackgroundJobService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public BackgroundJobService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public void ScheduleUserDeletion(string userId, TimeSpan delay)
        {
            BackgroundJob.Schedule(() => DeleteUnverifiedUser(userId), delay);
        }

        public async Task DeleteUnverifiedUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && !user.IsConfirmed)
            {
                await _userManager.DeleteAsync(user);
            }
        }
    }
}
