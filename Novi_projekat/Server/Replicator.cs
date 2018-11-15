using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace Server
{
    public class Replicator : IReplicator
    {
        public bool ArchiveConsumation()
        {
            if (StateService.stateService != EStateServers.Sekundarni)
            {
                MyException ex = new MyException("Error! Not secondary\n");
                throw new FaultException<MyException>(ex);
            }
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if (!principal.IsInRole(Permissions.Arhiving.ToString()))
            {
                MyException ex = new MyException("Error! IS NOT A Reader\n");
                throw new FaultException<MyException>(ex);
            }

            if (!File.Exists(DataBase.FileName))
            {
                //MyException ex = new MyException("Error! File doesn't exist\n");
                //throw new FaultException<MyException>(ex);
                return false;
            }

            string fileNameCopy = DataBase.FileName;
            do
            {
                fileNameCopy += "-copy";

            } while (File.Exists(fileNameCopy));
            File.Copy(DataBase.FileName, fileNameCopy);

            return true;
        }

        public bool CreateFile()
        {
            if (StateService.stateService != EStateServers.Sekundarni)
            {
                MyException ex = new MyException("Error! Not secundary\n");
                throw new FaultException<MyException>(ex);
            }
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if (!principal.IsInRole(Permissions.Create.ToString()))
            {
                MyException ex = new MyException("Error! IS NOT A Reader\n");
                throw new FaultException<MyException>(ex);
            }

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

        public bool RemoveConsumation()
        {
            if (StateService.stateService != EStateServers.Sekundarni)
            {
                MyException ex = new MyException("Error! Not secondary\n");
                throw new FaultException<MyException>(ex);
            }
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            if (!principal.IsInRole(Permissions.Deleting.ToString()))
            {
                MyException ex = new MyException("Error! IS NOT A Reader\n");
                throw new FaultException<MyException>(ex);
            }

            if (!File.Exists(DataBase.FileName))
            {
                //MyException ex = new MyException("Error! File cannot be find\n");
                //throw new FaultException<MyException>(ex);
                return false;
            }

            lock (DataBase.lockObject)
            {
                File.Delete(DataBase.FileName);
            }

            return true;
        }

        public bool SendDelta(Dictionary<string, Consumer> data)
        {
            if (StateService.stateService != EStateServers.Sekundarni)
            {
                MyException ex = new MyException("Error! Not secondary\n");
                throw new FaultException<MyException>(ex);
            }

            if (!File.Exists(DataBase.FileName))
            {
                return false;
            }

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
