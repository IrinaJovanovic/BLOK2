using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IDatabaseManagement
    {
        [OperationContract]
        [FaultContract(typeof(MyException))]
        void CreateFile(string fileName);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void ArchiveConsumation(string fileName);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void RemoveConsumation(string fileName);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void AddConsumer(Consumer c, string fileName);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void ModificationConsumer(string ID, string region, double consamption, string year, string city);

        [OperationContract]
        double CityConsumtion(string fileName, string city);

        [OperationContract]
        double RegionConsumtion(string fileName, string region);

        [OperationContract]
        double MaxRegionConsumation(string fileName, string region);



    }
}
