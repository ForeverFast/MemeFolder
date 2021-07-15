using MemeFolder.Domain.Models.AbstractModels;
using System.ComponentModel.DataAnnotations;

namespace MemeFolder.Domain.Models
{
    public class MemeTagNode : DomainObject
    {
        [Required]
        public MemeTag MemeTag { get; set; }
        [Required]
        public Meme Meme { get; set; }

        public override string ToString() => MemeTag.Title;
    }
}
