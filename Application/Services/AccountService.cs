using Application.Dto;
using Application.Interface;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Application.Services
{
    public class AccountService : BaseRepository<tbl_StaffBioData>, IAccountRepo
    {
        protected ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountService(AppDbContext dbContext, ITokenService tokenService, IMapper mapper) : base(dbContext)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<AccountResponse> Login(LoginRequest request)
        {
            AccountResponse response = new AccountResponse();
            var result = await GetAllAsync();
            var res = result.FirstOrDefault(c => c.StaffNo == request.StaffNo);
            if (res != null)
            {
                if (BC.Verify(request.Password, res.Password))
                {
                    var gettoken = await _tokenService.GetUserToken(res.Id);
                    if (gettoken.RefreshToken != null)
                    {
                      
                        response.StaffName = res.StaffName;
                        response.Token = new AuthResultDto
                        {
                            ExpiresAt = gettoken.ExpiresAt,
                            RefreshToken = gettoken.RefreshToken,
                            Token = gettoken.Token
                        };
                        response.ResponseCode = "00";
                        response.ResponseDescription = "Operation Successful";
                        return response;
                    }
                    else
                    {
                        var token = await _tokenService.CreateToken(res);
                        if (token != null)
                        {
                            response.StaffName = res.StaffName;
                            response.Token = new AuthResultDto
                            {
                                ExpiresAt = token.ExpiresAt,
                                RefreshToken = token.RefreshToken,
                                Token = token.Token
                            };
                            response.ResponseCode = "00";
                            response.ResponseDescription = "Operation Successful";
                            return response;
                        }
                        else
                        {
                            response.ResponseCode = "99";
                            response.ResponseDescription = "Invalid Credentials";
                            return response;
                        }
                        
                    }
                }

            }
            response.ResponseCode = "99";
            response.ResponseDescription = "Invalid Credentials";
            return response;
        }

        public async Task<StaffDto> RegisterAsync(RegisterRequest request)
        {
            Random d = new Random();
            var Staffidinput = d.Next(100000, 999999).ToString();
            var result = _mapper.Map<tbl_StaffBioData>(request);
            result.StaffNo = Staffidinput;
            result.Password = BC.HashPassword(request.Password);
            await AddAsync(result);
            var res = _mapper.Map<StaffDto>(result);
            return res;
        }

        public async Task<bool> SignOut(string token)
        {
            var result = await _tokenService.RemoveToken(token);
            if (result == true)
            {
                return true;
            }
            return false;
        }
    }
}
