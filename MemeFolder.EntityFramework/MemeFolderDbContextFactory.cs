using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;

namespace MemeFolder.EntityFramework
{
    public class MemeFolderDbContextFactory : IDesignTimeDbContextFactory<MemeFolderDbContext>
    {
        public MemeFolderDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<MemeFolderDbContext>();
            options.EnableSensitiveDataLogging(true);
            options.UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MemeFolderDB;Integrated Security=True;" +
           "Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
//#if DEBUG
           
//#else
//            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
//            if (!Directory.Exists($@"{appData}\MemeFolder"))
//                Directory.CreateDirectory($@"{appData}\MemeFolder");
//            options.UseSqlite($@"Data Source={appData}\MemeFolder\MF_DB.db; Cache=Shared;");
//#endif

            return new MemeFolderDbContext(options.Options);
        }
    }
}
//options.UseSqlServer(@$"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={AppDomain.CurrentDomain.BaseDirectory}\MF_DB.mdf;Integrated Security=True");
//options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=MemeFolderDB; Trusted_Connection=True;");
