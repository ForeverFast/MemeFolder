using MemeFolder.Domain.Models;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MemeFolder.Mvvm.Commands
{
    /// <summary> Команда открытия Meme </summary>
    public class OpenMemePictureCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
      
        public bool CanExecute(object parameter) => !string.IsNullOrEmpty((parameter as Meme).ImagePath);

        public void Execute(object parameter)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo((parameter as Meme).ImagePath)
            {
                UseShellExecute = true
            };
            p.Start();
            p.Dispose();

            //WaveStream waveStream = new WaveFileReader(Properties.Resources.bababooey_sound_effect);
            //var waveOut = new WaveOut();
            //waveOut.Init(waveStream);
            //waveOut.Volume = 0.1f;
            //waveOut.Play();

            CanExecuteChanged?.Invoke(this, null);
        }
    }
}
