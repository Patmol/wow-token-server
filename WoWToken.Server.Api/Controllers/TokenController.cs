using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WoWToken.Server.Api.ViewModels;
using WoWToken.Server.Data.Core;

namespace WoWToken.Server.Api.Controllers
{
    /// <summary>
    /// The token controller.
    /// </summary>
    [Produces("application/json")]
    [Route("token")]
    [ApiController]
    public class TokenController : Controller
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// The token service.
        /// </summary>
        private ITokenService tokenService;

        /// <summary>
        /// Initialize a new instance of a <see cref="TokenController"/> class.
        /// </summary>
        /// <param name="tokenService">The token service.</param>
        /// <param name="mapper">The mapper.</param>
        public TokenController(ITokenService tokenService, IMapper mapper)
        {
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets the latest information for a WoW Token for a specific region.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /token?region=eu
        ///
        /// </remarks>
        /// <param name="region">The region for which we want the information.</param>
        /// <returns>The token information.</returns>
        /// <response code="200">Return the token information for the provided region.</response>
        /// <response code="400">If no token information can be found for the specified region.</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Token>> GetLatestAync([Required] string region)
        {
            var token = await tokenService.GetLatestTokenInformationAsync(region);

            if (token == null)
            {
                return BadRequest();
            }

            return Ok(this.mapper.Map<Data.Models.Database.WoWToken, ViewModels.Token>(token));
        }
    }
}
