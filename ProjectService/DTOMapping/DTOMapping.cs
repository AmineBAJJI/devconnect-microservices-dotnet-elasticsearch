using AutoMapper;
using Shared.Events.Project;
using ProjectService.Models;

namespace ProjectService.DTOMapping;

public class DTOMapping : Profile
{
    public DTOMapping()
    {
        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectDto, Project>();
        CreateMap<ProjectCreateDto, Project>();
        CreateMap<Project, ProjectCreateDto>();
        CreateMap<ProjectUpdateDto, Project>();
        CreateMap<Project, ProjectUpdateDto>();
        CreateMap<ProjectDeleteDto, Project>();
        CreateMap<Project, ProjectDeleteDto>();
        CreateMap<Comment, CommentCreateDto>();
        CreateMap<CommentCreateDto, Comment>();
        CreateMap<CommentUpdateDto, Comment>();
        CreateMap<Comment, CommentUpdateDto>();
        CreateMap<Comment, CommentDto>();
        CreateMap<CommentDto, Comment>();

    }
}