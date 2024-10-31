using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Token_Ifinsys.Models
{
    public static class Utilities
    {
        public static IActionResult StatusCode(int code, HttpContext httpContext)
        {
            // here is the trick
            httpContext.Response.StatusCode = code;

            //return a static file.
            try
            {
                return new VirtualFileResult("~/" + code + ".html", "text/html");
            }
            catch (FileNotFoundException)
            {
                httpContext.Response.StatusCode = 404;
                return new VirtualFileResult("~/404.html", "text/html");
            }
        }
    }
}
