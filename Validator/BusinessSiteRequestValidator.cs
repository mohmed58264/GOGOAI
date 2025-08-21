using FluentValidation;
using FixoraBackend.DTOs.Requests;

namespace FixoraBackend.Validators
{
    public class BusinessSiteRequestValidator : AbstractValidator<BusinessSiteRequestDto>
    {
        public BusinessSiteRequestValidator()
        {
            RuleFor(x => x.BusinessClientId)
                .NotEmpty().WithMessage("BusinessClientId is required");

            RuleFor(x => x.SiteName)
                .NotEmpty().WithMessage("SiteName is required")
                .MaximumLength(100);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(200);

            RuleFor(x => x.ContactPhone)
                .NotEmpty().WithMessage("ContactPhone is required")
                .MaximumLength(20);
        }
    }
}