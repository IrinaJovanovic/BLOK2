using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Server
{
    public class Replicator : IReplicator
    {
        public void ArchiveConsumation()
        {
            if (StateService.stateService != EStateServers.Sekundarni)
            {
                MyException ex = new MyException("Error! Not secundary\n");
                throw new FaultException<MyException>(ex);
            }
           // IPrincipal principal = Thread.CurrentPrincipal;
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

           
        }

        public bool CreateFile()
        {
            if (StateService.stateService != EStateServers.Sekundarni)
            {
                MyException ex = new MyException("Error! Not secundary\n");
                throw new FaultException<MyException>(ex);
            }
            //IPrincipal principal = Thread.CurrentPrincipal;
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

           

            return true;
        }

        public void RemoveConsumation()
        {
            if (StateService.stateService != EStateServers.Sekundarni)
            {
                MyException ex = new MyException("Error! Not secondary\n");
                throw new FaultException<MyException>(ex);
            }
            //IPrincipal principal = Thread.CurrentPrincipal;
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

           
        }

        public bool SendDelta(Dictionary<string, Consumer> data)
        {
            if (StateService.stateService != EStateServers.Sekundarni)
            {
                MyException ex = new MyException("Error! Not secondary\n");
                throw new FaultException<MyException>(ex);
            }
            if (!File.Exists(DataBase.FileName))
            { return false; }

            lock (DataBase.lockObject)
            {
                using (StreamWriter file = File.AppendText(DataBase.FileName))
                {
                    foreach (Consumer consumer in data.Values)
                    {
                        DataBase.consumers[consumer.ConsumerID] = consumer;
                        file.WriteLine(CreateConsumerString(consumer.ConsumerID, consumer.Region, consumer.City, consumer.Year, consumer.Consumation));
                        Console.WriteLine("Repliciran consumer, ID: " + consumer.ConsumerID);
                    }
                }
            }

            return true;
        }
        private string CreateConsumerString(string ID, string region, string city, string year, double consamption)
        {
            return $"ID {ID} region {region} city {city} Year {year} Consumption {consamption}";
        }
    }
}
