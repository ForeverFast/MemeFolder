using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace MemeFolder.Domain.Models
{
    [Table("Memes")]
    public class Meme : FolderObject
    {
        public DateTime AddingDate { get; set; }

        public Folder Folder { get; set; }


        public string ImagePath { get; set; }

        public string MiniImagePath { get; set; }

        public virtual ObservableCollection<MemeTagNode> Tags { get; set; }


        [NotMapped]
        public int GetHC { get => this.GetHashCode(); }
        public override object Clone()
        {
            Meme newMeme = new Meme()
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description,
                Folder = this.Folder,
                ImagePath = this.ImagePath,
                MiniImagePath = this.MiniImagePath
            };
            return newMeme;
        }

        public Meme() :  base()
        {
            AddingDate = DateTime.Now;
            Tags = new ObservableCollection<MemeTagNode>();
        }
    }
}