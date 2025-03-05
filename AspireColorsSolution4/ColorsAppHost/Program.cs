using Projects;

// var appinsights = "InstrumentationKey=";

var builder = DistributedApplication.CreateBuilder(args);

var outputcache = builder.AddRedis("outputcache");

var colorsapi = builder.AddProject<ColorsAPI>("colorsapi")
//    .WithEnvironment("APPLICATIONINSIGHTS_CONNECTION_STRING", appinsights)
    .WithReference(outputcache);

var colorsweb = builder.AddProject<ColorsWeb>("colorsweb")
//    .WithEnvironment("APPLICATIONINSIGHTS_CONNECTION_STRING", appinsights)
    .WithReference(colorsapi)
    .WithExternalHttpEndpoints();

builder.Build().Run();
