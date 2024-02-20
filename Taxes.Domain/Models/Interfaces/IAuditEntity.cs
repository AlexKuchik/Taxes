using System;

namespace Taxes.Domain.Models.Interfaces
{
    public interface IAuditEntity
    {
        DateTime CreatedAt { get; set; }

        DateTime? ModifiedAt { get; set; }
    }
}