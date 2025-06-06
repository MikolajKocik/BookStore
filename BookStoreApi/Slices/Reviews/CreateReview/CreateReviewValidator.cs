using FluentValidation;

namespace BookStoreApi.Slices.Reviews.CreateReview
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewRequest>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(500);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
        }
    }
}
