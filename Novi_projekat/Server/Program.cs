using Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace Server
{
    class Program
    {
        private static ServiceHost svc;
        private static ServiceHost svc2;
        private static ServiceHost svc3;

        static void Main(string[] args)
        {
            

            ReadFileIfExist();
            Host();


            Thread replicateThread = new Thread(ReplicateThread);
            replicateThread.IsBackground = true;
            replicateThread.Start();

            Console.ReadLine();
        }

        static void Host()
        {
            svc = new ServiceHost(typeof(DataBaseManagement));
            svc2 = new ServiceHost(typeof(StateService));
            svc3 = new ServiceHost(typeof(Replicator));

            svc.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            svc.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
            svc.Authorization.PrincipalPermissionMode = System.ServiceModel.Description.PrincipalPermissionMode.Custom;

            svc.Open();
            svc2.Open();
            svc3.Open();
        }

        static void ReplicateThread()
        {
            while (true)
            {
                if(StateService.stateService != EStateServers.Primarni)
                {
                    Thread.Sleep(5000);
                    continue;
                }

               

                lock (DataBase.lockObject)
                {
                    try
                    {
                        ChannelFactory<IReplicator> cfh2 = new ChannelFactory<IReplicator>("sekundarni");
                        IReplicator proxy2 = cfh2.CreateChannel();
                        proxy2.SendDelta(DataBase.consumersDelta);
                        DataBase.consumersDelta.Clear();
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine("Greska pri replikaciji");
                    }
                }

                Thread.Sleep(2000);
            }
        }

        public static void ReadFileIfExist()
        {
            if (!File.Exists(DataBase.FileName))
            {
                return;
            }

            using (StreamReader sr = File.OpenText(DataBase.FileName))
            {
                string s = "";

                while ((s = sr.ReadLine()) != null)
                {
                    string[] words = s.Split(' ');
                    DataBase.consumers.Add(words[1], new Consumer(words[1], words[3], words[5], words[7], Double.Parse(words[9])));
                }
            }
        }
    }
}
