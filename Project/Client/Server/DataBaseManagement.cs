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
        public void AddConsumer(Consumer consumer)//metoda za writera
        {
            IPrincipal principal = Thread.CurrentPrincipal;

            //if (!principal.IsInRole("Writer")){

            //}

            if (!File.Exists(DataBase.FileName))
            {
                MyException ex = new MyException("Error! file not found\n");
                throw new FaultException<MyException>(ex);
            }

            if (DataBase.consumers.ContainsKey(consumer.ConsumerID))
            {
                return;
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
        }

        public double CityConsumtion(string city)//metoda za readera
        {
            double avg = 0;
            double avgTemp = 0;
            int count = 0;
            IPrincipal principal = Thread.CurrentPrincipal;

            //  if (principal.IsInRole("Reader"))
            {
                if (!File.Exists(DataBase.FileName))
                {
                    MyException ex = new MyException("Error! file not find\n");
                    throw new FaultException<MyException>(ex);
                }
                else
                {
                    using (StreamReader sr = File.OpenText(DataBase.FileName))
                    {
                        string s = "";
                        char[] delimiter = { ' ', '\n' };

                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] words = s.Split(delimiter);
                            if (Equals(city, words[5]))
                            {
                                if (double.TryParse(words[12], out avgTemp))
                                {
                                    avg += avgTemp;
                                    count++;
                                }
                                else
                                {
                                    Console.WriteLine("String could not be parsed!\n");
                                }
                            }
                        }

                    }

                }
                Console.WriteLine("Srednja vrednost za grad " + city + " je {0}", Math.Round((avg / count), 2));
            }
            return Math.Round(avg / count, 2);

        }

        public void CreateFile() //ovde treba samo da se kreira fajl, jer ce to raditi admin, a pravo upisa ima samo writter i to se morati u posebnoj metodi 
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            ///if (principal.IsInRole("Admin"))
            {
                if (!File.Exists(DataBase.FileName))
                {
                    FileStream fs = File.Create(DataBase.FileName);
                    fs.Close();

                }
            }
            //else
            //{
            //    MyException ex = new MyException("Error! File with this name already exists\n");
            //    throw new FaultException<MyException>(ex);
            //}

        }

        public double MaxRegionConsumation(string region)
        {

            double maxTemp = 0;

            double max = 0;   //!

            IPrincipal principal = Thread.CurrentPrincipal;
            //   if (principal.IsInRole("Reader"))
            {

                if (!File.Exists(DataBase.FileName))
                {
                    MyException ex = new MyException("Error! file not find\n");
                    throw new FaultException<MyException>(ex);
                }
                else
                {
                    using (StreamReader sr = File.OpenText(DataBase.FileName))
                    {
                        string s = "";
                        char[] delimiter = { ' ', '\n' };

                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] words = s.Split(delimiter);
                            if (Equals(region, words[3]))
                            {
                                if (double.TryParse(words[12], out maxTemp))
                                {
                                    if (max < maxTemp)
                                    {
                                        max = maxTemp;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("String could not be parsed!\n");
                                }
                            }
                        }

                    }

                }
                Console.WriteLine("Maksimalna vrednost za " + region + " region je {0}", max);
            }
            return max;
        }



        public void ModificationConsumer(string ID, string region, string city, string year, double consamption)//treba da se doda u fajl modifikovani potrosac
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            // if (principal.IsInRole("Writer"))
            {
                if (!DataBase.consumers.ContainsKey(ID))
                {
                }
                if (!File.Exists(DataBase.FileName))
                {
                    MyException ex = new MyException("Error!Cant open file\n");
                    throw new FaultException<MyException>(ex);
                }

                string TextNew = CreateConsumerString(ID, region, city, year, consamption);
                string TextOld = CreateConsumerString(DataBase.consumers[ID].ConsumerID, DataBase.consumers[ID].Region, DataBase.consumers[ID].City, DataBase.consumers[ID].Year, DataBase.consumers[ID].Consumation);
                string TextAll = File.ReadAllText(DataBase.FileName);
                string NewTextAll = TextAll.Replace(TextOld, TextNew);

                File.WriteAllText(DataBase.FileName, NewTextAll);

                DataBase.consumers[ID].Region = region;
                DataBase.consumers[ID].City = city;
                DataBase.consumers[ID].Consumation = consamption;
                DataBase.consumers[ID].Year = year;
            }
        }



        public double RegionConsumtion(string region)  //srednja potronja za odredjeni region
        {
            double avg = 0;
            double avgTemp = 0;
            int count = 0;
            IPrincipal principal = Thread.CurrentPrincipal;
            // if (principal.IsInRole("Reader"))
            {
                if (!File.Exists(DataBase.FileName))
                {
                    MyException ex = new MyException("Error! file not found\n");
                    throw new FaultException<MyException>(ex);
                }
                else
                {
                    using (StreamReader sr = File.OpenText(DataBase.FileName))
                    {
                        string s = "";
                        char[] delimiter = { ' ', '\n' };

                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] words = s.Split(delimiter);
                            if (Equals(region, words[3]))
                            {
                                if (double.TryParse(words[12], out avgTemp))
                                {
                                    avg += avgTemp;
                                    count++;
                                }
                                else
                                {
                                    Console.WriteLine("String could not be parsed!\n");
                                }
                            }
                        }

                    }

                }
                Console.WriteLine("Srednja vrednost za " + region + " region je {0}", Math.Round((avg / count), 2));
            }
            return Math.Round((avg / count), 2);
        }


        public void RemoveConsumation() //admin-pravo uklanjanja baze podataka (fajl-a)
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            // if (principal.IsInRole("Admin"))
            {
                if (File.Exists(DataBase.FileName))
                {
                    File.Delete(DataBase.FileName);

                    if (DataBase.FileName.Contains("_Copy"))
                    {
                        Common.Consumer.counter--;
                    }
                }
                else
                {

                    MyException ex = new MyException("Error! File cannot be find\n");
                    throw new FaultException<MyException>(ex);
                }
            }
        }

        public void ArchiveConsumation()
        {
            string name;
            IPrincipal principal = Thread.CurrentPrincipal;
            // if (!principal.IsInRole("Admin"))
            {
            }

            if (!File.Exists(DataBase.FileName))
            {
                MyException ex = new MyException("Error! File doesn't exist\n");
                throw new FaultException<MyException>(ex);
            }

            Common.Consumer.counter++;
            name = DataBase.FileName + "_Copy" + "(" + Common.Consumer.counter + ")";
            File.Copy(DataBase.FileName, name);

            if(StateService.stateService == EStateServers.Primarni)
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

            bool changed = false;

            foreach (Consumer c in DataBase.consumers.Values)
            {
                if (!data.ContainsKey(c.ConsumerID))
                {
                    //da li je objekat mozda izbrisan 
                    DataBase.consumers.Remove(c.ConsumerID);
                    changed = true;


                }
                else if (c.TimeStamp != data[c.ConsumerID].TimeStamp)
                {
                    DataBase.consumers[c.ConsumerID] = data[c.ConsumerID];
                    changed = true;
                }

            }
            foreach (Consumer c in data.Values)
            {
                if (!DataBase.consumers.ContainsKey(c.ConsumerID))
                {
                    DataBase.consumers[c.ConsumerID] = c;
                    changed = true;
                }
            }

            if (changed)
            {
                Console.WriteLine("Promenjeno u replikaciji nesto");
            }
        }

        private string CreateConsumerString(string ID, string region, string city, string year, double consamption)
        {
            return $"ID {ID} region {region} city {city} Year {year} Consumption {consamption}";
        }

        private bool ValidateFile(string fileName)
        {
            bool exists = false;
            fileName = fileName + ".txt";

            if (!File.Exists(fileName))
            {
                MyException ex = new MyException("Error! File doesn't exists \n");
                throw new FaultException<MyException>(ex);

            }
            else
            {
                exists = true;
            }
            return exists;
        }


    }
}
