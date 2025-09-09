using Election.API.Data;
using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IElectionRepository, FirestoreElectionRepository>();
builder.Services.AddSwaggerGen();
// Choose repo implementation
var useInMemory = (Environment.GetEnvironmentVariable("USE_INMEMORY") ?? "false")
                  .Equals("true", StringComparison.OrdinalIgnoreCase);

if (useInMemory)
{
    builder.Services.AddSingleton<IElectionRepository, InMemoryElectionRepository>();
}
else
{
    // Firestore DI (only if not using in-memory)
    string projectId =
        builder.Configuration["GoogleCloud:ProjectId"] ??
        Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT") ??
        Environment.GetEnvironmentVariable("GCP_PROJECT") ??
        throw new InvalidOperationException("Set GoogleCloud:ProjectId or GOOGLE_CLOUD_PROJECT.");

    builder.Services.AddSingleton(FirestoreDb.Create(projectId));
    builder.Services.AddScoped<IElectionRepository, FirestoreElectionRepository>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment() || true)
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
