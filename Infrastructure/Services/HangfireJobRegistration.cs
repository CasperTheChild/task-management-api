using Application.Services.Interfaces;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class HangfireJobRegistration
{
    private readonly ICurrentUserService currentUserService;

    public HangfireJobRegistration(ICurrentUserService currentUserService)
    {
        this.currentUserService = currentUserService;
    }

    public static void RegisterJobs()
    {
        RecurringJob.AddOrUpdate<IDeadlineService>(
            "deadline-check",
            job => job.CheckUpcomingDeadlinesAsync(),
            Cron.Minutely);
    }
}
