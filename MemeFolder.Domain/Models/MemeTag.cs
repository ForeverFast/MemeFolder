using MemeFolder.Domain.Models.AbstractModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemeFolder.Domain.Models
{
    [Table("MemeTags")]
    public class MemeTag : DomainObject
    {
        #region Поля
        private string _title;
        private bool _checkFlag;
        #endregion

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        [NotMapped]
        public bool CheckFlag { get => _checkFlag; set => SetProperty(ref _checkFlag, value); }
    }
}
