using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Taxes.Domain.Enums;
using Taxes.Domain.Models.Base;
using Taxes.Domain.Models.Interfaces;

namespace Taxes.Domain.Models
{
    [Index(nameof(City), Name = "Index_City")]
    public record Tax : BaseEntity, IAuditEntity
    {
        [Required]
        public string City { get; init; } = default!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public TaxCategory Category { get; init; }

        [Required]
        public decimal Rate { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}