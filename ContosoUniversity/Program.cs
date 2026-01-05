using ContosoUniversity.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddContosoServices(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.ConfigurePipeline();

app.Run();
