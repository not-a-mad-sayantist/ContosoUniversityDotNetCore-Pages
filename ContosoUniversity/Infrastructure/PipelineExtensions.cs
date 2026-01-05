using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace ContosoUniversity.Infrastructure;

public static class PipelineExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
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

        return app;
    }
}
