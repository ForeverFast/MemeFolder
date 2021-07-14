using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.Services.StatusMessagesService
{
    public delegate void StatusMessageDelegate(string message);

    public interface IStatusMessagesService
    {
        event StatusMessageDelegate NewMessage;
    }
}
