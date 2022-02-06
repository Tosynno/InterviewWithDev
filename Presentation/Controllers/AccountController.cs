using Application.Dto;
using Application.Interface;
using Application.Models;
using Application.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        protected IAccountRepo _accountRepo;

        public AccountController(IAccountRepo accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AccountResponse>> Login(LoginRequest request)
        {
            LoginRequest respone = new LoginRequest();
            LoginRequestValidator validator = new LoginRequestValidator();
            ValidationResult results = validator.Validate(request);
            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    return BadRequest(failure.ErrorMessage);
                }
            }
            return Ok(await _accountRepo.Login(request));
        }


        [HttpPost("CreateAccount")]
        public async Task<ActionResult> CreateAccount(RegisterRequest request)
        {
            RegisterRequest respone = new RegisterRequest();
            RegisterRequestValidator validator = new RegisterRequestValidator();
            ValidationResult results = validator.Validate(request);
            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    return BadRequest(failure.ErrorMessage);
                }
            }
            var result = await _accountRepo.RegisterAsync(request);
            if (result!= null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Something went wrong, Please try again");
            }
            
        }


        [HttpPost("LogOut")]
        public async Task<ActionResult> SignOut(string token)
        {
            return Ok(await _accountRepo.SignOut(token));
        }
    }
}
