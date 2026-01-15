using DMS.Auth.Feature.JobCard.Command;
using FluentValidation;

namespace DMS.Auth.Feature.JobCard.Validation
{
    public class CreateJobCardValidator : AbstractValidator<CreateJobCardCommand>
    {
        public CreateJobCardValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty()
                .MinimumLength(3);
        }
    }
}
