using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MemeFolder.Abstractions
{
    public interface IFolderVM
    {
        Folder Model { get; set; }
        ObservableCollection<FolderObject> FolderObjects { get; set; }
    }
}
