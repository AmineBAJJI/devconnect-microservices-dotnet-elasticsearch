using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.DTOs;
using ProfileService.Models;
using Shared.EventBus;
using Shared.Events.Profile;

namespace ProfileService.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    public ProfileRepository(AppDbContext dbContext, IMapper mapper, IEventPublisher eventPublisher)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
    }

    public async Task<IEnumerable<ProfileDTO>> GetAllProfiles()
    {
        var profiles = await _dbContext.Profiles.ToListAsync();

        return _mapper.Map<IEnumerable<ProfileDTO>>(profiles);
    }

    public async Task<ProfileDTO> GetProfileById(Guid id)
    {
        var profile = await _dbContext.Profiles.FindAsync(id);

        return profile != null ? _mapper.Map<ProfileDTO>(profile) : null;
    }

    public async Task<ProfileDTO> AddProfile(ProfileDTO profileDto)
    {
        var profileInfra = _mapper.Map<ProfileInfra>(profileDto);
        var addedEntityEntry = await _dbContext.Profiles.AddAsync(profileInfra);
        await _dbContext.SaveChangesAsync();
        var addedProfileInfra = addedEntityEntry.Entity;
        await _eventPublisher.PublishAsync(_mapper.Map<ProfileDocument>(addedProfileInfra));

        return _mapper.Map<ProfileDTO>(addedProfileInfra);
    }

    public async Task<ProfileDTO> UpdateProfile(ProfileDTO updatedProfileDto)
    {
        var updatedProfileInfra = _mapper.Map<ProfileInfra>(updatedProfileDto);
        var updatedEntityEntry = _dbContext.Profiles.Update(updatedProfileInfra);
        await _dbContext.SaveChangesAsync();
        await _eventPublisher.PublishAsync(_mapper.Map<ProfileDocument>(updatedEntityEntry.Entity));
        return _mapper.Map<ProfileDTO>(updatedEntityEntry.Entity);
    }

    public async Task DeleteProfile(Guid id)
    {
        var profileToDelete = await _dbContext.Profiles.FindAsync(id);

        if (profileToDelete != null)
        {
            _dbContext.Profiles.Remove(profileToDelete);
            await _dbContext.SaveChangesAsync();
            await _eventPublisher.PublishAsync(new ProfileDeleted
            {
                Id = id
            });
        }
    }
}