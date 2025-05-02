using EcoPowerHub.Data;
using EcoPowerHub.DTO;
using EcoPowerHub.Models;
using EcoPowerHub.Repositories.GenericRepositories;
using EcoPowerHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcoPowerHub.Repositories.Services
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly EcoPowerDbContext _context;
        public OrderRepository(EcoPowerDbContext context) : base(context)
        {
            _context = context;
        }
      
    }
}
