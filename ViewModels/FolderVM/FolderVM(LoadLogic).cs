using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Extentions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace MemeFolder.ViewModels
{
    public partial class FolderVM
    {
        public ICommand PageLoadedCommand { get; }

        private async Task PageLoadedExecuteAsync(object parameter)
        {

            Debug.WriteLine($"------------------------------------------------------------------------------------");
            Debug.WriteLine($"ViewModel: {Model.Title}");
            Debug.WriteLine($"Страница загрузилась. Начало метода PageLoadedExecuteAsync Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");

            if (Children == null || FolderObjects == null)
            {
                IsBusy = true;


                Children = new ObservableCollection<FolderVM>();
                FolderObjects = new ObservableCollection<FolderObject>();
                Memes = new ObservableCollection<Meme>();
                FolderObjects.CollectionChanged += FolderObjects_CollectionChanged;

                //FolderObjects.CollectionChanged += (o, e) => {
                //    Debug.WriteLine($"Новый элемент в FolderObjects. Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                //};

               

                BackgroundWorker bgW = new BackgroundWorker();
                bgW.DoWork += BgW_DoWork;
                bgW.RunWorkerCompleted += BgW_RunWorkerCompleted;
                bgW.ProgressChanged += BgW_ProgressChanged;
                bgW.RunWorkerAsync(Model);


               
            }
        }

        private void BgW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void BgW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //PMemes = new PagingCollectionView(FolderObjects, 60);
            OnPropertyChanged(nameof(FolderObjects));

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Debug.WriteLine($"{GC.GetTotalMemory(false) / 1024} kb");

            IsLoaded = true;
            IsBusy = false;
        }

        private void BgW_DoWork(object sender, DoWorkEventArgs e)
        {

            var model = (Folder)e.Argument;

            foreach (var item in model.Folders)
            {
                //item.PropertyChanged += Model_PropertyChanged;
                App.Current.Dispatcher.BeginInvoke(() => {
                    FolderObjects.Add(item);
                    Children.Add(new FolderVM(item, _dataService));
                });

                OnPropertyChanged(nameof(FolderObjects));
            }

            foreach (var item in model.Memes)
            {
                //item.PropertyChanged += Model_PropertyChanged;
                item.Image = MemeExtentions.ConvertByteArrayToImage(item.ImageData);
                item.ImageData = null;
                item.Image.Freeze();

                App.Current.Dispatcher.BeginInvoke(() => FolderObjects.Add(item));
                OnPropertyChanged(nameof(FolderObjects));
            }
          
        }
    }
}


/*
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


  Debug.WriteLine($"Начало работы фонового потока. \r\n Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Debug.WriteLine($"Начало работы фонового потока. \r\n Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
 Debug.WriteLine($"Страница загрузилась. Конец метода PageLoadedExecuteAsync Поток: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
 */