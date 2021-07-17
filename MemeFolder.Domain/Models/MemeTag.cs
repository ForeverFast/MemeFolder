using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemeFolder.Domain.Models
{
    [Table("MemeTags")]
    public class MemeTag : DomainObject, ICloneable
    {
        public string Title { get; set; }
        [NotMapped]
        public bool CheckFlag { get; set; }

        public override string ToString() => this.Title;
        public object Clone()
        {
            return new MemeTag() { Id = this.Id, Title = this.Title };
        }
    }
}
