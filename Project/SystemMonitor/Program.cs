using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SystemMonitor
{
    class Program
    {
        private static IStateService primarni;
        private static IStateService sekundarni;


        static void Main(string[] args)
        {
            Connect();
        }

        static void Connect()
        {
            try
            {
                ChannelFactory<IStateService> cPrimarni = new ChannelFactory<IStateService>("server1");
                primarni = cPrimarni.CreateChannel();
                primarni.AzuriranjeStanja(EStateServers.Primarni);
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine("Sekundarni servis nedostupan . Razlog: " + ex.Message);
            }

            try
            {
                ChannelFactory<IStateService> cSekundarni = new ChannelFactory<IStateService>("server2");
                primarni = cSekundarni.CreateChannel();
                primarni.AzuriranjeStanja(EStateServers.Sekundarni);
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine("Sekundarni servis nedostupan . Razlog: " + ex.Message);
            }

            if (primarni == null && sekundarni == null)
            {
                Console.WriteLine("Neuspelo povezivanje na servise");
                Environment.Exit(0);
            }
        }



    }

}

