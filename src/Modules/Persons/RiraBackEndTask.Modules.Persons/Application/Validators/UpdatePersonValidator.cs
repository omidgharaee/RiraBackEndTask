using FluentValidation;
using RiraBackEndTask.Modules.Persons.Application.Commands.UpdatePerson;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.Modules.Persons.Application.Validators;

public sealed class UpdatePersonValidator
    : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);

        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);

        RuleFor(x => x.NationalCode)
            .NotEmpty()
            .Matches("^[0-9]{10}$")
            .WithMessage("کد ملی باید دقیقاً ۱۰ رقم عددی باشد.");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .Must(x => DateOnly.TryParse(x, out _))
            .WithMessage("تاریخ تولد معتبر نیست.");
    }
}