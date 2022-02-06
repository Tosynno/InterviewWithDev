using Application.Dto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ITokenService
    {
        Task<AuthResultDto> CreateToken(tbl_StaffBioData user);
        Task<RefreshTokenDto> RefreshToken(string token);
        Task<AuthResultDto> GetUserToken(Guid id);
        Task<bool> GetUserToken(string token);
        Task<bool> RemoveToken(string token);
    }
}
