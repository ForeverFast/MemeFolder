using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemeFolder.Services
{
    public class MemeDataService : GenericDataService<Meme>, IMemeDataService
    {
    }
}
