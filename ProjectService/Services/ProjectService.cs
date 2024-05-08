using AutoMapper;
using Events.MongoDB;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Events.Project;
using ProjectService.Models;
using Shared.EventBus;

namespace ProjectService.Services;

public class ProjectService : IProjectService
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    public ProjectService(IRepository<Project> projectRepository, IEventPublisher eventPublisher, IMapper mapper)
    {
        _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
    }

    // Project operations

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        try
        {
            var projects = await _projectRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while retrieving projects: {ex.Message}");
            throw; 
        }
    }


    public async Task<ProjectDto> GetProjectByIdAsync(string id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> CreateProjectAsync(ProjectCreateDto projectCreateDto)
    {
        var project = _mapper.Map<Project>(projectCreateDto);
        await _projectRepository.CreateAsync(project);
        var projectDto = _mapper.Map<ProjectDto>(project);

        await _eventPublisher.PublishAsync(projectDto);


        return projectDto;
    }

    public async Task<bool> UpdateProjectAsync(string id, ProjectUpdateDto projectUpdateDto)
    {
        var projectToUpdate = await _projectRepository.GetByIdAsync(id);
        if (projectToUpdate == null)
        {
            return false;
        }

        _mapper.Map(projectUpdateDto, projectToUpdate);
        await _projectRepository.UpdateAsync(id, projectToUpdate);
        var projectDto = _mapper.Map<ProjectDto>(projectToUpdate);

        await _eventPublisher.PublishAsync(projectDto);


        return true;
    }

    public async Task<bool> DeleteProjectAsync(string id)
    {
        if (await _projectRepository.DeleteAsync(id))
        {
            var projectDeleteDto = new ProjectDeleteDto() { Id = id };

            await _eventPublisher.PublishAsync(projectDeleteDto);


            return true;
        }

        return false;
    }

    // Comment operations
    public async Task<CommentDto> AddCommentToProjectAsync(string projectId, CommentCreateDto commentDto)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            return null; // Project not found
        }

        var comment = _mapper.Map<Comment>(commentDto);
        comment.CreatedAt = DateTime.UtcNow;
        project.Comments.Add(comment);

        await _projectRepository.UpdateAsync(projectId, project);
        var projectDto = _mapper.Map<ProjectDto>(project);

        await _eventPublisher.PublishAsync(projectDto);


        return _mapper.Map<CommentDto>(comment);
    }


    public async Task<bool> UpdateCommentAsync(string projectId, string commentId, CommentUpdateDto commentDto)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);

        if (project == null)
        {
            // Project not found
            return false;
        }

        var commentIndex = project.Comments.FindIndex(c => c.Id == commentId);
        if (commentIndex != -1)
        {
            var updatedComment = _mapper.Map<Comment>(commentDto);
            updatedComment.Id = commentId;

            project.Comments[commentIndex] = updatedComment;

            try
            {
                if (await _projectRepository.UpdateAsync(projectId, project))
                {
                    var projectDto = _mapper.Map<ProjectDto>(project);


                    await _eventPublisher.PublishAsync(projectDto);


                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    public async Task<bool> DeleteCommentAsync(string projectId, string commentId)
    {
        try
        {
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (project == null)
            {
                return false;
            }

            var commentToRemove = project.Comments.FirstOrDefault(c => c.Id == commentId);
            if (commentToRemove != null)
            {
                project.Comments.Remove(commentToRemove);
                if (await _projectRepository.UpdateAsync(project.Id, project))
                {
                    var projectDto = _mapper.Map<ProjectDto>(project);


                    await _eventPublisher.PublishAsync(projectDto);


                    return true;
                }

                return false;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return false;
        }
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsByProjectIdAsync(string projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            return null;
        }

        ;
        var comments = project.Comments;
        var commentsDto = _mapper.Map<IEnumerable<CommentDto>>(comments);
        return commentsDto;
    }

    // Participant operations

    public async Task<bool> AddParticipantToProjectAsync(string projectId, string participantId)
    {
        try
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {

                return false;
            }
            //Console.WriteLine("");
            if (!project.Participants.Contains(participantId))
            {
                Console.WriteLine(project.Id);

                project.Participants.Add(participantId);
                if (await _projectRepository.UpdateAsync(projectId, project))
                {
                    var projectDto = _mapper.Map<ProjectDto>(project);
                    
                    await _eventPublisher.PublishAsync(projectDto);
                    
                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return false;
        }
    }


    public async Task<bool> RemoveParticipantFromProjectAsync(string projectId, string participantId)
    {
        try
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                return false;
            }

            if (project.Participants.Contains(participantId))
            {
                project.Participants.Remove(participantId);
                if (await _projectRepository.UpdateAsync(projectId, project))
                {
                    var projectDto = _mapper.Map<ProjectDto>(project);

                    await _eventPublisher.PublishAsync(projectDto);

                    return true;
                }

                return false;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return false;
        }
    }

    public async Task<IEnumerable<string>> GetParticipantsOfProjectAsync(string projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        return project?.Participants ?? Enumerable.Empty<string>();
    }
}