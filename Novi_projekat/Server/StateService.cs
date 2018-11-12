using Common;

namespace Server
{
    public class StateService : IStateService
    {
        public static EStateServers stateService = EStateServers.Nepoznato;

        public void StateUpdate(EStateServers state)
        {
            stateService = state;
        }

        public EStateServers StateCheck()
        {
            return stateService;
        }
    }
}
