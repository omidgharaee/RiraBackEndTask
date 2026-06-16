using RiraBackEndTask.SharedKernel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.Modules.Persons.Domain.Entities;

public sealed class Person : BaseEntity
{
    private Person()
    {
    }

    private Person(
        string firstName,
        string lastName,
        string nationalCode,
        DateOnly birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        NationalCode = nationalCode;
        BirthDate = birthDate;
    }

    public string FirstName { get; private set; } = null!;

    public string LastName { get; private set; } = null!;

    public string NationalCode { get; private set; } = null!;

    public DateOnly BirthDate { get; private set; }

    public static Person Create(
        string firstName,
        string lastName,
        string nationalCode,
        DateOnly birthDate)
    {
        return new Person(firstName, lastName, nationalCode, birthDate);
    }

    public void Update(
        string firstName,
        string lastName,
        string nationalCode,
        DateOnly birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        NationalCode = nationalCode;
        BirthDate = birthDate;

        SetModified();
    }
}