using Microsoft.AspNetCore.Mvc;
using Shared.Events.Project;
using ProjectService.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Shared.EventBus;

namespace ProjectService.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    //private readonly IEventPublisher _eventPublisher;
    private readonly IMapper _mapper;
    public ProjectController(IProjectService projectService, IEventPublisher eventPublisher, IMapper mapper)
    {
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        //_eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    // Project endpoints

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProjectById(string id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject(ProjectCreateDto projectCreateDto)
    {
        var createdProject = await _projectService.CreateProjectAsync(projectCreateDto);
        return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, createdProject);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProject(string id, ProjectUpdateDto projectUpdateDto)
    {
        var result = await _projectService.UpdateProjectAsync(id, projectUpdateDto);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProject(string id)
    {
        var result = await _projectService.DeleteProjectAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    // Comment endpoints
   
    [HttpGet("{projectId}/comments")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByProjectId(string projectId)
    {
        var comments = await _projectService.GetCommentsByProjectIdAsync(projectId);

        return Ok(comments);
    }

    [HttpPost("{projectId}/comments")]
    public async Task<ActionResult> AddCommentToProject(string projectId, CommentCreateDto commentDto)
    {
        var comment = await _projectService.AddCommentToProjectAsync(projectId, commentDto);
        if (comment == null)
        {
            return NotFound();
        }

        return NoContent();
    }


    [HttpPut("comments/{commentId}")]
    public async Task<ActionResult> UpdateComment(string projectId, string commentId, CommentUpdateDto commentUpdateDto)
    {
        var result = await _projectService.UpdateCommentAsync(projectId, commentId, commentUpdateDto);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("comments/{commentId}")]
    public async Task<ActionResult> DeleteComment(string projectId, string commentId)
    {
        var result = await _projectService.DeleteCommentAsync(projectId, commentId);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    // Participant endpoints

    [HttpPost("{projectId}/participants/{participantId}")]
    public async Task<ActionResult> AddParticipantToProject(string projectId, string participantId)
    {
        var result = await _projectService.AddParticipantToProjectAsync(projectId, participantId);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{projectId}/participants/{participantId}")]
    public async Task<ActionResult> RemoveParticipantFromProject(string projectId, string participantId)
    {
        var result = await _projectService.RemoveParticipantFromProjectAsync(projectId, participantId);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("{projectId}/participants")]
    public async Task<ActionResult<IEnumerable<string>>> GetParticipantsOfProject(string projectId)
    {
        var participants = await _projectService.GetParticipantsOfProjectAsync(projectId);
        return Ok(participants);
    }
}