using Application.Dto;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() {

            CreateMap<RefreshTokenDto, tbl_RefreshToken>().ReverseMap();
            CreateMap<RegisterRequest, tbl_StaffBioData>().ReverseMap();
            CreateMap<tbl_StaffBioData, StaffDto>().ReverseMap();
            CreateMap<tbl_Student, StudentDto>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.Id.ToString())).ReverseMap();
        }
    }
}
