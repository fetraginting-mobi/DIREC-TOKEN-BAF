using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Token_Ifinsys.Interfaces;
using Token_Ifinsys.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Token_Ifinsys.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {

        private readonly IAuthenticateService _authService;
        public AuthenticateController(IAuthenticateService authService)
        {
            _authService = authService;
        }

        //[AllowAnonymous]
        [HttpPost, Route("request")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult RequestToken([FromForm] IFormCollection data) // [FromHeader] TokenRequest request
        {

            //StringValues usernameVal;
            //StringValues passwordVal;

            TokenRequest request = new TokenRequest();
            DataList listData = new DataList();


            request.Username = data["username"];
            request.Password = data["password"];
            //request.ModuleName = data["moduleName"];

            // ini apabila memakai header
            //if (Request.Headers.TryGetValue("username", out usernameVal))
            //{
            //    request.Username = usernameVal.FirstOrDefault();
            //}
            //if (Request.Headers.TryGetValue("password", out passwordVal))
            //{
            //    request.Password = passwordVal.FirstOrDefault();
            //}
            // end

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string tokenApi;
            string messageApi;
            int statusApi;
            DataTable dataList;

            if (_authService.IsAuthenticated(request, out tokenApi, out statusApi, out messageApi, out dataList))
            {
                //return Ok(new { status = statusApi, token = tokenApi, message = messageApi });
                listData.status = statusApi;
                listData.token = tokenApi;
                listData.message = messageApi;
                listData.datalist = dataList;
                return Ok(listData);
            }

            return BadRequest("Invalid Request");
        }

        // GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
