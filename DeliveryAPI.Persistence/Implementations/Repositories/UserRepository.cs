using DeliveryAPI.Application.Abstractions.Repositories.Users;
using DeliveryAPi.Domain.Entities;
using DeliveryAPI.Persistence.Context;

namespace DeliveryAPI.Persistence.Implementations.Repositories;

internal class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
{
    
}