using Api.Dtos;
using AutoMapper;
using Services.Models;

namespace Api.Mappings
{
    public class MatchMappings: Profile
    {
        public MatchMappings() {
            CreateMap<GameModel, GameModelDto>();
        }
    }
}