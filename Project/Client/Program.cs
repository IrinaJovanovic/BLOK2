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
            ChannelFactory<IDatabaseManagement> channel = new ChannelFactory<IDatabaseManagement>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:4001/IDataBaseManagement"));
            IDatabaseManagement proxy = channel.CreateChannel();

            //Connect();
            Consumer c1 = new Consumer();
            Consumer c2 = new Consumer("2345", "Backi", "Zrenjanin", "2013", 245);
            Consumer c3 = new Consumer("3456", "Pomoravski", "Kragujevac", "2017", 5678);
            Consumer c4 = new Consumer("4567", "Macvanski", "Sabac", "2019", 3333);

            string fileName = @"..\..\..\" + "NoviFajl";

            try
            {
                proxy.AddConsumer(c1);
                proxy.AddConsumer(c2);
                proxy.AddConsumer(c3);
                proxy.AddConsumer(c4);
            }
            catch (FaultException<MyException> ex)
            {
                Console.WriteLine(ex.Detail.Reason);
            }

            try
            {
                proxy.CreateFile(fileName);
            }
            catch (FaultException<MyException> ex)
            {
                Console.WriteLine(ex.Detail.Reason);
            }

            try
            {
                proxy.ModificationConsumer("1111", "Vojvodina", 123.2, "2006", "Beograd"); //test za exception - ne treba da radi
            }
            catch (FaultException<MyException> ex)
            {
                Console.WriteLine(ex.Detail.Reason);
            }

            try
            {
                proxy.ModificationConsumer("1234", "Vojvodina", 123.2, "2006", "Beograd"); //test za exception - treba da radi
            }
            catch (FaultException<MyException> ex)
            {
                Console.WriteLine(ex.Detail.Reason);
            }

            try
            {
                proxy.SaveConsumation(fileName);
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
