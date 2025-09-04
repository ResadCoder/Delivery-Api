using DeliveryAPI.Application.Abstractions.Repositories.Generic;
using DeliveryAPi.Domain.Entities;

namespace DeliveryAPI.Application.Abstractions.Repositories.Users;

public interface IUserRepository: IGenericRepository<User>
{
}