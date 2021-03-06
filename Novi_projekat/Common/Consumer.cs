﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]

    public class Consumer
    {
        string conumerID;
        string region;
        string city;
        string year;
        double consumation;
        //koji je tip
       public static int counter=0;

       [DataMember]
       DateTime timeStamp;

        public Consumer()
        {
            this.conumerID = "1234";
            this.region = "Vojvodina";
            this.city = "NoviSad";
            this.year = "2018";
            this.consumation = 123.11;
            TimeStamp = DateTime.Now;
        }

        public Consumer(string conumerID, string region, string city, string year, double consumation)
        {
            this.conumerID = conumerID;
            this.region = region;
            this.city = city;
            this.year = year;
            this.consumation = consumation;
            TimeStamp = DateTime.Now;
        }
        
        [DataMember]
        public string ConsumerID { get => conumerID; set => conumerID = value; }
        [DataMember]
        public string Region { get => region; set => region = value; }
        [DataMember]
        public string City { get => city; set => city = value; }
        [DataMember]
        public string Year { get => year; set => year = value; }
        [DataMember]
        public double Consumation { get => consumation; set => consumation = value; }
        public DateTime TimeStamp { get => timeStamp; set => timeStamp = value; }
    }
}
