using Shared.Events.Project;

namespace ProjectService.Services;

public interface IProjectService
{
    // Project operations
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto> GetProjectByIdAsync(string id);
    Task<ProjectDto> CreateProjectAsync(ProjectCreateDto projectCreateDto);
    Task<bool> UpdateProjectAsync(string id, ProjectUpdateDto projectUpdateDto);
    Task<bool> DeleteProjectAsync(string id);

    // Comment operations
    Task<CommentDto> AddCommentToProjectAsync(string projectId, CommentCreateDto commentDto);
    //Task<CommentDto> GetCommentByIdAsync(string commentId);
    Task<IEnumerable<CommentDto>> GetCommentsByProjectIdAsync(string projectId);
    Task<bool> UpdateCommentAsync(string projectId, string commentId, CommentUpdateDto commentDto);
    Task<bool> DeleteCommentAsync(string projectId, string commentId);

    // Participant operations
    Task<bool> AddParticipantToProjectAsync(string projectId, string participantId);
    Task<bool> RemoveParticipantFromProjectAsync(string projectId, string participantId);
    Task<IEnumerable<string>> GetParticipantsOfProjectAsync(string projectId);
}