using MemeFolder.Domain.Models.AbstractModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace MemeFolder.Domain.Models
{
    [Table("Memes")]
    public class Meme : FolderObject
    {
        #region Поля
       
        private DateTime _addingDate;
        private Folder _folder;

        private string _imagePath;
        private byte[] _imageData;
        private ImageSource _image;

        public ObservableCollection<MemeTagNode> _tags;
        #endregion

        public DateTime AddingDate { get => _addingDate; set => SetProperty(ref _addingDate, value); }

        public Folder Folder { get => _folder; set => SetProperty(ref _folder, value); }


        public string ImagePath { get => _imagePath; set => SetProperty(ref _imagePath, value); }

        public byte[] ImageData { get => _imageData; set => SetProperty(ref _imageData, value); }

        [NotMapped]
        public ImageSource Image { get => _image; set => SetProperty(ref _image, value); }


        public virtual ObservableCollection<MemeTagNode> Tags { get => _tags; set => SetProperty(ref _tags, value); }


        [NotMapped]
        public int GetHC { get => this.GetHashCode(); }
        public override string ToString() => this.Title;

        public Meme() :  base()
        {
            AddingDate = DateTime.Now;
            Tags = new ObservableCollection<MemeTagNode>();
        }
    }
}