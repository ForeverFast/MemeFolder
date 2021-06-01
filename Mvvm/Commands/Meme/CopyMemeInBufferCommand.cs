using MemeFolder.Domain.Models;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MemeFolder.Mvvm.Commands.Memes
{
    public class CopyMemeInBufferCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => !string.IsNullOrEmpty((parameter as Meme).ImagePath);
    
        public void Execute(object parameter) => Clipboard.SetImage(new BitmapImage(new Uri((parameter as Meme).ImagePath)));
    }
}
