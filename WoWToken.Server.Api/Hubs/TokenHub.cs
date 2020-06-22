using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WoWToken.Server.Api.Hubs.Services;
using WoWToken.Server.Data.Core;

namespace WoWToken.Server.Api.Hubs
{
    /// <summary>
    /// Contains the methods for the SignalR Hub for the token.
    /// </summary>
    public class TokenHub : ITokenHub
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// The SignalR hub context.
        /// </summary>
        private readonly IHubContext<SignalRHub> hubContext;

        /// <summary>
        /// The token service.
        /// </summary>
        private readonly ITokenService tokenService;

        /// <summary>
        /// Initialize an instance of a <see cref="TokenHub"/>.
        /// </summary>
        /// <param name="tokenService">The token service.</param>
        /// <param name="hubContext">A SignalR context.</param>
        /// <param name="mapper">The mapper.</param>
        public TokenHub(IHubContext<SignalRHub> hubContext, ITokenService tokenService, IMapper mapper)
        {
            this.hubContext = hubContext;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Send the information for all refresh tokens.
        /// </summary>
        /// <returns>A SignalR task.</returns>
        public async Task SendTokens()
        {
            var tokenInformations = await this.tokenService.GetAllLatestTokenInformationAsync();

            foreach (var tokenInformation in tokenInformations)
            {
                await hubContext.Clients.All.SendAsync(
                    HubMethods.UpdateTokenPrice,
                    this.mapper.Map<Data.Models.Database.WoWToken, ViewModels.Token>(tokenInformation));
            }
        }
    }
}
