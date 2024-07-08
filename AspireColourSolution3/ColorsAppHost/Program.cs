using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var colorsapi = builder.AddProject<ColorsAPI>("colorsapi");

var colorsweb = builder.AddProject<ColorsWeb>("colorsweb")
    .WithReference(colorsapi)
    .WithExternalHttpEndpoints();

builder.Build().Run();
