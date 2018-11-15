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
           
            Meni(failOverClient);
            //IDataBaseManagement failOverClient = new FailOverClient();
            //failOverClient.CreateFile();
            //Console.WriteLine("Kreiran fajl!");
            //Connect();
            Consumer c1 = new Consumer();
            Consumer c2 = new Consumer("2345", "Backi", "Zrenjanin", "2013", 245);
            Consumer c3 = new Consumer("3456", "Backi", "Zrenjanin", "2017", 5678);
            Consumer c4 = new Consumer("4567", "Macvanski", "Sabac", "2019", 3333);
            Consumer c5 = new Consumer("3458", "Backi", "Zrenjanin", "2017", 9888);


            //failOverClient.AddConsumer(c1);
            //failOverClient.AddConsumer(c2);
            //failOverClient.AddConsumer(c2);
            //failOverClient.AddConsumer(c3);
            //failOverClient.AddConsumer(c4);
            //failOverClient.AddConsumer(c5);
            //failOverClient.ModificationConsumer(new Consumer("2345", "Macvanski", "Kraljevo", "2010", 132));
            //Console.WriteLine("Modifikacija");

            //double srednjaVrednost = failOverClient.CityConsumtion("NoviSad");
            //Console.WriteLine("Srednja vrednost je:{0}", srednjaVrednost);
            //Console.WriteLine("Srednja vrednost je:{0}", failOverClient.CityConsumtion("Zrenjanin"));


            //Console.WriteLine("Srednja vrednost je:{0}", failOverClient.RegionConsumtion("Backi"));

            //Console.WriteLine("Maksimalna vrednost je:{0}", failOverClient.MaxRegionConsumation("Backi"));


            //failOverClient.ModificationConsumer(new Consumer("1111", "Vojvodina", "Beograd", "2006", 123.2)); //test za exception - ne treba da radi
            //Console.WriteLine("MODIFIKOVAN CONSUMER");


            //failOverClient.ModificationConsumer(new Consumer("1234", "Vojvodina", "Beograd", "2006", 123.2)); //test za exception - treba da radi
            //Console.WriteLine("MODIFIKOVAN CONSUMER");

            //failOverClient.ArchiveConsumation();

            //failOverClient.RemoveConsumation();

           // Console.ReadLine();
            //failOverClient.AddConsumer(c1);
           // Console.ReadLine();


            //failOverClient.AddConsumer(c1);
            //failOverClient.AddConsumer(c2);
            //failOverClient.AddConsumer(c2);
            //failOverClient.AddConsumer(c3);
            //failOverClient.AddConsumer(c4);
            //failOverClient.AddConsumer(c5);
            //failOverClient.ModificationConsumer(new Consumer("2345", "Macvanski", "Kraljevo", "2010", 132));
            //Console.WriteLine("Modifikacija");

            // srednjaVrednost = failOverClient.CityConsumtion("NoviSad");
            //Console.WriteLine("Srednja vrednost je:{0}", srednjaVrednost);
            //Console.WriteLine("Srednja vrednost je:{0}", failOverClient.CityConsumtion("Zrenjanin"));


            //Console.WriteLine("Srednja vrednost je:{0}", failOverClient.RegionConsumtion("Backi"));

            //Console.WriteLine("Maksimalna vrednost je:{0}", failOverClient.MaxRegionConsumation("Backi"));


            //failOverClient.ModificationConsumer(new Consumer("1111", "Vojvodina", "Beograd", "2006", 123.2)); //test za exception - ne treba da radi
            //Console.WriteLine("MODIFIKOVAN CONSUMER");


            //failOverClient.ModificationConsumer(new Consumer("1234", "Vojvodina", "Beograd", "2006", 123.2)); //test za exception - treba da radi
            //Console.WriteLine("MODIFIKOVAN CONSUMER");

            //failOverClient.ArchiveConsumation();

            //failOverClient.RemoveConsumation();
            //Console.ReadKey();

        }

        public static void Meni(IDataBaseManagement failOverClient)
        {
            bool shouldQuit = false;
            while (!shouldQuit)
            {
                int selection = Menu();
                switch (selection)
                {
                    case 1:
                        {
                            if (failOverClient.CreateFile())
                            {
                                Console.WriteLine("Napravljen fajl");
                            }
                            else
                            {
                                Console.WriteLine("Vec postoji");
                            }
                          }
                        break;
                    case 2:
                        {

                            if (failOverClient.ArchiveConsumation())
                                Console.WriteLine("arhiviran fajl");
                            else { Console.WriteLine("ne moze se arhivirati");
                            }
                        }
                        break;
                    case 3:
                        {
                            if (failOverClient.RemoveConsumation())
                            {
                                Console.WriteLine("obrisan fajl");
                            }
                            else { Console.WriteLine("Nema sta da se obrise");
                            }
                            // Do whatever you want in here!
                        }
                        break;
                    case 4:
                        {
                            Consumer inputConsumer = InputConsumer();
                            if (failOverClient.AddConsumer(inputConsumer))
                            {
                                Console.WriteLine("Dodat consumer");
                            }
                            else
                            {
                                Console.WriteLine("Ne mozete dodati trazenog klijenta");
                            }
                            // Do whatever you want in here!
                        }
                        break;
                    case 5:
                        {
                            Console.WriteLine("Enter ID OF CONSUMER TO MODIFY IT");
                            Consumer inputConsumer = InputConsumer();
                            if (failOverClient.ModificationConsumer(inputConsumer))
                            { Console.WriteLine("Uspesno modifikovan"); }
                            else
                            {
                                Console.WriteLine("Ne mozete ga dodati postoji vec sa tim id-om");
                            }
                            // Do whatever you want in here!
                        }
                        break;
                    case 6:
                        {
                            Console.WriteLine("Enter city which averege consumption you want ");
                            string city = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Srednja vrednost za grad:" + city + "je :" + failOverClient.CityConsumtion(city));
                            // Do whatever you want in here!
                        }
                        break;
                    case 7:
                        {
                            Console.WriteLine("Enter region which averege consumption you want ");
                            string region = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Srednja vrednost za region:" + region + "je :" + failOverClient.CityConsumtion(region));
                            // Do whatever you want in here!
                        }
                        break;
                    case 8:
                        {
                            Console.WriteLine("Enter region which max consumption you want ");
                            string region = Convert.ToString(Console.ReadLine());
                            Console.WriteLine("Max vrednost za region:" + region + "je :" + failOverClient.CityConsumtion(region));

                            // Do whatever you want in here!
                        }
                        break;
                    case 9:
                        {

                            shouldQuit = true;

                        }
                        break;
                }
            }
        }

        public static int Menu()
        {
            int menuSelection = 0;
            while ((menuSelection < 1) || (menuSelection > 9))
            {
                DisplayMenu(); // Just displays text
                var input = Console.ReadLine();
                if (!Int32.TryParse(input, out menuSelection))
                {
                    Console.WriteLine("ERROR option does not exists\n");
                }
            }
            return menuSelection;
        }
        public static void DisplayMenu()
        {
            Console.WriteLine("1.Create file.");
            Console.WriteLine("2.Archive database.");
            Console.WriteLine("3.Delete database.");
            Console.WriteLine("4.Add consumer to database.");
            Console.WriteLine("5.Modify consumer.");
            Console.WriteLine("6.Read city average consumption from database.");
            Console.WriteLine("7.Read region average consumption from database.");
            Console.WriteLine("8.Read max consumption from database.");
            Console.WriteLine("9.Exit");
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
