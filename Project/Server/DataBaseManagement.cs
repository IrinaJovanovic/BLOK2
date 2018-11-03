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
        public void AddConsumer(Consumer c)
        {
           
                if (!DataBase.consumers.ContainsKey(c.ConumerID))
                {
                    DataBase.consumers[c.ConumerID] = c;
                }
                else
                {
                    MyException ex = new MyException("Error! Consumer already exists\n");
                    throw new FaultException<MyException>(ex);
                }
            
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
                }
            else
            {
                MyException ex = new MyException("Error! File with this name already exists\n");
                throw new FaultException<MyException>(ex);
            }
                //else
                //{
                //    char[] delimiter = { ' ', '\n' };

                //    foreach (var item in DataBase.consumers)
                //    {
                //        StreamReader sr = File.OpenText(fileName);
                //        string s = "";

                //        while ((s = sr.ReadLine()) != null)
                //        {
                //            string[] words = s.Split(delimiter);

                //            if (!Equals(words[1], DataBase.consumers.Keys))
                //            {
                //                File.AppendText("ID" + " " + item.Key + " " + "region " + item.Value.Region + " " + "city " + item.Value.City + " " + "Year " + " " + item.Value.Year + " " + " Consumption " + " " + item.Value.Consumation);

                //            }
                //            else
                //            {
                //                MyException ex = new MyException("Error! ID already exists\n");
                //                throw new FaultException<MyException>(ex);
                //            }

                //        }
                //    }
                //}
            
            //throw new NotImplementedException();
        }

        public void ModificationConsumer(string ID, string region, double consamption, string year, string city)
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

        public double ReadConsumation(string fileName, string city, string region)//pitanje
        {
            double avg = 0;
            if (File.Exists(fileName))
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = "";
                    char[] delimiter = { ' ', '\n' };

                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] words = s.Split(delimiter);
                        Console.WriteLine("ID:" + words[1] + "region" + words[3] + "city" + words[5] + "Year" + words[7] + "Consumption" + words[9]);
                        ///vratiti srednju vrednost
                    }

                }
            return avg;
        }

       

        public void RemoveConsumation(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            else
            {

                MyException ex = new MyException("Error! File cannot be find\n");
                throw new FaultException<MyException>(ex);
            }

        }

        public void SaveConsumation(string fileName)//sta radi ovaj da li on samo doda na vec postojeci fail-(realni zivot)
        {
            if (!File.Exists(fileName))
            {

                MyException ex = new MyException("Error! File doesn't exist\n");
                throw new FaultException<MyException>(ex);
            }
            else
            { 
                char[] delimiter = { ' ', '\n' };

                foreach (var item in DataBase.consumers)
                {
                    StreamReader sr = File.OpenText(fileName);
                    string s = "";

                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] words = s.Split(delimiter);

                        if (!Equals(words[1], DataBase.consumers.Keys))
                        {
                            File.AppendText("ID" + " " + item.Key + " " + "region " + item.Value.Region + " " + "city " + item.Value.City + " " + "Year " + " " + item.Value.Year + " " + " Consumption " + " " + item.Value.Consumation);

                        }
                        else
                        {
                            MyException ex = new MyException("Error! ID already exists\n");
                            throw new FaultException<MyException>(ex);
                        }

                    }
                }
            }
        }

    }
}
