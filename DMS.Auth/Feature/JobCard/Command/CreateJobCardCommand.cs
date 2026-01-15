using MediatR;

namespace DMS.Auth.Feature.JobCard.Command
{
    public record CreateJobCardCommand(string CustomerName) : IRequest<int>;
   
}
