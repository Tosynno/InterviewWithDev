using Application.Dto;
using Application.Interface;
using Application.Models;
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
    public class CompareController : ControllerBase
    {
        protected ICompareRepo _compareRepo;
        protected ITokenService _tokenService;

        public CompareController(ICompareRepo compareRepo, ITokenService tokenService)
        {
            _compareRepo = compareRepo;
            _tokenService = tokenService;
        }

        [HttpPost("InsertStudentRecord")]
        public async Task<ActionResult> InsertStudentRecord([FromForm] StudentRequest request)
        {
            string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            var result = await _tokenService.GetUserToken(auth[0].ToString());
            if (result == true)
            {
                return Ok(await _compareRepo.InsertStudentRecord(request));
            }
            else
            {
                return Unauthorized("Access Denied");
            }
            
        }

        [HttpGet("CompareAllStudent")]
        public async Task<ActionResult<List<StudentCompareDto>>> CompareAllStudent()
        {
            string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            var result = await _tokenService.GetUserToken(auth[0].ToString());
            if (result == true)
            {
                var res = await _compareRepo.CompareAllStudent();
                if (res.Count() > 0)
                {
                    return Ok(res);
                }
                else
                {
                    return BadRequest("No record Found");
                }
               
            }
            else
            {
                return Unauthorized("Access Denied");
            }

        }
    }
}
