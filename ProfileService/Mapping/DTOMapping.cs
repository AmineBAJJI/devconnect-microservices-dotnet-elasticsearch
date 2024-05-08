using AutoMapper;
using ProfileService.DTOs;
using ProfileService.Models;
using Shared.Events.Profile;

namespace ProfileService.Mapping;

public class DTOMapping : Profile
{
    public DTOMapping()
    {
        CreateMap<ProfileInfra, ProfileDTO>();
        CreateMap<ProfileDTO, ProfileInfra>();

        CreateMap<ProfileInfra, ProfileDocument>();
        CreateMap<ProfileDocument, ProfileInfra>();

        CreateMap<ProfileDTO, ProfileDeleted>();
        CreateMap<ProfileDeleted, ProfileDTO>();
    }
}