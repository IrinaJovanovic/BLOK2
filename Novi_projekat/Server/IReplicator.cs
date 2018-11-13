using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [ServiceContract]
    public interface IReplicator
    {
        [OperationContract]
        bool SendDelta(Dictionary<string, Consumer> data);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        bool CreateFile();

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void ArchiveConsumation();

        [OperationContract]
        [FaultContract(typeof(MyException))]
        void RemoveConsumation();


    }
}
