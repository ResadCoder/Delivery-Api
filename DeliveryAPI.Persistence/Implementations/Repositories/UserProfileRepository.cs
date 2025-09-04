using DeliveryAPI.Application.Abstractions.Repositories.UserProfiles;
using DeliveryAPi.Domain.Entities;
using DeliveryAPI.Persistence.Context;

namespace DeliveryAPI.Persistence.Implementations.Repositories;

internal class UserProfileRepository(AppDbContext context) 
    : GenericRepository<UserProfile>(context), IUserProfileRepository
{
}