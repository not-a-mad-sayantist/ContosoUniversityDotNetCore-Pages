using ContosoUniversity.Data;
using ContosoUniversity.Infrastructure.Tags;
using FluentValidation;
using FluentValidation.AspNetCore;
using HtmlTags;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoUniversity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddContosoServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddMiniProfiler().AddEntityFramework();

        services.AddDbContext<SchoolContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("db"))
        );

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

        return services;
    }
}
