using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IDataBaseManagement
    {
        [OperationContract]
        [FaultContract(typeof(MyException))]
        void CreateFile();

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void ArchiveConsumation();

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void RemoveConsumation();

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void AddConsumer(Consumer c);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void ModificationConsumer(string ID, string region, string city, string year, double consamption);

        [OperationContract]
        double CityConsumtion(string city);

        [OperationContract]
        double RegionConsumtion(string region);

        [OperationContract]
        double MaxRegionConsumation(string region);

        [OperationContract]
        Dictionary<string, Consumer> UzmiSve(); //PROMENI IME 

        [OperationContract]
        void AddAll(Dictionary<string, Consumer> data);
    }
}
