using FluentValidation;

namespace BookStoreApi.Slices.Reviews.UpdateReview
{
    public class UpdateReviewValidator : AbstractValidator<UpdateReviewRequest>
    {
        public UpdateReviewValidator()
        {
            RuleFor(x => x.Content).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Rating).InclusiveBetween(1, 5);
        }
    }
}
