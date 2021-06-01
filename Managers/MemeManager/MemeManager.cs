using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.Managers.MemeManager
{
    public class MemeManager : IMemeManager
    {
        private readonly IMemeDataService _memeDataService;


        #region Meme ивенты
        public delegate void ChangedMemeHandler(object sender, ActionChanged action, Meme meme);
        public event ChangedMemeHandler ChangedMemeEvent;
        /// <summary>Вспомогательный метод вызова события после удаления</summary>
        /// <param name="dormitories">Удалённое общежитие</param>
        protected void OnRemovememeEvent(Meme meme) => ChangedMemeEvent?.Invoke(this, ActionChanged.Remove, meme);
        /// <summary>Вспомогательный метод вызова события после добавления</summary>
        /// <param name="dormitories">Добавленное общежитие</param>
        protected void OnAddMemeEvent(Meme meme) => ChangedMemeEvent?.Invoke(this, ActionChanged.Add, meme);
        /// <summary>Вспомогательный метод вызова события после изменения</summary>
        /// <param name="dormitories">Изменённое общежитие</param>
        protected void OnChangedMemeEvent(Meme meme) => ChangedMemeEvent?.Invoke(this, ActionChanged.Changed, meme);
        #endregion

        #region Работа с данными

        public async Task<Meme> Get(Guid guid) 
        {
            return await _memeDataService.Get(guid);
        }

        public async Task Create(Meme entity)   
        {
            var createdEntity = await _memeDataService.Create(entity, entity.Id);
            OnAddMemeEvent(createdEntity);
        }

        public async Task Create(Meme entity, Guid containerId)
        {
            var createdEntity = await _memeDataService.Create(entity, containerId);
            OnAddMemeEvent(createdEntity);
        }

        public async Task Delete(Guid guid)
        {
            await _memeDataService.Delete(guid);
           
        }

        public async Task Update(Guid guid, Meme entity)
        {
            var updatedEntity = await _memeDataService.Update(guid, entity);
            OnChangedMemeEvent(updatedEntity);
        }

        

        public async Task<IEnumerable<Meme>> GetAll()
        {
            throw new NotImplementedException();
        }

        #endregion



        #region Конструкторы

        public MemeManager()
        {
            
        }

        #endregion
    }
}
