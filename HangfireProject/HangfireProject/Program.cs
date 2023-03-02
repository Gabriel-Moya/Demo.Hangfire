using Hangfire;
using Hangfire.Storage.SQLite;
using HangfireProject.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ========== Hangfire ==========
builder.Services.AddHangfire(configuration => configuration
                                        .UseRecommendedSerializerSettings()
                                        .UseSQLiteStorage());

GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
{
    Attempts = 3,
    DelaysInSeconds = new int[] { 300 }
});

builder.Services.AddHangfireServer();
// ========== Hangfire ==========

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ========== Hangfire Dashboard ==========
app.UseHangfireDashboard();
// ========== Hangfire Dashboard ==========

RegisterJobs();

app.Run();

static void RegisterJobs()
{
    BackgroundJob.Enqueue<JobsHangfire>(x => x.FireAndForget());
    BackgroundJob.Schedule<JobsHangfire>(x => x.Delayed(), TimeSpan.FromMinutes(1));
    BackgroundJob.Enqueue<JobsHangfire>(x => x.TimeConsumingMethod());

    RecurringJob.AddOrUpdate<JobsHangfire>(
        "Job recorrente",
        job => job.RecurringJobMethod(),
        Cron.Minutely,
        TimeZoneInfo.Local);
}
