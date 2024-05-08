using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProfileService.DTOs;
using ProfileService.Repositories;
using Shared.EventBus;
using Shared.Events.Profile;

namespace ProfileService.Controllers;

[Route("api/profiles")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IProfileRepository _profileRepository;
    //private readonly IEventPublisher _eventPublisher;
    private readonly IMapper _mapper;

    public ProfileController(IProfileRepository profileRepository, IEventPublisher eventPublisher, IMapper mapper)
    {
        _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
        //_eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProfileDTO>>> GetAllProfiles()
    {
        var profiles = await _profileRepository.GetAllProfiles();
        return Ok(profiles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileDTO>> GetById(Guid id)
    {
        var profile = await _profileRepository.GetProfileById(id);

        if (profile == null)
        {
            return NotFound();
        }

        return Ok(profile);
    }

    [HttpPost]
    public async Task<ActionResult<ProfileDTO>> Post(ProfileDTO profileDto)
    {
        //The model binder automatically associates ModelState with profileDto
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var addedProfileDto = await _profileRepository.AddProfile(profileDto);


        return CreatedAtAction("GetById", new { id = addedProfileDto.Id }, addedProfileDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, ProfileDTO updatedProfileDto)
    {
        if (id != updatedProfileDto.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updateProfilea = await _profileRepository.UpdateProfile(updatedProfileDto);


        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _profileRepository.DeleteProfile(id);

        return NoContent();
    }
}