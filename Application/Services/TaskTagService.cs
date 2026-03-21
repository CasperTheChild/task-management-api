using Application.DTOs;
using Application.Helpers;
using Application.Repository.Interfaces;

namespace Application.Services;

public class TaskTagService
{
    private readonly ITaskTagRepository taskTagRepository;
    private readonly IUnitOfWork unitOfWork;

    public TaskTagService(ITaskTagRepository taskTagRepository, IUnitOfWork unitOfWork)
    {
        this.taskTagRepository = taskTagRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<PaginatedModel<TagModel>> GetPagedTagsByTask(int taskId, int pageNumber, int pageSize)
    {
        var list = await this.taskTagRepository.GetPagedTagsByTask(taskId, pageNumber, pageSize);

        return PaginationMapper.ToPaginatedModel(list.Items.Select(t => TagMapper.ToModel(t)), list.TotalItems, pageNumber, pageSize);
    }

    public async Task<PaginatedModel<TaskModel>> GetPagedTasksByTag(int tagId, int pageNumber, int pageSize)
    {
        var list = await this.taskTagRepository.GetPagedTasksByTag(tagId, pageNumber, pageSize);

        return PaginationMapper.ToPaginatedModel(list.Items.Select(t => TaskMapper.ToModel(t)), list.TotalItems, pageNumber, pageSize);
    }

    public async Task AssignTag(int taskId, int tagId)
    {
        this.taskTagRepository.AssignTag(taskId, tagId);

        await this.unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveTag(int taskId, int tagId)
    {
        await this.taskTagRepository.RemoveTag(taskId, tagId);

        await this.unitOfWork.SaveChangesAsync();
    }
}
