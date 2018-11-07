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

namespace Server
{

    public class DataBaseManagement : IDatabaseManagement
    {

        


        public void AddConsumer(Consumer consumer, string fileName)//metoda za writera
        {
            IPrincipal principal = Thread.CurrentPrincipal;

            if (principal.IsInRole("Writer"))
            {

                if (!DataBase.consumers.ContainsKey(consumer.ConumerID))
                {
                    DataBase.consumers[consumer.ConumerID] = consumer;
                    if (!File.Exists(fileName))
                    {
                        MyException ex = new MyException("Error! File doesn't exists \n");
                        throw new FaultException<MyException>(ex);
                    }
                    else
                    {
                        using (StreamWriter file = File.AppendText(fileName))
                            /*foreach (var entry in DataBase.consumers)*/
                            file.WriteLine("ID" + " " + consumer.ConumerID + " " + "region " + consumer.Region
                                        + " " + "city " + consumer.City + " " + "Year " + " " + consumer.Year + " "
                                      + " Consumption " + " " + consumer.Consumation);


                    }

                }
            }

        }


        public double CityConsumtion(string fileName, string city)//metoda za readera
        {
            double avg = 0;
            double avgTemp = 0;
            int count = 0;
            IPrincipal principal = Thread.CurrentPrincipal;

            if (principal.IsInRole("Reader"))
            {
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
            }
                return Math.Round(avg / count, 2);
            
        }

        public void CreateFile(string fileName) //ovde treba samo da se kreira fajl, jer ce to raditi admin, a pravo upisa ima samo writter i to se morati u posebnoj metodi 
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            if (principal.IsInRole("Admin"))
            {
                if (!File.Exists(fileName))
                {
                    FileStream fs = File.Create(fileName);
                    fs.Close();

                }
            }
            //else
            //{
            //    MyException ex = new MyException("Error! File with this name already exists\n");
            //    throw new FaultException<MyException>(ex);
            //}

        }

        public double MaxRegionConsumation(string fileName, string region)
        {

            double maxTemp = 0;

            double max = 0;   //!

            IPrincipal principal = Thread.CurrentPrincipal;
            if (principal.IsInRole("Reader"))
            {

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
            }
                return max;
        }



        public void ModificationConsumer(string ID, string region, double consamption, string year, string city, string fileName)//treba da se doda u fajl modifikovani potrosac
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            if (principal.IsInRole("Writer"))
            {
                if (DataBase.consumers.ContainsKey(ID))
                {
                    if (File.Exists(fileName))
                    {
                        string s = "";

                        char[] delimiter = { ' ', '\n' };
                        string TextNew = "ID" + " " + ID + " " + "region " + region
                                            + " " + "city " + city + " " + "Year " + " " + year + " "
                                          + " Consumption " + " " + consamption;
                        using (StreamReader sr = File.OpenText(fileName))
                        {
                            while ((s = sr.ReadLine()) != null)
                            {



                                string[] words = s.Split(delimiter);

                                if (Equals(words[1], ID))
                                {
                                    string TextOld = "ID" + " " + DataBase.consumers[ID].ConumerID + " " + "region " + DataBase.consumers[ID].Region
                                                + " " + "city " + DataBase.consumers[ID].City + " " + "Year " + " " + DataBase.consumers[ID].Year + " "
                                              + " Consumption " + " " + DataBase.consumers[ID].Consumation;
                                    string TextAll = File.ReadAllText(fileName);
                                    string TextAllBlank = File.ReadAllText(fileName);
                                    string NewTextAllBlank = TextAllBlank.Replace(TextAllBlank, string.Empty);
                                    string NewTextAll = TextAll.Replace(TextOld, TextNew);


                                    sr.Close();
                                    File.WriteAllText(fileName, NewTextAllBlank);

                                    File.WriteAllText(fileName, NewTextAll);


                                    DataBase.consumers[ID].Region = region;
                                    DataBase.consumers[ID].City = city;
                                    DataBase.consumers[ID].Consumation = consamption;
                                    DataBase.consumers[ID].Year = year;


                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        MyException ex = new MyException("Error!Cant open file\n");
                        throw new FaultException<MyException>(ex);
                    }


                }
            }


        }


        public double RegionConsumtion(string fileName, string region)  //srednja potronja za odredjeni region
        {
            double avg = 0;
            double avgTemp = 0;
            int count = 0;
            IPrincipal principal = Thread.CurrentPrincipal;
            if (principal.IsInRole("Reader"))
            {
                if (!File.Exists(fileName))
                {
                    MyException ex = new MyException("Error! file not found\n");
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
            }
                return Math.Round((avg / count), 2);
        }



        public void RemoveConsumation(string fileName) //admin-pravo uklanjanja baze podataka (fajl-a)
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            if (principal.IsInRole("Admin"))
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
        }

        public void ArchiveConsumation(string fileName)
        {

            string name;
            IPrincipal principal = Thread.CurrentPrincipal;
            if (principal.IsInRole("Admin"))
            {
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
}
