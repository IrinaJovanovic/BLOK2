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
        bool CreateFile();

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void ArchiveConsumation();

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void RemoveConsumation();

        [OperationContract]
        [FaultContract(typeof(MyException))]
        bool AddConsumer(Consumer c);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        bool ModificationConsumer(Consumer consumer);

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
