using System.Linq.Expressions;

namespace Application.Services.Interfaces;

public interface IBackgroundJobService
{
    void Enqueue<T>(Expression<Func<T, Task>> job);

    void Schedule<T>(Expression<Func<T, Task>> job, TimeSpan delay);
}
