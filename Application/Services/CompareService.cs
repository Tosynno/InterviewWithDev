using Application.Interface;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Application.Dto;

namespace Application.Services
{
    public class CompareService : BaseRepository<tbl_Student>, ICompareRepo
    {
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _hostingEnvironment;
        public CompareService(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async  Task<List<StudentCompareDto>> CompareAllStudent()
        {
            FileStream fs1;
            FileStream fs2;
            int fl1byte;
            int fl2byte;
            List<StudentCompareDto> allResult = new List<StudentCompareDto>();
            var res = await GetAllAsync();
            foreach (var item in res)
            {
                foreach (var itemdesc in res.OrderByDescending(c => c.Id))
                {
                    if (item.Id == itemdesc.Id)
                    {
                        Console.WriteLine("");
                    }
                    else
                    {
                         fs1 = new FileStream(item.FilePath, FileMode.Open);
                         fs2 = new FileStream(itemdesc.FilePath, FileMode.Open);

                        if (fs1.Length != fs2.Length)
                        {
                            fs1.Close();
                            fs2.Close();
                        }
                        else
                        {
                            do
                            {
                                fl1byte = fs1.ReadByte();
                                fl2byte = fs2.ReadByte();
                            }
                            while ((fl1byte == fl2byte) && (fl1byte != -1));

                            fs1.Close();
                            fs2.Close();

                            if ((fl1byte - fl2byte) == 0)
                            {
                                StudentCompareDto dto = new StudentCompareDto();
                                dto.Name = item.FirstName + " " + item.FirstName + " " + "&" + " " + itemdesc.FirstName + " " + itemdesc.FirstName;
                                allResult.Add(dto);
                            }
                        }

                    }
                }
            }

            return allResult;
        }

        public async Task<List<StudentDto>> GetAllStudent()
        {
            var result = await GetAllAsync();
            var res = _mapper.Map<List<StudentDto>>(result);
            return res;
        }

        public async Task<StudentDto> GetAllStudent(Guid Id)
        {
            var result = await GetByIdAsync(Id);
            var res = _mapper.Map<StudentDto>(result);
            return res;
        }

        public async Task<string> InsertStudentRecord(StudentRequest request)
        {
            string webRootPath = "C:\\AccountTagging\\UpLoadDoc";//_hostingEnvironment.WebRootPath + @"\Uploads\";
            if (!Directory.Exists(webRootPath))
            {
                Directory.CreateDirectory(webRootPath);
            }
            var formFile = request.UploadFile;
            var fileExt = request.FirstName + "_" + Guid.NewGuid().ToString() + "_" + formFile.FileName;
            var filePath = Path.Combine(webRootPath, fileExt);



            using (var stream = System.IO.File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
            }
            tbl_Student sa = new tbl_Student();
            sa.FirstName = request.FirstName;
            sa.LastName = request.LastName;
            sa.FileName = fileExt;
            sa.FilePath = filePath;

            await AddAsync(sa);

            return "IsCreated";
        }
    }
}
