using Common;
using System;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading;

namespace SystemMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            IStateService primarni = null, sekundarni = null;
            EStateServers stanjePrimar, stanjeSekundar;

            while (true)
            {
                try
                {
                    ChannelFactory<IStateService> channel = new ChannelFactory<IStateService>("server1");
                    primarni = channel.CreateChannel();
                    stanjePrimar = primarni.StateCheck();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Greska na primarnom: " + ex.Message);
                    stanjePrimar = EStateServers.Nedostupno;
                }

                try
                {
                    ChannelFactory<IStateService> channel = new ChannelFactory<IStateService>("server2");
                    sekundarni = channel.CreateChannel();
                    stanjeSekundar = sekundarni.StateCheck();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Greska na sekundarnom: " + ex.Message);
                    stanjeSekundar = EStateServers.Nedostupno;
                }

                Console.WriteLine($"Stanja servisa.\n- primarni: {stanjePrimar}\n- sekundarni: {stanjeSekundar}");

                if ((stanjePrimar == EStateServers.Nepoznato || stanjePrimar == EStateServers.Sekundarni) && stanjeSekundar == EStateServers.Nepoznato)
                {
                    SetState(primarni, EStateServers.Primarni);
                    SetState(sekundarni, EStateServers.Sekundarni);
                }
                else if (stanjePrimar == EStateServers.Primarni && (stanjeSekundar == EStateServers.Nepoznato || stanjeSekundar == EStateServers.Primarni))
                {
                    SetState(sekundarni, EStateServers.Sekundarni);
                }
                else if (stanjePrimar == EStateServers.Nepoznato && stanjeSekundar == EStateServers.Primarni)
                {
                    SetState(primarni, EStateServers.Sekundarni);
                }
                else if ((stanjePrimar == EStateServers.Sekundarni || stanjePrimar == EStateServers.Nepoznato) && (stanjeSekundar == EStateServers.Nedostupno || stanjeSekundar == EStateServers.Sekundarni))
                {
                    SetState(primarni, EStateServers.Primarni);
                }
                else if (stanjePrimar == EStateServers.Nedostupno && (stanjeSekundar == EStateServers.Sekundarni || stanjeSekundar == EStateServers.Nepoznato))
                {
                    SetState(sekundarni, EStateServers.Primarni);
                }
                else if (stanjePrimar == EStateServers.Nedostupno && stanjeSekundar == EStateServers.Nedostupno)
                {
                    Console.WriteLine("Oba servera su pala. ALARM!!!");
                }

                Thread.Sleep(5000);
            }
        }

        private static void SetState(IStateService proxy, EStateServers state)
        {
            try
            {
                proxy.StateUpdate(state);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska na sekundarnom: " + ex.Message); //logovati!
            }
        }
    }
}

