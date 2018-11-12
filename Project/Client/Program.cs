using Common;
using Server;
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
            //ataBaseManagement failOverClient = new FailOverClient();
            //failOverClient.CreateFile();
            ChannelFactory<DataBaseManagement> channel = new ChannelFactory<IDatabaseManagement>(typeof(IDatabaseManagement).ToString());
            IDatabaseManagement proxy = channel.CreateChannel();
            //Connect();
            Consumer c1 = new Consumer();
            Consumer c2 = new Consumer("2345", "Backi", "Zrenjanin", "2013", 245);
            Consumer c3 = new Consumer("3456", "Backi", "Zrenjanin", "2017", 5678);
            Consumer c4 = new Consumer("4567", "Macvanski", "Sabac", "2019", 3333);
            Consumer c5 = new Consumer("3458", "Backi", "Zrenjanin", "2017", 9888);


            try
            {
                proxy.AddConsumer(c1);
                proxy.AddConsumer(c2);
                proxy.AddConsumer(c2);
                proxy.AddConsumer(c3);
                proxy.AddConsumer(c4);
                proxy.AddConsumer(c5);
                proxy.ModificationConsumer("2345", "Macvanski", "Kraljevo", "2010", 1324);
                Console.WriteLine("Modifikacija");
            }
            catch (FaultException<MyException> ex)
            {
                Console.WriteLine(ex.Detail.Reason);
            }


             try
             {
                 double srednjaVrednost = proxy.CityConsumtion(fileName, "NoviSad");
                 Console.WriteLine("Srednja vrednost je:{0}", srednjaVrednost);
                 Console.WriteLine("Srednja vrednost je:{0}", proxy.CityConsumtion(fileName, "Zrenjanin"));

             }
             catch (FaultException<MyException> ex)
             {
                 Console.WriteLine(ex.Detail.Reason);
             }
             try
             {
                 Console.WriteLine("Srednja vrednost je:{0}", proxy.RegionConsumtion(fileName, "Backi"));

             }
             catch (FaultException<MyException> ex)
             {
                 Console.WriteLine(ex.Detail.Reason);
             }

             try
             {

                 Console.WriteLine("Maksimalna vrednost je:{0}", proxy.MaxRegionConsumation(fileName, "Backi"));

             }
             catch (FaultException<MyException> ex)
             {
                 Console.WriteLine(ex.Detail.Reason);
             }
             try
             {
                 proxy.ModificationConsumer("1111", "Vojvodina", 123.2, "2006", "Beograd"); //test za exception - ne treba da radi
                 Console.WriteLine("MODIFIKOVAN CONSUMER");
             }
             catch (FaultException<MyException> ex)
             {
                 Console.WriteLine(ex.Detail.Reason);
             }

             try
             {
                 proxy.ModificationConsumer("1234", "Vojvodina", 123.2, "2006", "Beograd"); //test za exception - treba da radi
                 Console.WriteLine("MODIFIKOVAN CONSUMER");
             }
             catch (FaultException<MyException> ex)
             {
                 Console.WriteLine(ex.Detail.Reason);
             }

             try
             {
                 proxy.ArchiveConsumation(fileName);
             }
             catch (FaultException<MyException> ex)
             {
                 Console.WriteLine(ex.Detail.Reason);
             }



             try
             {
                 proxy.RemoveConsumation(fileName);
             }
             catch (FaultException<MyException> ex)
             {
                 Console.WriteLine(ex.Detail.Reason);
             }
             
            Console.ReadLine();


        }
    }
}
