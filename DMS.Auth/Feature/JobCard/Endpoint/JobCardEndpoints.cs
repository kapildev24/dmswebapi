using DMS.Auth.Feature.JobCard.Command;
using MediatR;

namespace DMS.Auth.Feature.JobCard.Endpoint
{
    public static  class JobCardEndpoints
    {
        public static void MapJobCardEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/jobcards",
                async (CreateJobCardCommand cmd, IMediator mediator) =>
                {
                    var id = await mediator.Send(cmd);
                    return Results.Created($"/jobcards/{id}", id);
                });

            //app.MapGet("/jobcards/{id:guid}",
            //    async (Guid id, IMediator mediator) =>
            //    {
            //        var result = await mediator.Send(
            //            new GetJobCardByIdQuery(id));
            //        return result is null ? Results.NotFound() : Results.Ok(result);
            //    });
        }
    }
}
