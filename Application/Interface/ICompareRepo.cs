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
    public interface ICompareRepo: IRepository<tbl_Student>
    {
        Task<string> InsertStudentRecord(StudentRequest request);
        Task<List<StudentCompareDto>> CompareAllStudent();
        Task<List<StudentDto>> GetAllStudent();
        Task<StudentDto> GetAllStudent(Guid Id);
    }
}
