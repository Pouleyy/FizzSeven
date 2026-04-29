var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.FizzSeven_Api>("api")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");

builder.Build().Run();
