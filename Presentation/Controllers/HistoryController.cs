using Application.Interface;
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
    public class HistoryController : ControllerBase
    {
        protected ICompareRepo _compareRepo;
        protected ITokenService _tokenService;

        public HistoryController(ICompareRepo compareRepo, ITokenService tokenService)
        {
            _compareRepo = compareRepo;
            _tokenService = tokenService;
        }

        [HttpGet("GetAllStudent")]
        public async Task<ActionResult> GetAllStudent()
        {
            string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            var result = await _tokenService.GetUserToken(auth[0].ToString());
            if (result == true)
            {
                return Ok(await _compareRepo.GetAllStudent());
            }
            else
            {
                return Unauthorized("Access Denied");
            }

        }

        [HttpGet("GetAllStudent/{Id}")]
        public async Task<ActionResult> GetAllStudent(string Id)
        {
            Guid id = Guid.Parse(Id);
            string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            var result = await _tokenService.GetUserToken(auth[0].ToString());
            if (result == true)
            {
                return Ok(await _compareRepo.GetAllStudent(id));
            }
            else
            {
                return Unauthorized("Access Denied");
            }

        }
    }
}
