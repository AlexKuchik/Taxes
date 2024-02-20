using System;

namespace Taxes.Domain.Models.Base
{
    public abstract record BaseEntity
    {
        public Guid Id { get; init; }
    }
}