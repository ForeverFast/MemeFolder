using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemeFolder.Domain.Models
{
    [Table("Folders")]
    public class Folder : FolderObject
    {
        public string FolderPath { get; set; }
        public DateTime CreatingDate { get; set; }
        public virtual Folder ParentFolder { get; set; }

        public virtual ObservableCollection<Folder> Folders { get; set; }

        public virtual ObservableCollection<Meme> Memes { get; set; }

        [NotMapped]
        public int GetHC { get => this.GetHashCode(); }

        public override object Clone()
        {
            Folder newFolder = new Folder()
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description,
                ParentFolder = this.ParentFolder
            };
            return newFolder;
        }

        public Folder() : base()
        {
            //CreatingDate = DateTime.Now;
            Folders = new ObservableCollection<Folder>();
            Memes = new ObservableCollection<Meme>();
        }

      
    }
}
