using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization; 
using API.Models.Auth; 

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IManager _manager; 
        
        public AuthController(IManager manager)
        {
            this._manager = manager; 
        }

        [AllowAnonymous] 
        [HttpPost("login")] 
        public ActionResult<string> Login(login_Request param)
        {
            var token = _manager.Authtenticate(param.userName, param.password); 

            return Ok(token); 

        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("do")]
        public ActionResult<string> getDo()
        {
            return Ok("Heyy"); 
        }

    }
}
