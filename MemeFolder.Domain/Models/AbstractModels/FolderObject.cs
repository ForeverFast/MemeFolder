using System.Collections.ObjectModel;

namespace MemeFolder.Domain.Models.AbstractModels
{
    public abstract class FolderObject : DomainObject
    {
        #region Поля
        public uint _position;
        public string _title;
        public string _description;    
        #endregion

        public uint Position { get => _position; set => SetProperty(ref _position, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
      
        public FolderObject()
        { }
    }
}
