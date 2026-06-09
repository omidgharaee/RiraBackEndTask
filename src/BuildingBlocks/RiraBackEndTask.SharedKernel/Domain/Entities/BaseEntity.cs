using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.SharedKernel.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;

    public DateTime? ModifiedAtUtc { get; protected set; }

    protected void SetModified()
    {
        ModifiedAtUtc = DateTime.UtcNow;
    }
}