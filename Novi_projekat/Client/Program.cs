using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        // private static IDatabaseManagement proxy;
        static void Main(string[] args)
        {
            IDataBaseManagement failOverClient = new FailOverClient();

            //bool shouldQuit = false;
            //while (!shouldQuit)
            //{
            //    var selection = Menu();
            //    switch (selection)
            //    {
            //        case 1:
            //            {
            //                failOverClient.CreateFile();
            //            }
            //            break;
            //        case 2:
            //            {
            //                Consumer inputConsumer = InputConsumer();
            //                failOverClient.AddConsumer(inputConsumer);
            //                // Do whatever you want in here!
            //            }
            //            break;
            //        case 3:
            //            {
            //                // Do whatever you want in here!
            //            }
            //            break;
            //        case 4:
            //            {
            //                // Do whatever you want in here!
            //            }
            //            break;
            //        case 5:
            //            {
            //                // Do whatever you want in here!
            //            }
            //            break;
            //        case 6:
            //            {
            //                // Do whatever you want in here!
            //            }
            //            break;
            //        case 7:
            //            {
            //                // Do whatever you want in here!
            //            }
            //            break;
            //        case 8:
            //            {
            //                shouldQuit = true;
            //                // Do whatever you want in here!
            //            }
            //            break;
            //    }
            //}
            //IDataBaseManagement failOverClient = new FailOverClient();
            failOverClient.CreateFile();

            //Connect();
            Consumer c1 = new Consumer();
            Consumer c2 = new Consumer("2345", "Backi", "Zrenjanin", "2013", 245);
            Consumer c3 = new Consumer("3456", "Backi", "Zrenjanin", "2017", 5678);
            Consumer c4 = new Consumer("4567", "Macvanski", "Sabac", "2019", 3333);
            Consumer c5 = new Consumer("3458", "Backi", "Zrenjanin", "2017", 9888);


            failOverClient.AddConsumer(c1);
            failOverClient.AddConsumer(c2);
            failOverClient.AddConsumer(c2);
            failOverClient.AddConsumer(c3);
            failOverClient.AddConsumer(c4);
            failOverClient.AddConsumer(c5);
            failOverClient.ModificationConsumer(new Consumer("2345", "Macvanski", "Kraljevo", "2010", 132));
            Console.WriteLine("Modifikacija");

            double srednjaVrednost = failOverClient.CityConsumtion("NoviSad");
            Console.WriteLine("Srednja vrednost je:{0}", srednjaVrednost);
            Console.WriteLine("Srednja vrednost je:{0}", failOverClient.CityConsumtion("Zrenjanin"));


            Console.WriteLine("Srednja vrednost je:{0}", failOverClient.RegionConsumtion("Backi"));

            Console.WriteLine("Maksimalna vrednost je:{0}", failOverClient.MaxRegionConsumation("Backi"));


            failOverClient.ModificationConsumer(new Consumer("1111", "Vojvodina", "Beograd", "2006", 123.2)); //test za exception - ne treba da radi
            Console.WriteLine("MODIFIKOVAN CONSUMER");


            failOverClient.ModificationConsumer(new Consumer("1234", "Vojvodina", "Beograd", "2006", 123.2)); //test za exception - treba da radi
            Console.WriteLine("MODIFIKOVAN CONSUMER");

            failOverClient.ArchiveConsumation();

            //failOverClient.RemoveConsumation();

            Console.ReadLine();
            failOverClient.AddConsumer(c1);
            Console.ReadLine();


            failOverClient.AddConsumer(c1);
            failOverClient.AddConsumer(c2);
            failOverClient.AddConsumer(c2);
            failOverClient.AddConsumer(c3);
            failOverClient.AddConsumer(c4);
            failOverClient.AddConsumer(c5);
            failOverClient.ModificationConsumer(new Consumer("2345", "Macvanski", "Kraljevo", "2010", 132));
            Console.WriteLine("Modifikacija");

             srednjaVrednost = failOverClient.CityConsumtion("NoviSad");
            Console.WriteLine("Srednja vrednost je:{0}", srednjaVrednost);
            Console.WriteLine("Srednja vrednost je:{0}", failOverClient.CityConsumtion("Zrenjanin"));


            Console.WriteLine("Srednja vrednost je:{0}", failOverClient.RegionConsumtion("Backi"));

            Console.WriteLine("Maksimalna vrednost je:{0}", failOverClient.MaxRegionConsumation("Backi"));


            failOverClient.ModificationConsumer(new Consumer("1111", "Vojvodina", "Beograd", "2006", 123.2)); //test za exception - ne treba da radi
            Console.WriteLine("MODIFIKOVAN CONSUMER");


            failOverClient.ModificationConsumer(new Consumer("1234", "Vojvodina", "Beograd", "2006", 123.2)); //test za exception - treba da radi
            Console.WriteLine("MODIFIKOVAN CONSUMER");

            failOverClient.ArchiveConsumation();

            failOverClient.RemoveConsumation();

        }
        public static int Menu()
        {
            int menuSelection = 0;
            while ((menuSelection < 1) || (menuSelection > 8))
            {
                DisplayMenu(); // Just displays text
                var input = Console.ReadLine();
                if (!Int32.TryParse(input, out menuSelection))
                {
                    // Display some error message
                }
            }
            return menuSelection;
        }
        public static void DisplayMenu()
        {
            Console.WriteLine("1.Create file.");
            Console.WriteLine("2.Archive consumer.");
            Console.WriteLine("3.Delete database.");
            Console.WriteLine("4.Add consumer to database.");
            Console.WriteLine("5.Modify database.");
            Console.WriteLine("6.Read city average consumption from database.");
            Console.WriteLine("7.Read region average consumption from database.");
            Console.WriteLine("8.Read max consumption from database.");

        }
        public static Consumer InputConsumer()
        {
            Consumer inputConsumer = new Consumer();

            Console.WriteLine("Enter Consumer ID.");
            inputConsumer.ConsumerID = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Enter Consumer region.");
            inputConsumer.Region = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Enter Consumer city.");
            inputConsumer.City = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Enter Consumer year.");
            inputConsumer.Year = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Enter Consumer consumption.");
            inputConsumer.Consumation =Convert.ToDouble(Console.ReadLine());

            return inputConsumer;
        }

    }
}
