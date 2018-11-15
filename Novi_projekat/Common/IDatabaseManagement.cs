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
        bool ArchiveConsumation();

        [OperationContract]
        [FaultContract(typeof(MyException))]
        bool RemoveConsumation();

        [OperationContract]
        [FaultContract(typeof(MyException))]
        bool AddConsumer(Consumer c);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        bool ModificationConsumer(Consumer consumer);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        bool CheckIfAlive();

        [OperationContract]
        [FaultContract(typeof(MyException))]
        double CityConsumtion(string city);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        double RegionConsumtion(string region);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        double MaxRegionConsumation(string region);

      
    }
}
