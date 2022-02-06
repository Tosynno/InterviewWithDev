using Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        
        public RegisterRequestValidator()
        {
        
            RuleFor(x => x.StaffName)
.NotEmpty().WithMessage("Enter a valid value");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Enter a valid value");

            RuleFor(x => x.ConfirmPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
    .NotEmpty().WithMessage("Enter a valid value")
    .When(x => x.Password != x.ConfirmPassword)
    .WithMessage("Password does not match");

        }

        //private bool MatchItems(string ConfirmPassword)
        //{
        //    RegisterRequest request = new RegisterRequest();
        //    if (request.Password == ConfirmPassword)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

    }
}
