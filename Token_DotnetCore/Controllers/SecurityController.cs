using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Token_Ifinsys.Interfaces;
using Token_Ifinsys.Models;

namespace Token_Ifinsys.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SecurityController : Controller
    {
        private readonly IAuthenticationService authService;

        public SecurityController(IAuthenticationService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        [Route("request")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var user = authService.Login(userName, password);
            if (null != user)
            {
                // create your login token here
                TokenRequest request = new TokenRequest();
                DataList listData = new DataList();


                request.Username = userName;
                request.Password = password;
                
                return Ok(request);
            }
            else
            {
                return Unauthorized();
            }
            //return BadRequest("Invalid Request");
        }
    }
}
