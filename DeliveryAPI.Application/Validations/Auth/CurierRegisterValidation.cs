using DeliveryAPI.Application.DTOs;
using FluentValidation;

namespace DeliveryAPI.Application.Validations.Auth;

public class CurierRegisterValidation: AbstractValidator<CurierrRegisterDto>
{
    public CurierRegisterValidation()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Firstname is required.")
            .MinimumLength(3).WithMessage("Firstname must be at least 3 characters long.")
            .MaximumLength(15).WithMessage("Firstname cannot exceed 15 characters.")
            .Matches("^[a-zA-Z]+$").WithMessage("Firstname must consist only of letters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Lastname is required.")
            .MinimumLength(3).WithMessage("Lastname must be at least 3 characters long.")
            .MaximumLength(15).WithMessage("Lastname cannot exceed 15 characters.")
            .Matches("^[a-zA-Z]+$").WithMessage("Lastname must consist only of letters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(20).WithMessage("Password cannot exceed 20 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Phone number must be valid and contain 10-15 digits.");

        RuleFor(x => x.Pin)
            .NotEmpty().WithMessage("Pin is required.");

        RuleFor(x => x.TransportEnumType)
            .IsInEnum().WithMessage("Transport type must be valid.");

        RuleFor(x => x.TransportNumber)
            .MaximumLength(10).WithMessage("Transport number cannot exceed 10 characters.")
            .When(x => !string.IsNullOrEmpty(x.TransportNumber));
    }
}