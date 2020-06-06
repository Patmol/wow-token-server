using System;

using Microsoft.AspNetCore.Mvc;

namespace WoWToken.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TimeController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(DateTime.Now);
        }
    }
}
