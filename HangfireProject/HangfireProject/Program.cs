using Hangfire;
using Hangfire.Storage.SQLite;

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

app.Run();
