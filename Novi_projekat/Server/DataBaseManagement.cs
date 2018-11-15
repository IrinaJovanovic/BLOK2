using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security;
using System.ServiceModel;
using System.Security.Principal;
using System.Threading;
using System.Text.RegularExpressions;

namespace Server
{
    public class DataBaseManagement : IDataBaseManagement
    {
        public bool AddConsumer(Consumer consumer)
        {
            if (StateService.stateService != EStateServers.Primarni)
            {
                MyException ex = new MyException("Error! Not primary\n");
                throw new FaultException<MyException>(ex);
            }

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if (!principal.IsInRole(Permissions.Writting.ToString()))
            {
                SecurityException ex = new SecurityException("Error! IS NOT A Writer\n");
                throw new FaultException<SecurityException>(ex);
            }
            // CustomAuditBehavior.AuthenticationSuccess(principal.Identity.Name.ToString());
            if (!File.Exists(DataBase.FileName))
            {
                MyException ex = new MyException("Error! file not found\n");
                throw new FaultException<MyException>(ex);
            }

            if (DataBase.consumers.ContainsKey(consumer.ConsumerID))
            {
                return false;
            }

            lock (DataBase.lockObject)
            {
                DataBase.consumers[consumer.ConsumerID] = consumer;
                DataBase.consumersDelta[consumer.ConsumerID] = consumer;
                using (StreamWriter file = File.AppendText(DataBase.FileName))
                {
                    file.WriteLine(CreateConsumerString(consumer.ConsumerID, consumer.Region, consumer.City, consumer.Year, consumer.Consumation));
                }
            }

            return true;
        }

        public bool ModificationConsumer(Consumer consumer)//treba da se doda u fajl modifikovani potrosac
        {
            if (StateService.stateService != EStateServers.Primarni)
            {
                MyException ex = new MyException("Error! Not primary\n");
                throw new FaultException<MyException>(ex);
            }

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if (!principal.IsInRole(Permissions.Modify.ToString()))
            {
                SecurityException ex = new SecurityException("Error! IS NOT A Writer\n");
                throw new FaultException<SecurityException>(ex);
            }

            if (!DataBase.consumers.ContainsKey(consumer.ConsumerID))
            {
                return false;
            }

            if (!File.Exists(DataBase.FileName))
            {
                MyException ex = new MyException("Error!Cant open file\n");
                throw new FaultException<MyException>(ex);
            }

            string TextAll = string.Empty;
            string TextNew = CreateConsumerString(consumer.ConsumerID, consumer.Region, consumer.City, consumer.Year, consumer.Consumation);
            string TextOld = CreateConsumerString(DataBase.consumers[consumer.ConsumerID].ConsumerID, DataBase.consumers[consumer.ConsumerID].Region,
                DataBase.consumers[consumer.ConsumerID].City, DataBase.consumers[consumer.ConsumerID].Year, DataBase.consumers[consumer.ConsumerID].Consumation);

            lock (DataBase.lockObject)
            {
                TextAll = File.ReadAllText(DataBase.FileName);

                string NewTextAll = TextAll.Replace(TextOld, TextNew);

                File.WriteAllText(DataBase.FileName, NewTextAll);
                DataBase.consumers[consumer.ConsumerID] = consumer;
                DataBase.consumersDelta[consumer.ConsumerID] = consumer;
            }

            return true;
        }

        public double CityConsumtion(string city)//metoda za readera
        {
            if (StateService.stateService != EStateServers.Primarni)
            {
                MyException ex = new MyException("Error! Not primary\n");
                throw new FaultException<MyException>(ex);
            }

            if (StateService.stateService != EStateServers.Primarni)
            {
                MyException ex = new MyException("Error! Not primary\n");
                throw new FaultException<MyException>(ex);
            }

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if (!principal.IsInRole(Permissions.ReadingCityAvgConsumption.ToString()))
            {
                SecurityException ex = new SecurityException("Error! IS NOT A Reader\n");
                throw new FaultException<SecurityException>(ex);
            }

            if (!File.Exists(DataBase.FileName))
            {
                MyException ex = new MyException("Error! file not find\n");
                throw new FaultException<MyException>(ex);
            }

            double suma = 0;
            int count = 0;
            lock (DataBase.lockObject)
            {
                foreach (var item in DataBase.consumers.Values)
                {
                    if (item.City == city)
                    {
                        suma += item.Consumation;
                        count++;
                    }
                }
            }

            Console.WriteLine("Srednja vrednost za grad " + city + " je {0}", Math.Round((suma / count), 2));

            return suma / count;
        }

        public double MaxRegionConsumation(string region)
        {
            if (StateService.stateService != EStateServers.Primarni)
            {
                MyException ex = new MyException("Error! Not primary\n");
                throw new FaultException<MyException>(ex);
            }

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if (!principal.IsInRole(Permissions.ReadingMaxAvgConsumption.ToString()))
            {
                MyException ex = new MyException("Error! IS NOT A Reader\n");
                throw new FaultException<MyException>(ex);
            }


            if (!File.Exists(DataBase.FileName))
            {
                MyException ex = new MyException("Error! file not find\n");
                throw new FaultException<MyException>(ex);
            }

            double maxTemp = 0;
            double max = 0;
            lock (DataBase.lockObject)
            {
                foreach (var item in DataBase.consumers.Values)
                {
                    if (item.Region == region)
                    {
                        if (max < maxTemp)
                        {
                            max = maxTemp;
                        }
                        max = item.Consumation;
                    }
                }
            }
            Console.WriteLine("Maksimalna vrednost za " + region + " region je {0}", max);

            return max;
        }

        public double RegionConsumtion(string region)  //srednja potronja za odredjeni region
        {
            if (StateService.stateService != EStateServers.Primarni)
            {
                MyException ex = new MyException("Error! Not primary\n");
                throw new FaultException<MyException>(ex);
            }

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if (!principal.IsInRole(Permissions.ReadingRegionAvgConsumption.ToString()))
            {
                SecurityException ex = new SecurityException("Error! IS NOT A READER\n");
                throw new FaultException<SecurityException>(ex);
            }

            if (!File.Exists(DataBase.FileName))
            {
                MyException ex = new MyException("Error! file not find\n");
                throw new FaultException<MyException>(ex);
            }

            double suma = 0;
            int count = 0;
            lock (DataBase.lockObject)
            {
                foreach (var item in DataBase.consumers.Values)
                {
                    if (item.Region == region)
                    {
                        suma += item.Consumation;
                        count++;
                    }
                }
            }

            Console.WriteLine("Srednja vrednost za grad " + region + " je {0}", Math.Round((suma / count), 2));

            return suma / count;
        }

        public bool CreateFile() //ovde treba samo da se kreira fajl, jer ce to raditi admin, a pravo upisa ima samo writter i to se morati u posebnoj metodi 
        {
            if (StateService.stateService != EStateServers.Primarni)
            {
                MyException ex = new MyException("Error! IS NOT a primary\n");
                throw new FaultException<MyException>(ex);
            }

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            if (!principal.IsInRole(Permissions.Create.ToString()))
            {
                MyException ex = new MyException("Error! IS NOT A Reader\n");
                throw new FaultException<MyException>(ex);
            }
            //   CustomAuditBehavior.AuthenticationSuccess(principal.Identity.Name.ToString());

            if (File.Exists(DataBase.FileName))
            {
                return false;
            }

            lock (DataBase.lockObject)
            {
                FileStream fs = File.Create(DataBase.FileName);
                fs.Close();
            }
            try
            {
                if (StateService.stateService == EStateServers.Primarni)
                {
                    ChannelFactory<IReplicator> cfh2 = new ChannelFactory<IReplicator>("sekundarni");
                    IReplicator proxy2 = cfh2.CreateChannel();


                    proxy2.CreateFile();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            //WindowsIdentity identity = WindowsIdentity.GetCurrent();
            //if (!identity.IsAuthenticated)
            //{
            //    MyException ex = new MyException("Neuspela autentifikacija ");
            //    throw new FaultException<MyException>(ex);
            //}


            return true;
        }

        public bool RemoveConsumation() //admin-pravo uklanjanja baze podataka (fajl-a)
        {
            if (StateService.stateService != EStateServers.Primarni)
            {
                SecurityException ex = new SecurityException("Error! IS NOT A primary\n");
                throw new FaultException<SecurityException>(ex);
            }
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if (!principal.IsInRole(Permissions.Deleting.ToString()))
            {
                MyException ex = new MyException("Error! IS NOT A READER\n");
                throw new FaultException<MyException>(ex);
            }

            if (!File.Exists(DataBase.FileName))
            {
                return false;
                //MyException ex = new MyException("Error! File cannot be find\n");
                //throw new FaultException<MyException>(ex);
            }

            lock (DataBase.lockObject)
            {
                File.Delete(DataBase.FileName);

            }

            if (StateService.stateService == EStateServers.Primarni)
            {
                ChannelFactory<IReplicator> cfh2 = new ChannelFactory<IReplicator>("sekundarni");
                IReplicator proxy2 = cfh2.CreateChannel();
                proxy2.RemoveConsumation();
            }
            return true;
        }

        public bool ArchiveConsumation()
        {
            if (StateService.stateService != EStateServers.Primarni)
            {
                MyException ex = new MyException("Error! Not primary\n");
                throw new FaultException<MyException>(ex);
            }
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;


            if (!File.Exists(DataBase.FileName))
            {
                return false;
                //MyException ex = new MyException("Error! File doesn't exist\n");
                //throw new FaultException<MyException>(ex);
            }
            if (!principal.IsInRole(Permissions.Arhiving.ToString()))
            {
                MyException ex = new MyException("Error! IS NOT A Reader\n");
                throw new FaultException<MyException>(ex);
            }

            string fileNameCopy = DataBase.FileName;
            do
            {
                fileNameCopy += "-copy";

            } while (File.Exists(fileNameCopy));
            File.Copy(DataBase.FileName, fileNameCopy);

            if (StateService.stateService == EStateServers.Primarni)
            {
                ChannelFactory<IReplicator> cfh2 = new ChannelFactory<IReplicator>("sekundarni");
                IReplicator proxy2 = cfh2.CreateChannel();
                proxy2.ArchiveConsumation();
            }
            return true;
        }


        private string CreateConsumerString(string ID, string region, string city, string year, double consamption)
        {
            return $"ID {ID} region {region} city {city} Year {year} Consumption {consamption}";
        }

        public bool CheckIfAlive()
        {
            bool p = false;

            if (StateService.stateService != EStateServers.Primarni)
            {
                //MyException ex = new MyException("Error!");
                //throw new FaultException<MyException>(ex);
                return false;
            }
            p = true;
            return p;
        }
    }
}
