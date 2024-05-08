using ProfileService.DTOs;

namespace ProfileService.Repositories;

public interface IProfileRepository
{
    Task<IEnumerable<ProfileDTO>> GetAllProfiles();
    Task<ProfileDTO> GetProfileById(Guid id);
    Task<ProfileDTO> AddProfile(ProfileDTO profileDto);
    Task<ProfileDTO> UpdateProfile(ProfileDTO updatedProfileDto);
    Task DeleteProfile(Guid id);
}