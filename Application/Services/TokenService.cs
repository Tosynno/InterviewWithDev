using Application.Dto;
using Application.Interface;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DataContext;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Application.Helpers;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly Jwt _jwt;
        public TokenService( IMapper mapper, IConfiguration config, AppDbContext context, IOptions<Jwt> jwt)
        {
            //_publishEndpoint = publishEndpoint;
            _jwt = jwt.Value;
            _mapper = mapper;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            _context = context;
        }
       
        public async Task<AuthResultDto> CreateToken(tbl_StaffBioData user)
        {
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.StaffName),
                }),
                Expires = DateTime.Now.AddMonths(3),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = tokenHandler.WriteToken(token);
            var refreshToken = new tbl_RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                StaffId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddDays(1),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };

            

            await _context.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResultDto()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };
            return response;
        }

        public async Task<AuthResultDto> GetUserToken(Guid id)
        {
            var result = new AuthResultDto();
            var gettoken = await _context.RefreshTokens.FirstOrDefaultAsync(c => c.StaffId == id && (c.DateExpire >= DateTime.Now || c.IsRevoked == false));

            if (gettoken != null)
            {
                result = new AuthResultDto
                {
                    ExpiresAt = gettoken.DateExpire,
                    RefreshToken = gettoken.Token,
                    Token = gettoken.Token
                };
            }
            


            return result;
        }
//        {
//  "staffNo": "871385",
//  "password": "tosin"
//}
    public async Task<bool> GetUserToken(string token)
        {
            var result = new AuthResultDto();
            var gettoken = await _context.RefreshTokens.FirstOrDefaultAsync(c => c.Token == token && (c.DateExpire >= DateTime.Now || c.IsRevoked == false));

            if (gettoken != null)
                return true;
            return false;
        }

        public Task<RefreshTokenDto> RefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveToken(string token)
        {
            var gettoken = await _context.RefreshTokens.FirstOrDefaultAsync(c => c.Token == token);
            gettoken.IsRevoked = true;

            _context.Update(gettoken);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
