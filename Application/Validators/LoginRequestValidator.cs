using Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.StaffNo)
  .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Enter a valid value");
        }
    }
}
