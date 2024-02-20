using System;

namespace Takes.Domain.Models.Base
{
    public abstract record BaseEntity
    {
        public Guid Id { get; init; }
    }
}