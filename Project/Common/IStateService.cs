using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IStateService
    {
        [OperationContract]
        [FaultContract(typeof(MyException))]
        EStateServers ProveraStanja();


        [OperationContract]
        [FaultContract(typeof(MyException))]
        void AzuriranjeStanja(EStateServers stanja);



    }
}
