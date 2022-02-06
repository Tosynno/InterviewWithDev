using Application.Dto;
using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAccountRepo : IRepository<tbl_StaffBioData>
    {
        Task<AccountResponse> Login(LoginRequest request);
        Task<StaffDto> RegisterAsync(RegisterRequest request);
        Task<bool> SignOut(string token);
    }
}
