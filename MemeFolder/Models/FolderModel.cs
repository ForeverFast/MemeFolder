using MemeFolder.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.Models
{
    public class FolderModel
    {
        private Folder _folder;


        public FolderModel(Folder folder)
        {
            _folder = folder;
        }
    }
}
