using MemeFolder.Domain.Models.AbstractModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemeFolder.Domain.Models
{
    [Table("Folders")]
    public class Folder : FolderObject
    {
        #region Поля
        private string _imageFolderPath;
        private DateTime _creatingDate;
        private Folder _parentFolder;
        private ObservableCollection<Folder> _folders;
        private ObservableCollection<Meme> _memes;
        #endregion

        public string FolderPath { get => _imageFolderPath; set => SetProperty(ref _imageFolderPath, value); }
        public DateTime CreatingDate { get => _creatingDate; set => SetProperty(ref _creatingDate, value); }
        public virtual Folder ParentFolder { get => _parentFolder; set => SetProperty(ref _parentFolder, value); }

        public virtual ObservableCollection<Folder> Folders { get => _folders; set => SetProperty(ref _folders, value); }
      
        public virtual ObservableCollection<Meme> Memes { get => _memes; set => SetProperty(ref _memes, value); }

        [NotMapped]
        public int GetHC { get => this.GetHashCode(); }

        public Folder() : base()
        {
            CreatingDate = DateTime.Now;
            Folders = new ObservableCollection<Folder>();
            Memes = new ObservableCollection<Meme>();
        }
    }
}
