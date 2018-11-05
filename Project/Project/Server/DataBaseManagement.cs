using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security;
using System.ServiceModel;

namespace Server
{

    public class DataBaseManagement : IDatabaseManagement
    {
        public void AddConsumer(Consumer consumer, string fileName)
        {

            if (!DataBase.consumers.ContainsKey(consumer.ConumerID))
            {
                DataBase.consumers[consumer.ConumerID] = consumer;
                if (File.Exists(fileName))
                {

                    using (StreamReader sr = File.OpenText(fileName))
                    {
                        string s = "";
                        char[] delimiter = { ' ', '\n' };

                        // while ((s = sr.ReadLine()) != sr.EndOfStream)
                        do
                        {
                            s = sr.ReadLine();
                            string[] words = s.Split(delimiter);
                            if (!Equals(consumer.ConumerID, words[1]))
                            {
                                File.OpenWrite(fileName);
                                File.AppendText("ID" + " " + DataBase.consumers[consumer.ConumerID].ConumerID + " " + "region " + DataBase.consumers[consumer.ConumerID].Region
                                    + " " + "city " + DataBase.consumers[consumer.ConumerID].City + " " + "Year " + " " + DataBase.consumers[consumer.ConumerID].Year + " "
                                    + " Consumption " + " " + DataBase.consumers[consumer.ConumerID].Consumation);

                            }
                        } while (!sr.EndOfStream);
                    }

                }
                else
                {
                    MyException ex = new MyException("Error! File doesn't exists \n");
                    throw new FaultException<MyException>(ex);
                }
            }
            else
            {
                MyException ex = new MyException("Error! Consumer already exists \n");
                throw new FaultException<MyException>(ex);
            }

        }

        public double CityConsumtion(string fileName, string city)
        {
            double avg = 0;
            double avgTemp = 0;
            int count = 0;
            if (!File.Exists(fileName))
            {
                MyException ex = new MyException("Error! file not find\n");
                throw new FaultException<MyException>(ex);
            }
            else
            {
                using (StreamReader sr = File.OpenText(fileName))
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
            return Math.Round(avg / count, 2);
        }

        public void CreateFile(string fileName)
        {

            if (!File.Exists(fileName))
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    foreach (var item in DataBase.consumers)
                    {
                        sw.WriteLine("ID:" + " " + item.Key + " " + "region " + item.Value.Region + " " + "city " + item.Value.City + " " + "Year " + " " + item.Value.Year + " " + " Consumption " + " " + item.Value.Consumation);

                    }
                    sw.Close();
                }
            else
            {
                MyException ex = new MyException("Error! File with this name already exists\n");
                throw new FaultException<MyException>(ex);
            }

        }

        public double MaxRegionConsumation(string fileName, string region)
        {
            double maxTemp = 0;

            double max = 0;   //!

            if (!File.Exists(fileName))
            {
                MyException ex = new MyException("Error! file not find\n");
                throw new FaultException<MyException>(ex);
            }
            else
            {
                using (StreamReader sr = File.OpenText(fileName))
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
            return max;
        }

        public void ModificationConsumer(string ID, string region, double consamption, string year, string city)//treba da se dada u fajil
        {
            if (DataBase.consumers.ContainsKey(ID))
            {
                DataBase.consumers[ID].Region = region;
                DataBase.consumers[ID].City = city;
                DataBase.consumers[ID].Consumation = consamption;
                DataBase.consumers[ID].Year = year;

            }
            else
            {
                MyException ex = new MyException("Error!Consumer with desired ID not find\n");
                throw new FaultException<MyException>(ex);
            }

        }

        public double RegionConsumtion(string fileName, string region)
        {
            double avg = 0;
            double avgTemp = 0;
            int count = 0;
            if (!File.Exists(fileName))
            {
                MyException ex = new MyException("Error! file not find\n");
                throw new FaultException<MyException>(ex);
            }
            else
            {
                using (StreamReader sr = File.OpenText(fileName))
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
            return Math.Round((avg / count), 2);
        }


        public void RemoveConsumation(string fileName) // da li samo jednog ili citavog fajla
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);

                if (fileName.Contains("_Copy"))
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

        public void ArchiveConsumation(string fileName)
        {

            string name;

            if (!File.Exists(fileName))
            {
                MyException ex = new MyException("Error! File doesn't exist\n");
                throw new FaultException<MyException>(ex);
            }
            else
            {

                Common.Consumer.counter++;
                name = fileName + "_Copy" + "(" + Common.Consumer.counter + ")";
                File.Copy(fileName, name);
            }


        }

    }
}
