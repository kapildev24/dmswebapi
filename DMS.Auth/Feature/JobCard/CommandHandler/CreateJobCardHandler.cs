using DMS.Auth.Feature.JobCard.Command;
using MediatR;



namespace DMS.Auth.Feature.JobCard.CommandHandler
{
    public class CreateJobCardHandler : IRequestHandler<CreateJobCardCommand, int>
    {
        private readonly AuthDbContext _authDb;

        public CreateJobCardHandler(AuthDbContext authDb)
        {
            _authDb = authDb;
        }

        public async Task<int> Handle(CreateJobCardCommand request, CancellationToken cancellationToken)
        {
            var jobCard = new BuildingBlock.Common.EntityModels.JobCardEntity.JobCard
            {
                
                CustomerName = request.CustomerName,
                Status = "OPEN",
                CreatedAt = DateTime.UtcNow
            };

            _authDb.JobCards.Add(jobCard);
            await _authDb.SaveChangesAsync(cancellationToken);

            return jobCard.Id;
        }
    }
}
