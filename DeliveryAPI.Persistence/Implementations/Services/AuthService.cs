using System.Security.Claims;
using DeliveryAPI.Application.Abstractions;
using DeliveryAPI.Application.Abstractions.Repositories.Curier;
using DeliveryAPI.Application.Abstractions.Repositories.UserProfiles;
using DeliveryAPI.Application.Abstractions.Repositories.Users;
using DeliveryAPI.Application.Abstractions.UnitOfWork;
using DeliveryAPI.Application.DTOs;
using DeliveryAPi.Domain.Entities;
using DeliveryAPi.Domain.Entities.Base;
using DeliveryAPi.Domain.Enums;
using DeliveryAPI.Infrastructure.Models;
using Microsoft.AspNetCore.Http;

namespace DeliveryAPI.Persistence.Implementations.Services;

internal class AuthService(ICookieService cookieService, IUserRepository userRepository,IOtpService otpService
    ,IHashService hashService,IUnitOfWork unitOfWork,ICourierProfileRepository courierProfile
    ,IEmailService emailService,IJwtService jwtService,IHttpContextAccessor httpContextAccessor
    ,IRedisService redisService): IAuthService
{
    public async Task RegisterUserAsycn(UserRegisterDto dto,CancellationToken cancellationToken)
    {
      User? user = await userRepository.GetAnyAsync(u => u.Email == dto.Email, isTracking:true, includes:[nameof(User.UserProfile) , nameof(User.OtpCodes)], cancellationToken: cancellationToken);

      if (user is null)
      {
          if (await userRepository.isExistAsync(u => u.PhoneNumber == dto.PhoneNumber, cancellationToken))
              throw new Exception("User with the same phone number has already been registered");
          user = new User
          {
              Email = dto.Email,
              Name = dto.FirstName,
              Surname = dto.LastName,
              Role = UserRoleEnum.Customer,
              PhoneNumber = dto.PhoneNumber,
              UserProfile = new UserProfile
              {
                Address  = dto.Address
              },
              PasswordHash = hashService.Hash(dto.Password),
          };
          await userRepository.CreateAsync(user, cancellationToken);
      }
      else
      {
          if (user.CurierProfile is not null)
              throw new Exception("You have already registered");
          if (await userRepository.isExistAsync(u => u.PhoneNumber == dto.PhoneNumber, cancellationToken))
              throw new Exception("User with the same phone number has already been registered");
          user.UserProfile = new UserProfile
          {
            Address = dto.Address,
          };
          userRepository.Update(user);
      }
      await SendOtpToUserAndSaveChangesAsync(user);
    }

    public async Task RegisterCurierAsync(CurierrRegisterDto dto,CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetAnyAsync(u => u.Email == dto.Email,isTracking:true,includes:[nameof(User.CurierProfile), nameof(User.OtpCodes)], cancellationToken: cancellationToken);
        if (user is null)
        {
            if (await courierProfile.isExistAsync(cp => cp.Pin == dto.Pin, cancellationToken))
                throw new Exception("User with the same pin has already been registered");
            if (await userRepository.isExistAsync(u => u.PhoneNumber == dto.PhoneNumber, cancellationToken))
                throw new Exception("User with the same phone number has already been registered");
            user = new User
            {
                Email = dto.Email,
                Name = dto.FirstName,
                Surname = dto.LastName,
                Role = UserRoleEnum.Curier,
                PhoneNumber = dto.PhoneNumber,
                CurierProfile = new()
                {
                    Pin = dto.Pin,
                    Transport = dto.TransportEnumType,
                    TransportNumber = dto.TransportNumber
                },
                PasswordHash = hashService.Hash(dto.Password),
            };
            await userRepository.CreateAsync(user, cancellationToken);
        }
        else
        {
            if (user.CurierProfile is not null)
                throw new Exception("You have already registered");
            if(await courierProfile.isExistAsync(cp => cp.Pin == dto.Pin, cancellationToken))
                throw new Exception("User with the same pin has already been registered");
            user.CurierProfile = new()
            {
                Pin = dto.Pin,
                Transport = dto.TransportEnumType,
                TransportNumber = dto.TransportNumber
            };
            userRepository.Update(user);
        }

        await SendOtpToUserAndSaveChangesAsync(user);
    }

    public async Task<string> ConfirmEmailWithOtpAsync(ConfirmEmailWithOtpCodeDto dto,CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetAnyAsync(u => u.Email == dto.Email, isTracking:true, includes:[nameof(User.OtpCodes),
                         nameof(User.UserAuthSessions), nameof(User.CurierProfile),
                         nameof(User.UserProfile) ], cancellationToken: cancellationToken)
            ?? throw new Exception("User not found");
        
        if (user.EmailConfirmed)
            throw new Exception("Email already confirmed");
        
        if (!user.OtpCodes.Any(code => code.Code == dto.OtpCode && code.ExpirationDate > DateTime.UtcNow))
            throw new Exception("Invalid OTP code");
        
        user.EmailConfirmed = true;

        JwtTokenDto tokens = jwtService.GenerateJwtToken(user);
        
        user.UserAuthSessions.Add(new UserAuthSession
        {
            RefreshToken = tokens.RefreshToken,
            ExpiresAt = tokens.RefreshTokenExpireTime
        });
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        cookieService.SetCookie("Delivery_RefreshToken" , tokens.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = tokens.RefreshTokenExpireTime,
            SameSite = SameSiteMode.Lax
        });
        
        return tokens.AccessToken;
    }

    public async Task LogOutAsync(CancellationToken cancellationToken = default)
    {
        string refreshToken = cookieService.GetCookie("Delivery_RefreshToken") 
            ?? throw new Exception("You have not registered yet");
        
        User? user = await userRepository.GetAnyAsync(u => u.Id == int.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier))
            , cancellationToken: cancellationToken
            ,isTracking:true
            ,includes:[nameof(User.UserAuthSessions)])
            ?? throw new Exception("User not found");
        
        UserAuthSession? userAuthSession = user.UserAuthSessions.FirstOrDefault(au => au.RefreshToken == refreshToken)
            ?? throw new Exception("You are not logged in yet");
        
        user.UserAuthSessions.Remove(userAuthSession);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await redisService.SetAsync
            ("bl_accessToken:" + httpContextAccessor.HttpContext!.User.FindFirstValue("jti"),"true",cancellationToken, TimeSpan.FromMinutes(15));
        cookieService.RemoveCookie("Delivery_RefreshToken");
        
    }

    public async Task<string> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetAnyAsync(u => u.Email == dto.Email,isTracking:true, cancellationToken: cancellationToken,
                     includes: [nameof(User.CurierProfile),nameof(User.UserProfile)])
            ?? throw new Exception("User not found");
        if (!user.EmailConfirmed)
            throw new Exception("Email already confirmed");

        if (!hashService.VerifyHash(dto.Password, user.PasswordHash))
            throw new Exception("Invalid password");
        JwtTokenDto tokens = jwtService.GenerateJwtToken(user);
        
        user.UserAuthSessions.Add(new UserAuthSession
        {
            RefreshToken = tokens.RefreshToken,
            ExpiresAt = tokens.RefreshTokenExpireTime
        });
        
        cookieService.SetCookie("Delivery_RefreshToken", tokens.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = tokens.RefreshTokenExpireTime,
            SameSite = SameSiteMode.Lax,
            Secure = true 
        });
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return tokens.AccessToken;
    }

    private async Task SendOtpToUserAndSaveChangesAsync(User user)
    {
      
        var otpCode = otpService.GetOtpCode();
        var expirationTime = DateTime.UtcNow.AddMinutes(15);
        
        user.OtpCodes.Add(new OtpCode
        {
            Code = otpCode,
            ExpirationDate = expirationTime,
        });
        
        await unitOfWork.SaveChangesAsync();
        
        await emailService.SendConfirmationOtpAsync(user.Email, new SendConfirmOtpViewModel(otpCode, expirationTime));
    }
}