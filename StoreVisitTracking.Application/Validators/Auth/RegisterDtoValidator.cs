using FluentValidation;
using StoreVisitTracking.Application.DTOs.Auth;

namespace StoreVisitTracking.Application.Validators.Auth;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı boş olamaz")
            .Length(3, 100).WithMessage("Kullanıcı adı 3-100 karakter arasında olmalıdır");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır")
            .Length(6, 100).WithMessage("Şifre 6-100 karakter arasında olmalıdır")
            .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir")
            .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir")
            .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Şifreler eşleşmiyor");
    }
} 