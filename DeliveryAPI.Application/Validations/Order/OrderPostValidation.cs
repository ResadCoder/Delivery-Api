using DeliveryAPI.Application.DTOs.Orders;
using FluentValidation;

namespace DeliveryAPI.Application.Validations.Order;

public class OrderPostValidation : AbstractValidator<OrderPostDto>
{
    public OrderPostValidation()
    {
        RuleFor(x => x.ReceiverFullName)
            .NotEmpty().WithMessage("Receiver full name is required.")
            .MinimumLength(3).WithMessage("Receiver full name must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Receiver full name cannot exceed 50 characters.")
            .Matches("^[a-zA-Z ]+$").WithMessage("Receiver full name must consist only of letters and spaces.");

        RuleFor(x => x.ReceiverEmail)
            .NotEmpty().WithMessage("Receiver email is required.")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Invalid email address.");

        RuleFor(x => x.ReceiverPhoneNumber)
            .NotEmpty().WithMessage("Receiver phone number is required.")
            .Matches(@"^\+?\d{10,15}$")
            .WithMessage("Receiver phone number must be valid and contain 10-15 digits.");

        RuleFor(x => x.ReceiverAddress)
            .NotEmpty().WithMessage("Receiver address is required.")
            .MinimumLength(5).WithMessage("Receiver address must be at least 5 characters long.")
            .MaximumLength(100).WithMessage("Receiver address cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(150).WithMessage("Description cannot exceed 150 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
        
        // file edemmedim
    }
}