var builder = DistributedApplication.CreateBuilder(args);

var useInMemoryStatisticsStore = builder.Configuration.GetValue<bool>("Statistics:UseInMemoryStore");

var api = builder.AddProject<Projects.FizzSeven_Api>("api")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithEnvironment("Statistics__UseInMemoryStore", useInMemoryStatisticsStore.ToString().ToLowerInvariant());

if (!useInMemoryStatisticsStore)
{
    var cache = builder.AddRedis("cache")
        .WithDataVolume();

    api.WithReference(cache)
        .WaitFor(cache);
}

builder.Build().Run();
