using System;
using System.Data;
using System.Data.SqlClient;

namespace MemeFolder.ConsoleTest
{
    class Program
    {
        //static public void CreateData(MemeFolderDbContext context)
        //{
        //    Folder folderRoot = new Folder();
        //    folderRoot.Title = "root";

        //    Folder f1 = new Folder();
        //    f1.Title = "f1";
        //    Folder f2 = new Folder();
        //    f2.Title = "f2";
        //    folderRoot.Folders.Add(f1);
        //    folderRoot.Folders.Add(f2);


        //    Folder f11 = new Folder();
        //    f11.Title = "f11";
        //    Folder f12 = new Folder();
        //    f12.Title = "f12";
        //    f1.Folders.Add(f11);
        //    f1.Folders.Add(f12);


        //    Folder f21 = new Folder();
        //    f21.Title = "f21";
        //    Folder f22 = new Folder();
        //    f22.Title = "f22";
        //    f2.Folders.Add(f21);
        //    f2.Folders.Add(f22);

          
        //    context.AddRange(f1, f2, folderRoot);
        //    context.SaveChanges();
        //}

        //static public void RemoveData(MemeFolderDbContext context)
        //{
        //    var q = context.Memes.AsEnumerable();
        //    var t = q.ToList();
        //    context.Memes.RemoveRange(q);
        //    context.Folders.RemoveRange(context.Folders.AsEnumerable());
        //    context.MemeTags.RemoveRange(context.MemeTags.AsEnumerable());
        //    context.SaveChanges();
        //}

        //static public void LoadData(MemeFolderDbContext context)
        //{
        //    Folders = context.Folders.Include(m => m.Memes).ToList();
        //}

        //static public List<Meme> Memes;
        //static public List<Folder> Folders;
        //static public List<MemeTag> MemeTags;

        static void Main(string[] args)
        {
            //var context = new MemeFolderDbContextFactory().CreateDbContext(null);
            //RemoveData(context);
            ////CreateData(context);
            ////LoadData(context);

            ////context.Folders.Remove(Folders.FirstOrDefault(x => x.Title == "root"));

            //context.SaveChanges();


            String str;
            SqlConnection myConn = new SqlConnection("Server=127.0.0.1;Integrated security=SSPI;database=master");

            str = "CREATE DATABASE MyDatabase ON PRIMARY " +
             "(NAME = MyDatabase_Data, " +
             "FILENAME = 'C:\\MyDatabaseData.mdf', " +
             "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)" +
             "LOG ON (NAME = MyDatabase_Log, " +
             "FILENAME = 'C:\\MyDatabaseLog.ldf', " +
             "SIZE = 1MB, " +
             "MAXSIZE = 5MB, " +
             "FILEGROWTH = 10%)";

            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
                Console.WriteLine("DataBase is Created Successfully");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
               
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }

            return;
        }
    }
}


//public static Folder operator + (Folder folder, FolderVM folderVM)
//{

//    return null;
//}