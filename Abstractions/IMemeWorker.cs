using MemeFolder.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MemeFolder.Abstractions
{
    public interface IMemeWorker
    {
        ICommand OpenMemePictureCommand { get; }
        ICommand CopyMemeInBufferCommand { get; }
        ICommand AddMemeCommand { get; }
        ICommand RemoveMemeCommand { get; }
    }
}
