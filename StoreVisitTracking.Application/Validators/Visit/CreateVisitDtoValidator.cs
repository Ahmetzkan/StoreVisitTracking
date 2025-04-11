using FluentValidation;
using StoreVisitTracking.Application.DTOs.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreVisitTracking.Application.Validators.Visit
{
    public class CreateVisitDtoValidator : AbstractValidator<CreateVisitDto>
    {
        public CreateVisitDtoValidator()
        {
            RuleFor(x => x.StoreId).NotEmpty();
        }
    }
}
