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
      

        static void Main(string[] args)
        {
            ServiceHost svc = new ServiceHost(typeof(DataBaseManagement));

           
            svc.Open();
           
            Console.WriteLine("Server je otvoren.");
            Console.ReadLine();
            svc.Close();
        }

        
    }
}
