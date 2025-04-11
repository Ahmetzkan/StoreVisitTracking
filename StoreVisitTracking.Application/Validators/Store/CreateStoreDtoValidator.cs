using FluentValidation;
using StoreVisitTracking.Application.DTOs;
using StoreVisitTracking.Application.DTOs.Store;

public class CreateStoreDtoValidator : AbstractValidator<CreateStoreDto>
{
    public CreateStoreDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Mağaza adı boş olamaz.");
        RuleFor(x => x.Location).NotEmpty().WithMessage("Konum bilgisi boş olamaz.");
    }
}
