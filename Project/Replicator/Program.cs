using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Replicator
{
    class Program
    {
        private static IDatabaseManagement proxy1;
        private static IDatabaseManagement proxy2;
        static void Main(string[] args)
        {
           
            while (true)
            {
                Connect();
                Dictionary<string, Consumer> data = proxy1.UzmiSve();
                proxy2.AddAll(data); //dodaj sve
                                     //proxy2 dodaje sve 

                Console.WriteLine("Odradjena replikacija");
            }
           
        }

        static void Connect()
        {
            ChannelFactory<IDatabaseManagement> cfh = new ChannelFactory<IDatabaseManagement>("primarni");
            proxy1 = cfh.CreateChannel();


            ChannelFactory<IDatabaseManagement> cfh2 = new ChannelFactory<IDatabaseManagement>("sekundarni");
            proxy2 = cfh2.CreateChannel();
        }
    }
}
