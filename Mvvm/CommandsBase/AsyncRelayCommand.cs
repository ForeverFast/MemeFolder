using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.CommandsBase
{
    public class AsyncRelayCommand : AsyncCommandBase
    {
        private readonly Func<object, Task> _callback;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"> функция вызова </param>
        /// <param name="onException"></param>
        public AsyncRelayCommand(Func<object, Task> callback, Action<Exception> onException = null) : base(onException)
        {
            _callback = callback;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await _callback(parameter);
        }
    }
}
