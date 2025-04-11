using FluentValidation;
using StoreVisitTracking.Application.DTOs.Photo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreVisitTracking.Application.Validators.Photo
{
    public class PhotoDtoValidator : AbstractValidator<PhotoDto>
    {
        public PhotoDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Base64Image).NotEmpty();
        }
    }
}
