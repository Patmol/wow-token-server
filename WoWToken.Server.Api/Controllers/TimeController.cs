using System;

using Microsoft.AspNetCore.Mvc;

namespace WoWToken.Server.Api.Controllers
{
    /// <summary>
    /// The time controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class TimeController : Controller
    {
        /// <summary>
        /// Return the current time.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(DateTime.Now);
        }
    }
}
