using System;
using System.ComponentModel.DataAnnotations;

namespace MemeFolder.Domain.Models
{
    public abstract class DomainObject
    {
        [Key]
        public Guid Id { get; set; }
    }
}
