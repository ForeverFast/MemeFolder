using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Extentions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MemeFolder.ViewModels
{
    public partial class FolderVM
    {
        public ICommand PageLoadedCommand { get; }

        private void PageLoadedExecuteAsync(object parameter)
        {
            if (Children == null || FolderObjects == null)
            {
                IsBusy = true;

                Children = new ObservableCollection<FolderVM>();
                FolderObjects = new ObservableCollection<FolderObject>();

                BackgroundWorker bgW = new BackgroundWorker();
                bgW.DoWork += BgW_DoWork;
                bgW.RunWorkerCompleted += BgW_RunWorkerCompleted;
                bgW.RunWorkerCompleted += (o, e) => bgW.Dispose();
                bgW.RunWorkerAsync(Model);
            }
        }

        private void BgW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnPropertyChanged(nameof(FolderObjects));
            foreach (var item in (List<FolderObject>)e.Result)
                FolderObjects.Add(item);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            IsLoaded = true;
            IsBusy = false;
        }

        private void BgW_DoWork(object sender, DoWorkEventArgs e)
        {
            var model = (Folder)e.Argument;

            List<FolderObject> folderObjects = new List<FolderObject>();

            foreach (var item in model.Folders)
                folderObjects.Add(item);

            foreach (var item in model.Memes)
            {
                item.Image = MemeExtentions.ConvertByteArrayToImage(item.ImageData);
                item.ImageData = null;
                if (item.Image != null)
                    item.Image.Freeze();
                folderObjects.Add(item);
            }

            e.Result = folderObjects;
        }
    }
}


/*
 * 
 *  Debug.WriteLine($"------------------------------------------------------------------------------------");
            Debug.WriteLine($"ViewModel: {Model.Title}");
            Debug.WriteLine($"Страница загрузилась. Начало метода PageLoadedExecuteAsync Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
 * 
 * App.Current.Dispatcher.BeginInvoke(() => {
                    folderObjects.Add(item);
                    Children.Add(new FolderVM(item, _dataService));
                });
 App.Current.Dispatcher.BeginInvoke(() => FolderObjects.Add(item));
 * 
  sw.Stop();
            Debug.WriteLine($"Фоновый поток завершён. Время: {sw.ElapsedMilliseconds}ms. \r\n Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            sw.Restart();
            //var data = (SearchData)e.Result;

            Debug.WriteLine($"Элементы загружены в FolderObject. Время: {sw.ElapsedMilliseconds}ms. \r\n Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            sw.Restart();

            //OnPropertyChanged(nameof(Children));
            //OnPropertyChanged(nameof(FolderObjects));


            sw.Stop();
            Debug.WriteLine($"{GC.GetTotalMemory(false) / 1024} kb");
            Debug.WriteLine($"UI прогрузила пикчи. Время: {sw.ElapsedMilliseconds}ms. \r\n Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Debug.WriteLine($"------------------------------------------------------------------------------------");
//Debug.WriteLine($"{GC.GetTotalMemory(false) / 1024} kb");


  Debug.WriteLine($"Начало работы фонового потока. \r\n Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Debug.WriteLine($"Начало работы фонового потока. \r\n Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
 Debug.WriteLine($"Страница загрузилась. Конец метода PageLoadedExecuteAsync Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
 */