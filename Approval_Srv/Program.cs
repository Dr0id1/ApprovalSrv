using System;
using System.IO;
using System.Linq;
using System.Timers;

namespace Approval_Srv
{
    class Program
    {
        static int seconds = 0;

        static void Main(string[] args)
        {        
            // Timer initialisation
            Timer aTimer = new Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 10000;
            aTimer.Enabled = true;

            Console.WriteLine("Press \'q\' to quit.");
            while (Console.Read() != 'q') ;
        }

        // Timer Event
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            // Dtb conncetion
            manicEntities db = new manicEntities();
            seconds++;
            string sourcepath = "C:\\Users\\Dr0id\\Desktop\\Test";

            string[] files =
            Directory.GetFiles(sourcepath);

            foreach (var file in files)
            {
                string FileName = Path.GetFileName(file);
                var duplicate = db.invoices.Where(x => x.name == FileName).FirstOrDefault();

                if (duplicate is null)
                {
                    invoices i = new invoices();
                    i.name = Path.GetFileName(file);
                    i.sourcepath = file;
                    i.date = DateTime.Now;

                    db.invoices.Add(i);
                    db.SaveChanges();

                    Console.WriteLine("Le fichier " + FileName + " a été ajouté");
                }
                else
                {
                    Console.WriteLine("Le fichier " + FileName + " existe déjà");
                }
            }

            Console.ReadLine();
        }
    }
}
