using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CustomAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            bool authorized = false;

            IPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;

            if (principal != null)
            {
                authorized = ((principal as CustomPrincipal).IsInRole(Permissions.Create.ToString())|| 
                    (principal as CustomPrincipal).IsInRole(Permissions.Deleting.ToString())|| 
                    (principal as CustomPrincipal).IsInRole(Permissions.Arhiving.ToString())||
                    (principal as CustomPrincipal).IsInRole(Permissions.Writting.ToString())|| 
                    (principal as CustomPrincipal).IsInRole(Permissions.Modify.ToString())||
                     (principal as CustomPrincipal).IsInRole(Permissions.ReadingRegionAvgConsumption.ToString()) ||
                    (principal as CustomPrincipal).IsInRole(Permissions.ReadingMaxAvgConsumption.ToString()) ||
                    (principal as CustomPrincipal).IsInRole(Permissions.ReadingCityAvgConsumption.ToString()));
            }

            return authorized;
        }

    }
}
