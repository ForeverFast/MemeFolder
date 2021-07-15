using System;
using System.ComponentModel.DataAnnotations;

namespace MemeFolder.Domain.Models.AbstractModels
{
    public abstract class DomainObject : OnPropertyChangedClass
    {
        [Key]
        public Guid Id { get; set; }
    }
}
