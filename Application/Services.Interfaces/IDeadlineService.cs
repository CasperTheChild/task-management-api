using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Application.Services.Interfaces;

public interface IDeadlineService
{
    public Task CheckUpcomingDeadlinesAsync();
}
