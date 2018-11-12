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
            IPrincipal principal = Thread.CurrentPrincipal;
            //if (!principal.IsInRole("Wr")){

            //}

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
            IPrincipal principal = Thread.CurrentPrincipal;
            // if (!principal.IsInRole("Writer"))
            //{ }

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
            //IPrincipal principal = Thread.CurrentPrincipal;
            //if (!principal.IsInRole("Reader"))
            //{
            //}

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

            //IPrincipal principal = Thread.CurrentPrincipal;
            //if (!principal.IsInRole("Reader"))
            //{
            //}

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
            //IPrincipal principal = Thread.CurrentPrincipal;
            //if (!principal.IsInRole("Reader"))
            //{
            //}

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
            IPrincipal principal = Thread.CurrentPrincipal;
            ///if (!principal.IsInRole("Admin"))
            ///{
            ///}

            if (File.Exists(DataBase.FileName))
            {
                return false;
            }

            lock (DataBase.lockObject)
            {
                FileStream fs = File.Create(DataBase.FileName);
                fs.Close();
            }

            if (StateService.stateService == EStateServers.Primarni)
            {
                ChannelFactory<IDataBaseManagement> cfh2 = new ChannelFactory<IDataBaseManagement>("sekundarni");
                IDataBaseManagement proxy2 = cfh2.CreateChannel();
                proxy2.CreateFile();
            }

            return true;
        }

        public void RemoveConsumation() //admin-pravo uklanjanja baze podataka (fajl-a)
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            // if (!principal.IsInRole("Admin"))
            //{}

            if (!File.Exists(DataBase.FileName))
            {
                MyException ex = new MyException("Error! File cannot be find\n");
                throw new FaultException<MyException>(ex);
            }

            lock (DataBase.lockObject)
            {
                File.Delete(DataBase.FileName);
            }

            if (StateService.stateService == EStateServers.Primarni)
            {
                ChannelFactory<IDataBaseManagement> cfh2 = new ChannelFactory<IDataBaseManagement>("sekundarni");
                IDataBaseManagement proxy2 = cfh2.CreateChannel();
                proxy2.RemoveConsumation();
            }
        }

        public void ArchiveConsumation()
        {
           
            IPrincipal principal = Thread.CurrentPrincipal;
            // if (!principal.IsInRole("Admin"))
            //{
            //}

            if (!File.Exists(DataBase.FileName))
            {
                MyException ex = new MyException("Error! File doesn't exist\n");
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
                ChannelFactory<IDataBaseManagement> cfh2 = new ChannelFactory<IDataBaseManagement>("sekundarni");
                IDataBaseManagement proxy2 = cfh2.CreateChannel();
                proxy2.ArchiveConsumation();
            }
        }

        public Dictionary<string, Consumer> UzmiSve()
        {
            //throw new NotImplementedException();
            return DataBase.consumers;
        }

        public void AddAll(Dictionary<string, Consumer> data)
        {
            lock (DataBase.lockObject)
            {
                foreach (Consumer c in data.Values)
                {
                    DataBase.consumers[c.ConsumerID] = c;
                }
            }
        }

        private string CreateConsumerString(string ID, string region, string city, string year, double consamption)
        {
            return $"ID {ID} region {region} city {city} Year {year} Consumption {consamption}";
        }
    }
}
