using DeliveryAPI.Application.Abstractions.Repositories.Curier;
using DeliveryAPI.Application.Abstractions.Repositories.UserProfiles;
using DeliveryAPi.Domain.Entities;
using DeliveryAPI.Persistence.Context;

namespace DeliveryAPI.Persistence.Implementations.Repositories;

internal class CurierProfileRepository(AppDbContext dbContext)
    : GenericRepository<CurierProfile>(dbContext), ICourierProfileRepository;