using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
       // private static ServiceHost svc;

        static void Main(string[] args)
        {
            ServiceHost svc = new ServiceHost(typeof(DataBaseManagement));
            svc.AddServiceEndpoint(typeof(IDatabaseManagement), new NetTcpBinding(), new Uri("net.tcp://localhost:4001/IDataBaseManagement"));

            // Host();
            svc.Open();
           
            Console.WriteLine("Server je otvoren.");
            Console.ReadLine();
            svc.Close();
        }

        //static void Host()
        //{
        //    svc = new ServiceHost(typeof(DataBaseManagement));
        //    svc.Open();
        //}
    }
}
