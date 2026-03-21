using Application.DTOs;
using Application.Repository.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class TagService
{
    private readonly ITagRepository tagRepository;
    private readonly IUnitOfWork unitOfWork;

    public TagService(ITagRepository tagRepository, IUnitOfWork unitOfWork)
    {
        this.tagRepository = tagRepository;
        this.unitOfWork = unitOfWork;
    }

    public Task<IEnumerable<TagEntity>> GetAllTags()
    {
        return this.tagRepository.GetAllTags();
    }

    public Task<PaginatedModel<TagEntity>> GetTodoListTags(int todoListId, int pageNumber, int pageSize)
    {
        return this.tagRepository.GetTodoListTags(todoListId, pageNumber, pageSize);
    }

    public Task<TagEntity?> GetTagById(int tagId)
    {
        return this.tagRepository.GetTagById(tagId);
    }

    public Task<TagEntity?> GetTagByName(string tagName)
    {
        return this.tagRepository.GetTagByName(tagName);
    }

    public TagEntity CreateTag(int todoListId, TagCreateModel model)
    {
        var entity = this.tagRepository.CreateTag(todoListId, model);

        this.unitOfWork.SaveChangesAsync();

        return entity;
    }

    public Task UpdateTag(int tagId, TagCreateModel model)
    {
        this.tagRepository.UpdateTag(tagId, model);

        return this.unitOfWork.SaveChangesAsync();
    }

    public Task DeleteTag(int tagId)
    {
        this.tagRepository.DeleteTag(tagId);

        return this.unitOfWork.SaveChangesAsync();
    }
}
