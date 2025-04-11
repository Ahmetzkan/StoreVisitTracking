using FluentValidation;
using StoreVisitTracking.Application.DTOs.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreVisitTracking.Application.Validators.Store
{
    public class UpdateStoreDtoValidator : AbstractValidator<UpdateStoreDto>
    {
        public UpdateStoreDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Location).NotEmpty();
        }
    }
}
