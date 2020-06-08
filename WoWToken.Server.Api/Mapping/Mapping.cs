using AutoMapper;

namespace WoWToken.Server.Api.Mapping
{
    /// <summary>
    /// The token models mapping.
    /// </summary>
    public class Mapping : Profile
    {
        /// <summary>
        /// Initialize a new instance of a <see cref="Mapping"/> class.
        /// </summary>
        public Mapping()
        {
            CreateMap<Data.Models.Database.WoWToken, ViewModels.Token>();
        }
    }
}
