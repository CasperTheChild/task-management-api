using Application.Services.Interfaces;
using Hangfire;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class HangfireJobService : IBackgroundJobService
{
    private readonly IBackgroundJobClient jobs;

    public HangfireJobService(IBackgroundJobClient jobs)
    {
        this.jobs = jobs;
    }

    public void Enqueue<T>(Expression<Func<T, Task>> job)
    {
        this.jobs.Enqueue(job);
    }

    public void Schedule<T>(Expression<Func<T, Task>> job, TimeSpan delay)
    {
        this.jobs.Schedule(job, delay);
    }
}
