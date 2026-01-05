using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Courses;

public class Index(IMediator mediator) : PageModel
{
    public Result Data { get; private set; }

    public async Task OnGetAsync() => Data = await mediator.Send(new Query());

    public record Query : IRequest<Result> { }

    public record Result
    {
        public List<Course> Courses { get; init; }

        public record Course
        {
            public int Id { get; init; }
            public string Title { get; init; }
            public int Credits { get; init; }
            public string DepartmentName { get; init; }
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile() => CreateProjection<Course, Result.Course>();
    }

    public class QueryHandler(SchoolContext db, IConfigurationProvider configuration)
        : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query message, CancellationToken token)
        {
            var courses = await db
                .Courses.OrderBy(d => d.Id)
                .ProjectToListAsync<Result.Course>(configuration);

            return new Result { Courses = courses };
        }
    }
}
