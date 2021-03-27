using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework;
using MemeFolder.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public class MemeDataService : GenericDataService<Meme>, IMemeDataService
    {
       
        public override async Task<Meme> Create(Meme meme)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    var check = await context.Folders.Where(x => x.Id == meme.Folder.Id).FirstOrDefaultAsync();
                    if (check != null)
                        meme.Folder = check;

                    EntityEntry<Meme> createdResult = await context.Set<Meme>().AddAsync(meme);
                    await context.SaveChangesAsync();

                    return createdResult.Entity;
                }
                catch(Exception ex)
                {
                    return null;
                }


               
            }
        }
    }
}
