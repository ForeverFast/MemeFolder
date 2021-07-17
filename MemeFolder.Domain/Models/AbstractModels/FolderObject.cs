using System;

namespace MemeFolder.Domain.Models
{
    public abstract class FolderObject : DomainObject, ICloneable
    {
        public uint Position { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public override string ToString() => this.Title;
        public abstract object Clone();

        public FolderObject()
        { }
    }
}
