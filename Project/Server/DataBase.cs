using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class DataBase
    {
        public const string FileName = "Baza.txt";
        public static readonly Dictionary<string, Consumer> consumers = new Dictionary<string, Consumer>();
        public static readonly Dictionary<string, Consumer> consumersDelta = new Dictionary<string, Consumer>();
        public static readonly object lockObject = new object();
    }
}
