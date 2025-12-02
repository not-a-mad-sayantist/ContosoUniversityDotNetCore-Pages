using ContosoUniversity.Data;
using ContosoUniversity.Infrastructure;
using ContosoUniversity.Infrastructure.Tags;
using FluentValidation;
using FluentValidation.AspNetCore;
using HtmlTags;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

RegisterServices(builder);

var app = builder.Build();

app.MapDefaultEndpoints();

ConfigureApplication(app);

app.Run();

static void RegisterServices(WebApplicationBuilder builder)
{
    var services = builder.Services;

    services.AddMiniProfiler().AddEntityFramework();

    services.AddDbContext<SchoolContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("contosouniversity-db")));

    services.AddAutoMapper(_ => { }, typeof(Program));

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

    services.AddHtmlTags(new TagConventions());

    services.AddRazorPages(opt =>
        {
            opt.Conventions.ConfigureFilter(new DbContextTransactionPageFilter());
            opt.Conventions.ConfigureFilter(new ValidatorPageFilter());
        });

    services.AddFluentValidationAutoValidation();
    services.AddFluentValidationClientsideAdapters();
    services.AddValidatorsFromAssemblyContaining<Program>();

    services.AddMvc(opt => opt.ModelBinderProviders.Insert(0, new EntityModelBinderProvider()));
}

static void ConfigureApplication(WebApplication app)
{
    app.UseMiniProfiler();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseAuthorization();

    app.MapRazorPages();
}